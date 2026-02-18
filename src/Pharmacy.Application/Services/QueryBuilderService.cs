using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Pharmacy.Infrastructure.Services;

public class QueryBuilderService : IQueryBuilderService
{
    public IQueryable<T> ApplyFilters<T>(IQueryable<T> query, List<FilterRequest> filters)
    {
        if (filters == null || !filters.Any())
            return query;

        foreach (var filter in filters)
        {
            query = ApplyFilter(query, filter);
        }

        return query;
    }

    public IQueryable<T> ApplySorting<T>(IQueryable<T> query, List<SortRequest> sorts)
    {
        if (sorts == null || !sorts.Any())
            return query;

        IOrderedQueryable<T>? orderedQuery = null;

        for (int i = 0; i < sorts.Count; i++)
        {
            var sort = sorts[i];
            var property = GetProperty<T>(sort.SortBy);

            if (property == null) continue;

            if (i == 0)
            {
                orderedQuery = sort.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(CreatePropertyExpression<T>(property))
                    : query.OrderBy(CreatePropertyExpression<T>(property));
            }
            else
            {
                orderedQuery = sort.SortDirection.ToLower() == "desc"
                    ? orderedQuery!.ThenByDescending(CreatePropertyExpression<T>(property))
                    : orderedQuery!.ThenBy(CreatePropertyExpression<T>(property));
            }
        }

        return orderedQuery ?? query;
    }

    public async Task<PagedResult<T>> ApplyPaginationAsync<T>(IQueryable<T> query, PaginationRequest pagination)
    {
        // Apply includes for specific entities before counting/materializing
        query = ApplyIncludes(query);

        var totalRecords = await query.CountAsync();

        if (pagination.GetAll)
        {
            var allData = await query.ToListAsync();
            return PagedResult<T>.Create(allData, totalRecords, 1, totalRecords);
        }

        var pageNumber = Math.Max(1, pagination.PageNumber);
        var pageSize = Math.Max(1, Math.Min(1000, pagination.PageSize)); // Max 1000 records per page

        var skip = (pageNumber - 1) * pageSize;
        var pagedData = await query.Skip(skip).Take(pageSize).ToListAsync();

        return PagedResult<T>.Create(pagedData, totalRecords, pageNumber, pageSize);
    }

    public IQueryable<T> SelectColumns<T>(IQueryable<T> query, List<string> columns)
    {
        // For complex column selection, you might want to use System.Linq.Dynamic.Core
        // For now, we'll return the full query as column selection is complex with strong typing
        return query;
    }

    private IQueryable<T> ApplyIncludes<T>(IQueryable<T> query)
    {
        // Apply specific includes based on entity type
        if (typeof(T) == typeof(AppLookupMaster))
        {
            return query.Cast<AppLookupMaster>()
                .Include(x => x.LookupDetails.Where(d => !d.IsDeleted))
                .Cast<T>();
        }

    

        if (typeof(T) == typeof(SystemUser))
        {
            // For system users, we might want to include related data if needed
            // Currently no navigation properties to include, but can be extended
            return query;
        }

        // Add more include logic for other entities as needed
        return query;
    }

    private IQueryable<T> ApplyFilter<T>(IQueryable<T> query, FilterRequest filter)
    {
        var property = GetProperty<T>(filter.PropertyName);
        if (property == null) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var filterValue = ConvertValue(filter.Value, property.PropertyType);

        Expression? filterExpression = filter.Operation switch
        {
            FilterOperation.Equal => CreateEqualExpression(propertyAccess, filterValue, property.PropertyType),
            FilterOperation.NotEqual => CreateNotEqualExpression(propertyAccess, filterValue, property.PropertyType),
            FilterOperation.Contains => CreateContainsExpression(propertyAccess, filterValue),
            FilterOperation.StartsWith => CreateStartsWithExpression(propertyAccess, filterValue),
            FilterOperation.EndsWith => CreateEndsWithExpression(propertyAccess, filterValue),
            FilterOperation.GreaterThan => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.GreaterThan),
            FilterOperation.LessThan => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.LessThan),
            FilterOperation.GreaterThanOrEqual => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.GreaterThanOrEqual),
            FilterOperation.LessThanOrEqual => CreateComparisonExpression(propertyAccess, filterValue, ExpressionType.LessThanOrEqual),
            FilterOperation.IsNull => CreateNullExpression(propertyAccess, true),
            FilterOperation.IsNotNull => CreateNullExpression(propertyAccess, false),
            FilterOperation.In => CreateInExpression(propertyAccess, filter.Value, property.PropertyType),
            FilterOperation.NotIn => CreateNotInExpression(propertyAccess, filter.Value, property.PropertyType),
            _ => null
        };

        if (filterExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private PropertyInfo? GetProperty<T>(string propertyName)
    {
        return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
    }

    private Expression<Func<T, object>> CreatePropertyExpression<T>(PropertyInfo property)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var objectConversion = Expression.Convert(propertyAccess, typeof(object));
        return Expression.Lambda<Func<T, object>>(objectConversion, parameter);
    }

    private object? ConvertValue(string value, Type targetType)
    {
        if (string.IsNullOrEmpty(value)) return null;

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
        // Handle nullable types properly
        if (value == null)
        {
            return Expression.Equal(property, Expression.Constant(null, propertyType));
        }

        var constant = Expression.Constant(value, propertyType);
        return Expression.Equal(property, constant);
    }

    private Expression CreateNotEqualExpression(Expression property, object? value, Type propertyType)
    {
        // Handle nullable types properly
        if (value == null)
        {
            return Expression.NotEqual(property, Expression.Constant(null, propertyType));
        }

        var constant = Expression.Constant(value, propertyType);
        return Expression.NotEqual(property, constant);
    }

    private Expression CreateContainsExpression(Expression property, object? value)
    {
        if (property.Type != typeof(string) || value is not string stringValue)
            return Expression.Constant(true); // Skip invalid contains operations

        // Handle null check for string properties
        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
        var method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
        var constant = Expression.Constant(stringValue);
        var containsCall = Expression.Call(property, method!, constant);

        return Expression.AndAlso(nullCheck, containsCall);
    }

    private Expression CreateStartsWithExpression(Expression property, object? value)
    {
        if (property.Type != typeof(string) || value is not string stringValue)
            return Expression.Constant(true);

        // Handle null check for string properties
        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
        var method = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });
        var constant = Expression.Constant(stringValue);
        var startsWithCall = Expression.Call(property, method!, constant);

        return Expression.AndAlso(nullCheck, startsWithCall);
    }

    private Expression CreateEndsWithExpression(Expression property, object? value)
    {
        if (property.Type != typeof(string) || value is not string stringValue)
            return Expression.Constant(true);

        // Handle null check for string properties
        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
        var method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) });
        var constant = Expression.Constant(stringValue);
        var endsWithCall = Expression.Call(property, method!, constant);

        return Expression.AndAlso(nullCheck, endsWithCall);
    }

    private Expression CreateComparisonExpression(Expression property, object? value, ExpressionType comparison)
    {
        if (value == null)
            return Expression.Constant(false); // Can't compare with null

        var constant = Expression.Constant(value, property.Type);
        return Expression.MakeBinary(comparison, property, constant);
    }

    private Expression CreateNullExpression(Expression property, bool isNull)
    {
        var nullConstant = Expression.Constant(null, property.Type);
        return isNull ? Expression.Equal(property, nullConstant) : Expression.NotEqual(property, nullConstant);
    }

    private Expression CreateInExpression(Expression property, string values, Type propertyType)
    {
        var valueArray = values.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => ConvertValue(v.Trim(), propertyType))
            .Where(v => v != null)
            .ToArray();

        if (!valueArray.Any())
            return Expression.Constant(false);

        var constantArray = Expression.Constant(valueArray);

        var containsMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(propertyType);

        return Expression.Call(containsMethod, constantArray, property);
    }

    private Expression CreateNotInExpression(Expression property, string values, Type propertyType)
    {
        var inExpression = CreateInExpression(property, values, propertyType);
        return Expression.Not(inExpression);
    }
}