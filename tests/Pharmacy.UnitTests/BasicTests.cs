using FluentAssertions;

namespace Pharmacy.UnitTests;

public class BasicTests
{
    [Fact]
    public void Test_Framework_Is_Working()
    {
        // Arrange
        var expected = true;

        // Act
        var actual = true;

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 3, 5)]
    [InlineData(-1, 1, 0)]
    public void Add_Should_Return_Correct_Sum(int a, int b, int expected)
    {
        // Arrange & Act
        var result = a + b;

        // Assert
        result.Should().Be(expected);
    }
}