using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace Pharmacy.ArchitectureTests;

public class DependencyTests
{
    private static readonly Assembly ApiAssembly = Assembly.Load("Pharmacy.Api");
    private static readonly Assembly ApplicationAssembly = Assembly.Load("Pharmacy.Application");
    private static readonly Assembly DomainAssembly = Assembly.Load("Pharmacy.Domain");
    private static readonly Assembly InfrastructureAssembly = Assembly.Load("Pharmacy.Infrastructure");

    [Fact]
    public void Domain_Should_Not_Depend_On_Any_Other_Project()
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
    public void Application_Should_Not_Depend_On_Infrastructure_Or_Api()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny("Pharmacy.Api", "Pharmacy.Infrastructure")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Api()
    {
        // Arrange & Act
        var result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn("Pharmacy.Api")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_Be_In_Handlers_Namespace()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .ResideInNamespaceStartingWith("Pharmacy.Application.Handlers")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Should_Be_In_Commands_Namespace()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Command")
            .Should()
            .ResideInNamespaceStartingWith("Pharmacy.Application.Commands")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_Should_Be_In_Queries_Namespace()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Query")
            .Should()
            .ResideInNamespaceStartingWith("Pharmacy.Application.Queries")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}