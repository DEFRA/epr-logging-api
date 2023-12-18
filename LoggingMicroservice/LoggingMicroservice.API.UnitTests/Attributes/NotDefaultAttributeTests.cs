namespace LoggingMicroservice.API.UnitTests.Attributes;

using FluentAssertions;
using LoggingMicroservice.API.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class NotDefaultAttributeTests
{
    [TestMethod]
    public void Empty_Guid_Should_Not_Be_Valid()
    {
        // Arrange
        var validator = new NotDefaultAttribute();

        // Act
        var isValid = validator.IsValid(Guid.Empty);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestMethod]
    public void Default_DateTime_Should_Not_Be_Valid()
    {
        // Arrange
        var validator = new NotDefaultAttribute();

        // Act
        var isValid = validator.IsValid(DateTime.MinValue);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestMethod]
    public void Default_Integer_Should_Not_Be_Valid()
    {
        // Arrange
        var validator = new NotDefaultAttribute();

        // Act
        var isValid = validator.IsValid((int)0);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestMethod]
    public void Null_Should_Be_Valid()
    {
        // Arrange
        var validator = new NotDefaultAttribute();

        // Act
        var isValid = validator.IsValid(null);

        // Assert
        isValid.Should().BeTrue();
    }

    [TestMethod]
    public void Default_ReferenceType_Should_Be_Valid()
    {
        // Arrange
        var validator = new NotDefaultAttribute();

        // Act
        var isValid = validator.IsValid(default(object));

        // Assert
        isValid.Should().BeTrue();
    }

    [TestMethod]
    public void Default_String_Should_Be_Valid()
    {
        // Arrange
        var validator = new NotDefaultAttribute();

        // Act
        var isValid = validator.IsValid(string.Empty);

        // Assert
        isValid.Should().BeTrue();
    }
}