using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Pharmacy.Infrastructure.Services;

public class QueryBuilderService : IQueryBuilderService
{
    public IQueryable<T> ApplyFilters<T>(IQueryable<T> query, List<FilterRequest> filters)
    {
        if (filters == null || !filters.Any())
            return query;

        foreach (var f in filters)
        {
            if (f.GroupId.HasValue && f.GroupId.Value == 0)
                f.GroupId = null;
        }

        var hasGroups = filters.Any(f => f.GroupId.HasValue);

        return hasGroups
            ? ApplyGroupedFilters(query, filters)
            : ApplySimpleFilters(query, filters);
    }

    private IQueryable<T> ApplySimpleFilters<T>(IQueryable<T> query, List<FilterRequest> filters)
    {
        if (!filters.Any())
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? combinedExpression = null;

        foreach (var filter in filters)
        {
            var filterExpression = BuildFilterExpression<T>(parameter, filter);

            if (filterExpression == null)
                continue;

            if (combinedExpression == null)
            {
                combinedExpression = filterExpression;
            }
            else
            {
                combinedExpression = filter.LogicalOperator == LogicalOperator.Or
                    ? Expression.OrElse(combinedExpression, filterExpression)
                    : Expression.AndAlso(combinedExpression, filterExpression);
            }
        }

        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private IQueryable<T> ApplyGroupedFilters<T>(IQueryable<T> query, List<FilterRequest> filters)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var groups = filters
            .Where(f => f.GroupId.HasValue)
            .GroupBy(f => f.GroupId!.Value)
            .OrderBy(g => g.Key)
            .ToList();

        var ungroupedFilters = filters.Where(f => !f.GroupId.HasValue).ToList();

        Expression? combinedExpression = null;

        foreach (var filter in ungroupedFilters)
        {
            var filterExpression = BuildFilterExpression<T>(parameter, filter);

            if (filterExpression == null)
                continue;

            combinedExpression = combinedExpression == null
                ? filterExpression
                : filter.LogicalOperator == LogicalOperator.Or
                    ? Expression.OrElse(combinedExpression, filterExpression)
                    : Expression.AndAlso(combinedExpression, filterExpression);
        }

        foreach (var group in groups)
        {
            Expression? groupExpression = null;
            var groupFilters = group.ToList();

            foreach (var filter in groupFilters)
            {
                var filterExpression = BuildFilterExpression<T>(parameter, filter);

                if (filterExpression == null)
                    continue;

                groupExpression = groupExpression == null
                    ? filterExpression
                    : filter.LogicalOperator == LogicalOperator.Or
                        ? Expression.OrElse(groupExpression, filterExpression)
                        : Expression.AndAlso(groupExpression, filterExpression);
            }

            if (groupExpression == null)
                continue;

            if (combinedExpression == null)
            {
                combinedExpression = groupExpression;
            }
            else
            {
                var firstFilterInGroup = groupFilters.FirstOrDefault();

                combinedExpression = firstFilterInGroup?.LogicalOperator == LogicalOperator.Or
                    ? Expression.OrElse(combinedExpression, groupExpression)
                    : Expression.AndAlso(combinedExpression, groupExpression);
            }
        }

        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private Expression? BuildFilterExpression<T>(ParameterExpression parameter, FilterRequest filter)
    {
        if (string.IsNullOrWhiteSpace(filter.PropertyName))
            return null;

        if (filter.PropertyName.Contains('.'))
        {
            var collectionExpression = TryBuildCollectionAnyExpression<T>(parameter, filter);

            if (collectionExpression != null)
                return collectionExpression;
        }

        var (propertyAccess, propertyType) = BuildPropertyAccess<T>(parameter, filter.PropertyName);

        if (propertyAccess == null || propertyType == null)
        {
            var aliasMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "drugname", "Items.Product.DrugName" },
                { "drugnamear", "Items.Product.DrugName" },
                { "genericname", "Items.Product.GenericName" },
                { "genericnameen", "Items.Product.GenericNameEN" },
                { "price", "Items.Product.Price" },
                { "gtin", "Items.Product.GTIN" },
                { "barcode", "Items.Product.GTIN" }
            };

            if (aliasMap.TryGetValue(filter.PropertyName.Trim(), out var mapped))
            {
                var mappedFilter = new FilterRequest
                {
                    PropertyName = mapped,
                    Value = filter.Value,
                    Operation = filter.Operation,
                    LogicalOperator = filter.LogicalOperator,
                    GroupId = filter.GroupId
                };

                return BuildFilterExpression<T>(parameter, mappedFilter);
            }

            return null;
        }

        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        var isNullable = Nullable.GetUnderlyingType(propertyType) != null;

        var isDateTimeFilter =
            underlyingType == typeof(DateTime) &&
            filter.Operation is FilterOperation.Equal
                or FilterOperation.NotEqual
                or FilterOperation.GreaterThan
                or FilterOperation.LessThan
                or FilterOperation.GreaterThanOrEqual
                or FilterOperation.LessThanOrEqual;

        if (isDateTimeFilter)
        {
            if (!DateTime.TryParse(filter.Value, out var parsedDate))
                return null;

            var nonNullableAccess = isNullable
                ? Expression.Property(propertyAccess, "Value")
                : propertyAccess;

            var dateProperty = Expression.Property(nonNullableAccess, nameof(DateTime.Date));
            var dateConstant = Expression.Constant(parsedDate.Date, typeof(DateTime));

            Expression comparison = filter.Operation switch
            {
                FilterOperation.Equal => Expression.Equal(dateProperty, dateConstant),
                FilterOperation.NotEqual => Expression.NotEqual(dateProperty, dateConstant),
                FilterOperation.GreaterThan => Expression.GreaterThan(dateProperty, dateConstant),
                FilterOperation.LessThan => Expression.LessThan(dateProperty, dateConstant),
                FilterOperation.GreaterThanOrEqual => Expression.GreaterThanOrEqual(dateProperty, dateConstant),
                FilterOperation.LessThanOrEqual => Expression.LessThanOrEqual(dateProperty, dateConstant),
                _ => Expression.Constant(false)
            };

            if (isNullable)
            {
                var hasValue = Expression.Property(propertyAccess, "HasValue");
                comparison = Expression.AndAlso(hasValue, comparison);
            }

            return comparison;
        }

        var filterValue = ConvertValue(filter.Value, propertyType);

        return filter.Operation switch
        {
            FilterOperation.Equal => CreateEqualExpression(propertyAccess, filterValue, propertyType),
            FilterOperation.NotEqual => CreateNotEqualExpression(propertyAccess, filterValue, propertyType),
            FilterOperation.Contains => CreateContainsExpression(propertyAccess, filterValue),
            FilterOperation.StartsWith => CreateStartsWithExpression(propertyAccess, filterValue),
            FilterOperation.EndsWith => CreateEndsWithExpression(propertyAccess, filterValue),
            FilterOperation.GreaterThan => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.GreaterThan),
            FilterOperation.LessThan => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.LessThan),
            FilterOperation.GreaterThanOrEqual => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.GreaterThanOrEqual),
            FilterOperation.LessThanOrEqual => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.LessThanOrEqual),
            FilterOperation.IsNull => CreateNullExpression(propertyAccess, true),
            FilterOperation.IsNotNull => CreateNullExpression(propertyAccess, false),
            FilterOperation.In => CreateInExpression(propertyAccess, filter.Value, propertyType),
            FilterOperation.NotIn => CreateNotInExpression(propertyAccess, filter.Value, propertyType),
            _ => null
        };
    }

    private Expression? TryBuildCollectionAnyExpression<T>(ParameterExpression parameter, FilterRequest filter)
    {
        var parts = filter.PropertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);

        Expression current = parameter;
        Type currentType = typeof(T);

        for (int i = 0; i < parts.Length; i++)
        {
            var prop = currentType.GetProperty(parts[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop == null)
                return null;

            var propAccess = Expression.Property(current, prop);
            var propType = prop.PropertyType;

            var isCollection =
                propType != typeof(string) &&
                typeof(System.Collections.IEnumerable).IsAssignableFrom(propType);

            if (!isCollection)
            {
                current = propAccess;
                currentType = propType;
                continue;
            }

            Type? elementType = propType.IsArray
                ? propType.GetElementType()
                : propType.IsGenericType
                    ? propType.GetGenericArguments().FirstOrDefault()
                    : null;

            if (elementType == null)
                return null;

            var remainingParts = parts.Skip(i + 1).ToArray();

            if (!remainingParts.Any())
                return null;

            var innerParam = Expression.Parameter(elementType, "y");
            Expression innerCurrent = innerParam;
            Type innerType = elementType;

            foreach (var part in remainingParts)
            {
                var innerProp = innerType.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (innerProp == null)
                    return null;

                innerCurrent = Expression.Property(innerCurrent, innerProp);
                innerType = innerProp.PropertyType;
            }

            var innerFilterValue = ConvertValue(filter.Value, innerType);

            Expression? innerComparison = filter.Operation switch
            {
                FilterOperation.Equal => CreateEqualExpression(innerCurrent, innerFilterValue, innerType),
                FilterOperation.NotEqual => CreateNotEqualExpression(innerCurrent, innerFilterValue, innerType),
                FilterOperation.Contains => CreateContainsExpression(innerCurrent, innerFilterValue),
                FilterOperation.StartsWith => CreateStartsWithExpression(innerCurrent, innerFilterValue),
                FilterOperation.EndsWith => CreateEndsWithExpression(innerCurrent, innerFilterValue),
                FilterOperation.GreaterThan => CreateComparisonExpression(innerCurrent, innerFilterValue, ExpressionType.GreaterThan),
                FilterOperation.LessThan => CreateComparisonExpression(innerCurrent, innerFilterValue, ExpressionType.LessThan),
                FilterOperation.GreaterThanOrEqual => CreateComparisonExpression(innerCurrent, innerFilterValue, ExpressionType.GreaterThanOrEqual),
                FilterOperation.LessThanOrEqual => CreateComparisonExpression(innerCurrent, innerFilterValue, ExpressionType.LessThanOrEqual),
                FilterOperation.IsNull => CreateNullExpression(innerCurrent, true),
                FilterOperation.IsNotNull => CreateNullExpression(innerCurrent, false),
                FilterOperation.In => CreateInExpression(innerCurrent, filter.Value, innerType),
                FilterOperation.NotIn => CreateNotInExpression(innerCurrent, filter.Value, innerType),
                _ => null
            };

            if (innerComparison == null)
                return null;

            var innerLambda = Expression.Lambda(innerComparison, innerParam);

            var anyMethod = typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 2)
                .MakeGenericMethod(elementType);

            return Expression.Call(anyMethod, propAccess, innerLambda);
        }

        return null;
    }

    public IQueryable<T> ApplySorting<T>(IQueryable<T> query, List<SortRequest> sorts)
    {
        if (sorts == null || !sorts.Any())
            return query;

        IOrderedQueryable<T>? orderedQuery = null;

        for (int i = 0; i < sorts.Count; i++)
        {
            var sort = sorts[i];

            var parameter = Expression.Parameter(typeof(T), "x");
            var (memberExpr, propType) = BuildPropertyAccess<T>(parameter, sort.SortBy);

            if (memberExpr == null || propType == null)
                continue;

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), propType);
            var lambda = Expression.Lambda(delegateType, memberExpr, parameter);

            var methodName = i == 0
                ? sort.SortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true
                    ? nameof(Queryable.OrderByDescending)
                    : nameof(Queryable.OrderBy)
                : sort.SortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true
                    ? nameof(Queryable.ThenByDescending)
                    : nameof(Queryable.ThenBy);

            var method = typeof(Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propType);

            orderedQuery = i == 0
                ? (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, lambda })!
                : (IOrderedQueryable<T>)method.Invoke(null, new object[] { orderedQuery!, lambda })!;
        }

        return orderedQuery ?? query;
    }

    public async Task<PagedResult<T>> ApplyPaginationAsync<T>(IQueryable<T> query, PaginationRequest pagination)
    {
        query = ApplyIncludes(query);

        var totalRecords = await query.CountAsync();

        if (pagination.GetAll)
        {
            var allData = await query.ToListAsync();
            return PagedResult<T>.Create(allData, totalRecords, 1, totalRecords);
        }

        var pageNumber = Math.Max(1, pagination.PageNumber);
        var pageSize = Math.Max(1, Math.Min(1000, pagination.PageSize));

        var skip = (pageNumber - 1) * pageSize;
        var pagedData = await query.Skip(skip).Take(pageSize).ToListAsync();

        return PagedResult<T>.Create(pagedData, totalRecords, pageNumber, pageSize);
    }

    public IQueryable<T> SelectColumns<T>(IQueryable<T> query, List<string> columns)
    {
        if (columns == null || !columns.Any())
            return query;

        var entityType = typeof(T);
        var validColumns = new List<string>();

        foreach (var column in columns)
        {
            var property = entityType.GetProperty(column, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property != null)
                validColumns.Add(property.Name);
        }

        return query;
    }

    public async Task<PagedResult<Dictionary<string, object>>> ApplyPaginationWithColumnsAsync<T>(
        IQueryable<T> query,
        PaginationRequest pagination,
        List<string> columns)
    {
        query = ApplyIncludes(query);

        var entityType = typeof(T);
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        List<PropertyInfo> selectedProperties;

        if (columns == null || !columns.Any())
        {
            selectedProperties = properties.ToList();
        }
        else
        {
            selectedProperties = new List<PropertyInfo>();

            foreach (var column in columns)
            {
                var property = properties.FirstOrDefault(p =>
                    p.Name.Equals(column, StringComparison.OrdinalIgnoreCase));

                if (property != null)
                    selectedProperties.Add(property);
            }

            if (!selectedProperties.Any())
                selectedProperties = properties.ToList();
        }

        if (!IsOrdered(query) && selectedProperties.Any())
        {
            var firstProp = selectedProperties.First();

            var orderParam = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(orderParam, firstProp);
            var orderLambda = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(entityType, firstProp.PropertyType),
                propertyAccess,
                orderParam);

            var orderMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == nameof(Queryable.OrderBy) && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, firstProp.PropertyType);

            query = (IQueryable<T>)orderMethod.Invoke(null, new object[] { query, orderLambda })!;
        }

        var totalRecords = await query.CountAsync();

        var parameter = Expression.Parameter(entityType, "x");

        var elementExprs = selectedProperties
            .Select(p => (Expression)Expression.Convert(Expression.Property(parameter, p), typeof(object)))
            .ToArray();

        var newArray = Expression.NewArrayInit(typeof(object), elementExprs);
        var selector = Expression.Lambda<Func<T, object[]>>(newArray, parameter);

        var projectionQuery = query.Select(selector);

        int pageNumber;
        int pageSize;
        IQueryable<object[]> pagedQuery;

        if (pagination.GetAll)
        {
            pagedQuery = projectionQuery;
            pageNumber = 1;
            pageSize = totalRecords;
        }
        else
        {
            pageNumber = Math.Max(1, pagination.PageNumber);
            pageSize = Math.Max(1, Math.Min(1000, pagination.PageSize));

            var skip = (pageNumber - 1) * pageSize;
            pagedQuery = projectionQuery.Skip(skip).Take(pageSize);
        }

        var raw = await pagedQuery.ToListAsync();

        var result = raw.Select(arr =>
        {
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < selectedProperties.Count; i++)
                dict[selectedProperties[i].Name] = arr[i];

            return dict;
        }).ToList();

        return PagedResult<Dictionary<string, object>>.Create(result, totalRecords, pageNumber, pageSize);
    }

    private bool IsOrdered<T>(IQueryable<T> query)
    {
        return query.Expression.ToString().Contains("OrderBy") ||
               query.Expression.ToString().Contains("ThenBy");
    }

    public async Task<PagedResult<object>> ExecuteQueryAsync<T>(IQueryable<T> query, DataRequest request)
    {
        query = ApplyFilters(query, request.Filters);
        query = ApplySorting(query, request.Sort);

        if (request.Columns != null && request.Columns.Count > 0)
        {
            var pagedDictResult = await ApplyPaginationWithColumnsAsync(query, request.Pagination, request.Columns);

            var objectList = pagedDictResult.Data.Cast<object>().ToList();

            return PagedResult<object>.Create(
                objectList,
                pagedDictResult.TotalRecords,
                pagedDictResult.PageNumber,
                pagedDictResult.PageSize);
        }

        var pagedResult = await ApplyPaginationAsync(query, request.Pagination);
        var fullObjectList = pagedResult.Data.Cast<object>().ToList();

        return PagedResult<object>.Create(
            fullObjectList,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }

    private IQueryable<T> ApplyIncludes<T>(IQueryable<T> query)
    {
        if (typeof(T) == typeof(AppLookupMaster))
        {
            return query.Cast<AppLookupMaster>()
                .Include(x => x.LookupDetails.Where(d => !d.IsDeleted))
                .Cast<T>();
        }

        if (typeof(T) == typeof(SystemUser))
        {
            return query;
        }

        return query;
    }

    private (Expression? memberExpression, Type? propertyType) BuildPropertyAccess<T>(
        ParameterExpression parameter,
        string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return (null, null);

        Expression current = parameter;
        Type currentType = typeof(T);

        var parts = propertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var prop = currentType.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop == null)
                return (null, null);

            current = Expression.Property(current, prop);
            currentType = prop.PropertyType;
        }

        return (current, currentType);
    }

    private object? ConvertValue(string value, Type targetType)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        try
        {
            if (underlyingType == typeof(string))
                return value;

            if (underlyingType == typeof(Guid))
                return Guid.Parse(value);

            if (underlyingType == typeof(DateTime))
                return DateTime.Parse(value);

            if (underlyingType == typeof(DateOnly))
                return DateOnly.Parse(value);

            if (underlyingType == typeof(TimeOnly))
                return TimeOnly.Parse(value);

            if (underlyingType == typeof(bool))
                return bool.Parse(value);

            if (underlyingType == typeof(char))
                return char.Parse(value);

            if (underlyingType == typeof(int))
                return int.Parse(value);

            if (underlyingType == typeof(long))
                return long.Parse(value);

            if (underlyingType == typeof(decimal))
                return decimal.Parse(value);

            if (underlyingType == typeof(double))
                return double.Parse(value);

            if (underlyingType == typeof(float))
                return float.Parse(value);

            return Convert.ChangeType(value, underlyingType);
        }
        catch
        {
            return null;
        }
    }

    private Expression CreateEqualExpression(Expression property, object? value, Type propertyType)
    {
        if (value == null)
            return Expression.Equal(property, Expression.Constant(null, propertyType));

        var constant = Expression.Constant(value, propertyType);
        return Expression.Equal(property, constant);
    }

    private Expression CreateNotEqualExpression(Expression property, object? value, Type propertyType)
    {
        if (value == null)
            return Expression.NotEqual(property, Expression.Constant(null, propertyType));

        var constant = Expression.Constant(value, propertyType);
        return Expression.NotEqual(property, constant);
    }

    private Expression? CreateContainsExpression(Expression property, object? value)
    {
        if (property.Type != typeof(string) || value is not string stringValue)
            return null;

        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
        var method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
        var constant = Expression.Constant(stringValue);
        var containsCall = Expression.Call(property, method!, constant);

        return Expression.AndAlso(nullCheck, containsCall);
    }

    private Expression? CreateStartsWithExpression(Expression property, object? value)
    {
        if (property.Type != typeof(string) || value is not string stringValue)
            return null;

        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
        var method = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });
        var constant = Expression.Constant(stringValue);
        var startsWithCall = Expression.Call(property, method!, constant);

        return Expression.AndAlso(nullCheck, startsWithCall);
    }

    private Expression? CreateEndsWithExpression(Expression property, object? value)
    {
        if (property.Type != typeof(string) || value is not string stringValue)
            return null;

        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
        var method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) });
        var constant = Expression.Constant(stringValue);
        var endsWithCall = Expression.Call(property, method!, constant);

        return Expression.AndAlso(nullCheck, endsWithCall);
    }

    private Expression? CreateComparisonExpression(Expression property, object? value, ExpressionType comparison)
    {
        if (value == null)
            return null;

        var constant = Expression.Constant(value, property.Type);
        return Expression.MakeBinary(comparison, property, constant);
    }

    private Expression CreateNullExpression(Expression property, bool isNull)
    {
        var nullConstant = Expression.Constant(null, property.Type);
        return isNull
            ? Expression.Equal(property, nullConstant)
            : Expression.NotEqual(property, nullConstant);
    }

    private Expression? CreateInExpression(Expression property, string values, Type propertyType)
    {
        var valueArray = values
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => ConvertValue(v.Trim(), propertyType))
            .Where(v => v != null)
            .ToArray();

        if (!valueArray.Any())
            return null;

        var constantArray = Expression.Constant(valueArray);

        var containsMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
            .MakeGenericMethod(propertyType);

        return Expression.Call(containsMethod, constantArray, property);
    }

    private Expression? CreateNotInExpression(Expression property, string values, Type propertyType)
    {
        var inExpression = CreateInExpression(property, values, propertyType);

        return inExpression == null
            ? null
            : Expression.Not(inExpression);
    }
}