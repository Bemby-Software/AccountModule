using System;
using System.Threading.Tasks;
using Bemby.AccountModule.Application.Interfaces.Repositories;
using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Application.Services;
using Bemby.AccountModule.Domain.DTOs;
using Bemby.AccountModule.Domain.Entities;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;

namespace Bemby.AccountModule.Application.Tests.Services
{
    [TestFixture]
    public class AccountServiceTests
    {
        private AutoMocker _mocker;
        private readonly string _email = "test@test.com";
        private readonly string _mobileNumber = "07703123123";
        private readonly string _password = "password";
        private readonly string _hashedPassword = "hashed";
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public async Task CreateAccountAsync_Valid()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();

            _mocker.GetMock<IAccountRepository>().Setup(ar => ar.DoesAccountExistAsync(_email))
                .Returns(Task.FromResult(false));

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.IsStrongPassword(_password))
                .Returns(true);

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.HashPassword(_password, It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(_hashedPassword);

            _mocker.GetMock<IEmailService>().Setup(es => es.IsValidEmailAddress(_email))
                .Returns(true);
            
            _mocker.GetMock<IPhoneService>().Setup(ps => ps.IsValidPhoneNumber(_mobileNumber))
                .Returns(true);

            
            //Act
            var accountEntity = await serviceUnderTest.CreateAccountAsync(new CreateAccountDto(_email,  _mobileNumber, _password));

            //Assert
            _mocker.GetMock<IAccountRepository>()
                   .Verify(ar => ar.DoesAccountExistAsync(_email));
            
            _mocker.GetMock<IAccountRepository>()
                   .Verify(ar => ar.CreateAccountAsync(It.Is<AccountEntity>(ae => ae.Email == _email
                                                                                                            && ae.HashedPassword == _hashedPassword
                                                                                                            && ae.MobileNumber == _mobileNumber)));
            accountEntity.ShouldNotBeNull();
            accountEntity.Id.ShouldNotBe(Guid.Empty);
            accountEntity.Email.ShouldBe(_email);
            accountEntity.MobileNumber.ShouldBe(_mobileNumber);
            accountEntity.HashedPassword.ShouldBe(_hashedPassword);
        }
        
        [Test]
        public async Task CreateAccountAsync_AccountExists()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();

            _mocker.GetMock<IAccountRepository>().Setup(ar => ar.DoesAccountExistAsync(_email))
                .Returns(Task.FromResult(true));

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.IsStrongPassword(_password))
                .Returns(true);

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.HashPassword(_password, It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(_hashedPassword);

            _mocker.GetMock<IEmailService>().Setup(es => es.IsValidEmailAddress(_email))
                .Returns(true);
            
            _mocker.GetMock<IPhoneService>().Setup(ps => ps.IsValidPhoneNumber(_mobileNumber))
                .Returns(true);

            //Act
            var accountEntity = await serviceUnderTest.CreateAccountAsync(new CreateAccountDto(_email,  _mobileNumber, _password));

            //Assert
            _mocker.GetMock<IAccountRepository>()
                .Verify(ar => ar.DoesAccountExistAsync(_email));

            _mocker.GetMock<IAccountRepository>()
                .Verify(ar => ar.CreateAccountAsync(It.Is<AccountEntity>(ae => ae.Email == _email
                                                                               && ae.HashedPassword == _hashedPassword
                                                                               && ae.MobileNumber == _mobileNumber)), Times.Never);
            
            accountEntity.ShouldBeNull();
        }
        
        [Test]
        public async Task CreateAccountAsync_WeakPassword()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();

            _mocker.GetMock<IAccountRepository>().Setup(ar => ar.DoesAccountExistAsync(_email))
                .Returns(Task.FromResult(false));

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.IsStrongPassword(_password))
                .Returns(false);

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.HashPassword(_password, It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(_hashedPassword);

            _mocker.GetMock<IEmailService>().Setup(es => es.IsValidEmailAddress(_email))
                .Returns(true);
            
            _mocker.GetMock<IPhoneService>().Setup(ps => ps.IsValidPhoneNumber(_mobileNumber))
                .Returns(true);
            
            //Act
            var accountEntity = await serviceUnderTest.CreateAccountAsync(new CreateAccountDto(_email,  _mobileNumber, _password));

            //Assert
            _mocker.GetMock<IPasswordService>()
                .Verify(ps => ps.IsStrongPassword(_password));
            
            _mocker.GetMock<IAccountRepository>()
                .Verify(ar => ar.CreateAccountAsync(It.Is<AccountEntity>(ae => ae.Email == _email
                                                                               && ae.HashedPassword == _hashedPassword
                                                                               && ae.MobileNumber == _mobileNumber)), Times.Never);
            
            accountEntity.ShouldBeNull();
        }
        
        [Test]
        public async Task CreateAccountAsync_InvalidEmail()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();

            _mocker.GetMock<IAccountRepository>().Setup(ar => ar.DoesAccountExistAsync(_email))
                .Returns(Task.FromResult(false));

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.IsStrongPassword(_password))
                .Returns(true);

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.HashPassword(_password, It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(_hashedPassword);

            _mocker.GetMock<IEmailService>().Setup(es => es.IsValidEmailAddress(_email))
                .Returns(false);
            
            _mocker.GetMock<IPhoneService>().Setup(ps => ps.IsValidPhoneNumber(_mobileNumber))
                .Returns(true);
            
            //Act
            var accountEntity = await serviceUnderTest.CreateAccountAsync(new CreateAccountDto(_email,  _mobileNumber, _password));

            //Assert
            _mocker.GetMock<IEmailService>()
                .Verify(es => es.IsValidEmailAddress(_email));
            
            _mocker.GetMock<IAccountRepository>()
                .Verify(ar => ar.CreateAccountAsync(It.Is<AccountEntity>(ae => ae.Email == _email
                                                                               && ae.HashedPassword == _hashedPassword
                                                                               && ae.MobileNumber == _mobileNumber)), Times.Never);
            
            accountEntity.ShouldBeNull();
        }
        
        [Test]
        public async Task CreateAccountAsync_InvalidMobileNumber()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();

            _mocker.GetMock<IAccountRepository>().Setup(ar => ar.DoesAccountExistAsync(_email))
                .Returns(Task.FromResult(false));

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.IsStrongPassword(_password))
                .Returns(true);

            _mocker.GetMock<IPasswordService>().Setup(ps => ps.HashPassword(_password, It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(_hashedPassword);

            _mocker.GetMock<IEmailService>().Setup(es => es.IsValidEmailAddress(_email))
                .Returns(true);
            
            _mocker.GetMock<IPhoneService>().Setup(ps => ps.IsValidPhoneNumber(_mobileNumber))
                .Returns(false);
            
            //Act
            var accountEntity = await serviceUnderTest.CreateAccountAsync(new CreateAccountDto(_email,  _mobileNumber, _password));

            //Assert
            _mocker.GetMock<IPhoneService>()
                .Verify(es => es.IsValidPhoneNumber(_mobileNumber));
            
            _mocker.GetMock<IAccountRepository>()
                .Verify(ar => ar.CreateAccountAsync(It.Is<AccountEntity>(ae => ae.Email == _email
                                                                               && ae.HashedPassword == _hashedPassword
                                                                               && ae.MobileNumber == _mobileNumber)), Times.Never);
            
            accountEntity.ShouldBeNull();
        }
        
        private IAccountService CreateServiceUnderTest() => _mocker.CreateInstance<AccountService>();
    }
}