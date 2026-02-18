using FluentAssertions;
using Pharmacy.Api.Controllers;
using NetArchTest.Rules;
using System.Reflection;

namespace Pharmacy.ArchitectureTests;

public class ControllerArchitectureTests
{
    private static readonly Assembly ApiAssembly = Assembly.Load("Pharmacy.Api");
    private static readonly Assembly ApplicationAssembly = Assembly.Load("Pharmacy.Application");
    private static readonly Assembly DomainAssembly = Assembly.Load("Pharmacy.Domain");
    private static readonly Assembly InfrastructureAssembly = Assembly.Load("Pharmacy.Infrastructure");

    [Fact]
    public void Controllers_Should_Have_Controller_Suffix()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("Pharmacy.Api.Controllers")
            .And()
            .AreClasses()
            .Should()
            .HaveNameEndingWith("Controller")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_Inherit_From_BaseApiController()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApiAssembly)
       .That()
       .ResideInNamespace("Pharmacy.Api.Controllers")
       .And()
       .AreClasses()
       .And()
       .DoNotHaveName("BaseApiController")
       .Should()
       .Inherit(typeof(BaseApiController))
       .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_Not_Reference_Infrastructure()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("Pharmacy.Api.Controllers")
            .Should()
            .NotHaveDependencyOn("Pharmacy.Infrastructure")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_Use_MediatR_For_Business_Logic()
    {
        // Arrange & Act
        var controllerTypes = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("Pharmacy.Api.Controllers")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveName("BaseApiController")
            .GetTypes();

        // Assert
        foreach (var controllerType in controllerTypes)
        {
            var constructors = controllerType.GetConstructors();
            var usesMediatR = constructors.Any(c => 
                c.GetParameters().Any(p => 
                    p.ParameterType.FullName != null && 
                    p.ParameterType.FullName.Contains("MediatR")));
            
            usesMediatR.Should().BeTrue($"{controllerType.Name} should use MediatR for business logic");
        }
    }

    [Fact]
    public void Application_Should_Not_Reference_Infrastructure()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn("Pharmacy.Infrastructure")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_Not_Have_Dependencies_On_Other_Layers()
    {
        // Arrange & Act
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny("Pharmacy.Api", "Pharmacy.Application", "Pharmacy.Infrastructure")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_Be_Public()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("Pharmacy.Api.Controllers")
            .And()
            .AreClasses()
            .Should()
            .BePublic()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_Have_Route_Attribute()
    {
        // Arrange & Act
        var controllerTypes = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("Pharmacy.Api.Controllers")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveName("BaseApiController")
            .GetTypes();

        // Assert
        foreach (var controllerType in controllerTypes)
        {
            var hasRouteAttribute = controllerType.GetCustomAttributes()
                .Any(attr => attr.GetType().Name.Contains("Route"));
            
            hasRouteAttribute.Should().BeTrue($"{controllerType.Name} should have Route attribute");
        }
    }
}