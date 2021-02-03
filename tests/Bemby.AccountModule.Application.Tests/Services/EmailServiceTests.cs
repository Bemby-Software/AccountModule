using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Application.Services;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;

namespace Bemby.AccountModule.Application.Tests.Services
{
    [TestFixture]
    public class EmailServiceTests
    {
        private AutoMocker _mocker;
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }
        
        [TestCase("test@gmail.com")]
        [TestCase("test@test.karoo.co.uk")]
        [TestCase("test@sub.domain.ext")]
        [TestCase("email @example.com")]
        [TestCase("firstname.lastname @example.com")]
        [TestCase("email@subdomain.example.com")]
        [TestCase("firstname +lastname @example.com")]
        [TestCase("email@123.123.123.123")]
        [TestCase("email@[123.123.123.123]")]
        [TestCase("\"email\"@example.com")]
        [TestCase("1234567890@example.com")]
        [TestCase("email@example-one.com")]
        [TestCase("_______@example.com")]
        [TestCase("email@example.name")]
        [TestCase("email@example.museum")]
        [TestCase("email@example.co.jp")]
        [TestCase("firstname -lastname @example.com")]
        public void IsEmailValid_True(string email)
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            var isEmailValid = serviceUnderTest.IsValidEmailAddress(email);
            
            //Assert
            isEmailValid.ShouldBe(true);
        }

        [TestCase("plainaddress")]
        [TestCase("#@%^%#$@#$@#.com")]
        [TestCase("@example.com")]
        [TestCase("email.example.com")]
        [TestCase("email@example @example.com")]
        [TestCase(" ")]
        [TestCase(null)]
        public void IsEmailValid_False(string email)
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            var isEmailValid = serviceUnderTest.IsValidEmailAddress(email);
            
            //Assert
            isEmailValid.ShouldBe(false);
        }
        
        private IEmailService CreateServiceUnderTest() => _mocker.CreateInstance<EmailService>();
    }
}