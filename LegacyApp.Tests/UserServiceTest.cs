using System;
using JetBrains.Annotations;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{
    private readonly UserService _userService;
    private readonly UserTest _userTest;

    public UserServiceTest()
    {
        _userService = new UserService();
        _userTest = new UserTest{ FirstName = "Joe", LastName = "Doe", EmailAddress = "johndoe@gmail.com", DateOfBirth = DateTime.Parse("1982-03-21"), ClientId = 1};
    }
    
    
    [Fact]
    public void AddUser_FirstNameIsNull_ReturnFalse()
    {
        //Arrange
        _userTest.FirstName = "";
        // Act
        var testResult = _userService.AddUser(
            _userTest.FirstName,
            _userTest.LastName,
            _userTest.EmailAddress,
            _userTest.DateOfBirth,
            _userTest.ClientId
        );
        // Assert
        Assert.False(testResult);
    }
    [Fact]
    public void AddUser_SecondNameIsNull_ReturnFalse()
    {
        // Arrange
        _userTest.LastName = "";
        // Act
        var testResult = _userService.AddUser(
            _userTest.FirstName,
            _userTest.LastName,
            _userTest.EmailAddress,
            _userTest.DateOfBirth,
            _userTest.ClientId
        );
        // Assert
        Assert.False(testResult);
    }
    [Fact]
    public void AddUser_EmailIsInvalid_ReturnFalse()
    {
        // Arrange
        _userTest.EmailAddress = "johndoegmail.com";
        // Act
        var testResult = _userService.AddUser(
            _userTest.FirstName,
            _userTest.LastName,
            _userTest.EmailAddress,
            _userTest.DateOfBirth,
            _userTest.ClientId
        );
        // Assert
        Assert.False(testResult);
    }
    [Fact]
    public void AddUser_IsOverCreditLimit_ReturnFalse()
    {
        // Arrange
        _userTest.ClientId = 1;
        _userTest.LastName = "Kowalski";
        // Act
        var testResult = _userService.AddUser(
            _userTest.FirstName,
            _userTest.LastName,
            _userTest.EmailAddress,
            _userTest.DateOfBirth,
            _userTest.ClientId
        );
        // Assert
        Assert.False(testResult);
    }
    [Fact]
    public void AddUser_IsSuccessful_ReturnTrue()
    {
        // Act
        var testResult = _userService.AddUser(
            _userTest.FirstName,
            _userTest.LastName,
            _userTest.EmailAddress,
            _userTest.DateOfBirth,
            _userTest.ClientId
        );
        // Assert
        Assert.True(testResult);
    }

    [Fact]
    public void AddUser_Should_Throw_ArgumentException_When_Client_Id_Doesnt_Exist()
    {
        // Arrange
        _userTest.ClientId = -1;
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _userService.AddUser(
                _userTest.FirstName,
                _userTest.LastName,
                _userTest.EmailAddress,
                _userTest.DateOfBirth,
                _userTest.ClientId
            );
        });
       
    }
    [Fact]
    public void AddUser_Should_Throw_ArgumentException_When_Last_Name_Doesnt_Exist()
    {
        // Arrange
        _userTest.LastName = "Wdawdawda";
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _userService.AddUser(
                _userTest.FirstName,
                _userTest.LastName,
                _userTest.EmailAddress,
                _userTest.DateOfBirth,
                _userTest.ClientId
            );
        });
       
    }
}