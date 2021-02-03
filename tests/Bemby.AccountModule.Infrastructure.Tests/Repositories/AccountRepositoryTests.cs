using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bemby.AccountModule.Application.Interfaces.Repositories;
using Bemby.AccountModule.Domain.Entities;
using Bemby.AccountModule.Infrastructure.Documents;
using Bemby.AccountModule.Infrastructure.Repositories;
using Convey.Persistence.MongoDB;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;

namespace Bemby.AccountModule.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class AccountRepositoryTests
    {
        private AutoMocker _mocker;
        private readonly Guid _id = Guid.NewGuid();
        private readonly string _email = "test@test.com";
        private readonly string _mobileNumber = "07703123123";
        private readonly string _hashedPassword = "hashed";

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public async Task CreateAccountAsync()
        {
            //Arrange
            var accountEntityMock = new Mock<IAccountEntity>();
            accountEntityMock.SetupGet(o => o.Id).Returns(_id);
            accountEntityMock.SetupGet(o => o.Email).Returns(_email);
            accountEntityMock.SetupGet(o => o.MobileNumber).Returns(_mobileNumber);
            accountEntityMock.SetupGet(o => o.HashedPassword).Returns(_hashedPassword);
            
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            await serviceUnderTest.CreateAccountAsync(accountEntityMock.Object);

            //Assert
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>()
                   .Verify(o => o.AddAsync(It.Is<AccountDocument>(ad => ad.Id == _id 
                                                                                                                                            && ad.Email == _email 
                                                                                                                                            && ad.MobileNumber == _mobileNumber
                                                                                                                                            && ad.HashedPassword == _hashedPassword)));
        }
        
        [Test]
        public async Task CreateAccountAsync_Null()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            await serviceUnderTest.CreateAccountAsync(null);

            //Assert
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>()
                .Verify(o => o.AddAsync(It.Is<AccountDocument>(ad => ad.Id == _id 
                                                                     && ad.Email == _email 
                                                                     && ad.MobileNumber == _mobileNumber
                                                                     && ad.HashedPassword == _hashedPassword)), Times.Never);
        }

        [Test]
        public async Task GetAccountAsyncGuid_Exists()
        {
            //Arrange
            var accountEntityMock = new Mock<IAccountEntity>();
            accountEntityMock.SetupGet(o => o.Id).Returns(_id);
            accountEntityMock.SetupGet(o => o.Email).Returns(_email);
            accountEntityMock.SetupGet(o => o.MobileNumber).Returns(_mobileNumber);
            accountEntityMock.SetupGet(o => o.HashedPassword).Returns(_hashedPassword);
            
            var serviceUnderTest = CreateServiceUnderTest();
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsIn(_id))).Returns(Task.FromResult(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword}));
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsNotIn(_id))).Returns(Task.FromResult(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword}));
            
            //Act
            var accountEntity = await serviceUnderTest.GetAccountAsync(_id);
          
            //Assert
            accountEntity.ShouldNotBeNull();
            accountEntity.Id.ShouldBe(_id);
            accountEntity.Email.ShouldBe(_email);
            accountEntity.MobileNumber.ShouldBe(_mobileNumber);
            accountEntity.HashedPassword.ShouldBe(_hashedPassword);
        }
        
        [Test]
        public async Task GetAccountAsyncGuid_DoesNotExist()
        {
            //Arrange
            var accountEntityMock = new Mock<IAccountEntity>();
            accountEntityMock.SetupGet(o => o.Id).Returns(_id);
            accountEntityMock.SetupGet(o => o.Email).Returns(_email);
            accountEntityMock.SetupGet(o => o.MobileNumber).Returns(_mobileNumber);
            accountEntityMock.SetupGet(o => o.HashedPassword).Returns(_hashedPassword);
            
            var serviceUnderTest = CreateServiceUnderTest();
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsIn(_id))).Returns(Task.FromResult(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword}));
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsNotIn(_id))).Returns(Task.FromResult<AccountDocument>(null));
            
            //Act
            var accountEntity = await serviceUnderTest.GetAccountAsync(Guid.NewGuid());
            
            //Assert
            accountEntity.ShouldBeNull();
        }
        
        [Test]
        public async Task GetAccountAsyncEmail_Exists()
        {
            //Arrange
            var accountEntityMock = new Mock<IAccountEntity>();
            accountEntityMock.SetupGet(o => o.Id).Returns(_id);
            accountEntityMock.SetupGet(o => o.Email).Returns(_email);
            accountEntityMock.SetupGet(o => o.MobileNumber).Returns(_mobileNumber);
            accountEntityMock.SetupGet(o => o.HashedPassword).Returns(_hashedPassword);

            var serviceUnderTest = CreateServiceUnderTest();

            var accounts = new List<AccountDocument>();
            accounts.Add(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword});
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsAny<Expression<Func<AccountDocument, bool>>>())).Returns(async (Expression<Func<AccountDocument, bool>> predicate) => await Task.FromResult(accounts.FirstOrDefault(predicate.Compile())));

            //Act
            var accountEntity = await serviceUnderTest.GetAccountAsync(_email);
          
            //Assert
            accountEntity.ShouldNotBeNull();
            accountEntity.Id.ShouldBe(_id);
            accountEntity.Email.ShouldBe(_email);
            accountEntity.MobileNumber.ShouldBe(_mobileNumber);
            accountEntity.HashedPassword.ShouldBe(_hashedPassword);
        }
        
        [Test]
        public async Task GetAccountAsyncEmail_DoesNotExist()
        {
            //Arrange
            var accountEntityMock = new Mock<IAccountEntity>();
            accountEntityMock.SetupGet(o => o.Id).Returns(_id);
            accountEntityMock.SetupGet(o => o.Email).Returns(_email);
            accountEntityMock.SetupGet(o => o.MobileNumber).Returns(_mobileNumber);
            accountEntityMock.SetupGet(o => o.HashedPassword).Returns(_hashedPassword);
            
            var serviceUnderTest = CreateServiceUnderTest();

            var accounts = new List<AccountDocument>();
            accounts.Add(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword});
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsAny<Expression<Func<AccountDocument, bool>>>())).Returns(async (Expression<Func<AccountDocument, bool>> predicate) => await Task.FromResult(accounts.FirstOrDefault(predicate.Compile())));

            //Act
            var accountEntity = await serviceUnderTest.GetAccountAsync("unknow@email.com");
            
            //Assert
            accountEntity.ShouldBeNull();
        }
        
        [Test]
        public async Task GetAccountAsyncEmail_Null()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            var accountEntity = await serviceUnderTest.GetAccountAsync(null);
            
            //Assert
            accountEntity.ShouldBeNull();
        }

        [Test]
        public async Task UpdateAccountAsync_Valid()
        {           
            //Arrange
            string newEmail = "test2@test.com";
            string newMobileNumber = "07703123456";
            string newHashedPassword = "hashed2";
            
            var accountEntityMock = new Mock<IAccountEntity>();
            accountEntityMock.SetupGet(o => o.Id).Returns(_id);
            accountEntityMock.SetupGet(o => o.Email).Returns(newEmail);
            accountEntityMock.SetupGet(o => o.MobileNumber).Returns(newMobileNumber);
            accountEntityMock.SetupGet(o => o.HashedPassword).Returns(newHashedPassword);
            
            var serviceUnderTest = CreateServiceUnderTest();
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsIn(_id))).Returns(Task.FromResult(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword}));
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsNotIn(_id))).Returns(Task.FromResult(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword}));
            
            //Act
            await serviceUnderTest.UpdateAccountAsync(accountEntityMock.Object);

            //Assert
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>()
                .Verify(o =>  o.UpdateAsync(It.Is<AccountDocument>(ad => ad.Id == _id 
                                                                         && ad.Email == newEmail 
                                                                         && ad.MobileNumber == newMobileNumber 
                                                                         && ad.HashedPassword == newHashedPassword))); 
        }
        
        [Test]
        public async Task UpdateAccountAsync_DoesNotExist()
        {
            //Arrange
            string newEmail = "test2@test.com";
            string newMobileNumber = "07703123456";
            string newHashedPassword = "hashed2";
            
            var accountEntityMock = new Mock<IAccountEntity>();
            accountEntityMock.SetupGet(o => o.Id).Returns(Guid.NewGuid());
            accountEntityMock.SetupGet(o => o.Email).Returns(newEmail);
            accountEntityMock.SetupGet(o => o.MobileNumber).Returns(newMobileNumber);
            accountEntityMock.SetupGet(o => o.HashedPassword).Returns(newHashedPassword);
            
            var serviceUnderTest = CreateServiceUnderTest();
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsIn(_id))).Returns(Task.FromResult(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword}));
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsNotIn(_id))).Returns(Task.FromResult<AccountDocument>(null));

            //Act
            await serviceUnderTest.UpdateAccountAsync(accountEntityMock.Object);

            //Assert
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>()
                .Verify(o =>  o.UpdateAsync(It.Is<AccountDocument>(ad => ad.Id == _id 
                                                                                                                                          && ad.Email == newEmail 
                                                                                                                                          && ad.MobileNumber == newMobileNumber 
                                                                                                                                          && ad.HashedPassword == newHashedPassword)), Times.Never()); 
        }
        
        [Test]
        public async Task UpdateAccountAsync_Null()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsIn(_id))).Returns(Task.FromResult(new AccountDocument() { Id = _id, Email = _email, MobileNumber = _mobileNumber, HashedPassword = _hashedPassword}));
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.GetAsync(It.IsNotIn(_id))).Returns(Task.FromResult<AccountDocument>(null));

            //Act
            await serviceUnderTest.UpdateAccountAsync(null);
            
            //Assert
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>()
                .Verify(o =>  o.UpdateAsync(It.IsAny<AccountDocument>()), Times.Never()); 
        }

        [Test]
        public async Task DoesAccountExistAsyncGuid_Yes()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            List<AccountDocument> accounts = new List<AccountDocument>();
            accounts.Add(new AccountDocument() { Id = _id, Email = _email, HashedPassword = _hashedPassword, MobileNumber = _mobileNumber});
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.ExistsAsync(It.IsAny<Expression<Func<AccountDocument, bool>>>())).Returns(async (Expression<Func<AccountDocument, bool>> predicate) => await Task.FromResult(accounts.Exists(new Predicate<AccountDocument>(predicate.Compile()))));

            //Act
            var exists = await serviceUnderTest.DoesAccountExistAsync(_id);

            //Act and Assert
            exists.ShouldBe(true);
        }
        
        [Test]
        public async Task DoesAccountExistAsyncGuid_No()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            List<AccountDocument> accounts = new List<AccountDocument>();
            accounts.Add(new AccountDocument() { Id = _id, Email = _email, HashedPassword = _hashedPassword, MobileNumber = _mobileNumber});
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.ExistsAsync(It.IsAny<Expression<Func<AccountDocument, bool>>>()))
                                                                                                                                 .Returns(async (Expression<Func<AccountDocument, bool>> predicate) => 
                                                                                                                                                     await Task.FromResult(accounts.Exists(new Predicate<AccountDocument>(predicate.Compile()))));

            //Act
            var exists = await serviceUnderTest.DoesAccountExistAsync(Guid.NewGuid());

            //Act and Assert
            exists.ShouldBe(false);
        }

        [Test]
        public async Task DoesAccountExistAsyncEmail_Yes()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            List<AccountDocument> accounts = new List<AccountDocument>();
            accounts.Add(new AccountDocument() { Id = _id, Email = _email, HashedPassword = _hashedPassword, MobileNumber = _mobileNumber});
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.ExistsAsync(It.IsAny<Expression<Func<AccountDocument, bool>>>())).Returns(async (Expression<Func<AccountDocument, bool>> predicate) => await Task.FromResult(accounts.Exists(new Predicate<AccountDocument>(predicate.Compile()))));

            //Act
            var exists = await serviceUnderTest.DoesAccountExistAsync(_email);

            //Act and Assert
            exists.ShouldBe(true);
        }
        
        [Test]
        public async Task DoesAccountExistAsyncEmail_No()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            List<AccountDocument> accounts = new List<AccountDocument>();
            accounts.Add(new AccountDocument() { Id = _id, Email = _email, HashedPassword = _hashedPassword, MobileNumber = _mobileNumber});
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.ExistsAsync(It.IsAny<Expression<Func<AccountDocument, bool>>>())).Returns(async (Expression<Func<AccountDocument, bool>> predicate) => await Task.FromResult(accounts.Exists(new Predicate<AccountDocument>(predicate.Compile()))));

            //Act
            var exists = await serviceUnderTest.DoesAccountExistAsync("unkown@email.com");

            //Act and Assert
            exists.ShouldBe(false);
        }
        
        [Test] public async Task DoesAccountExistAsyncEmail_Null()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            List<AccountDocument> accounts = new List<AccountDocument>();
            accounts.Add(new AccountDocument() { Id = _id, Email = _email, HashedPassword = _hashedPassword, MobileNumber = _mobileNumber});
            
            _mocker.GetMock<IMongoRepository<AccountDocument, Guid>>().Setup(adr => adr.ExistsAsync(It.IsAny<Expression<Func<AccountDocument, bool>>>())).Returns(async (Expression<Func<AccountDocument, bool>> predicate) => await Task.FromResult(accounts.Exists(new Predicate<AccountDocument>(predicate.Compile()))));

            //Act
            var exists = await serviceUnderTest.DoesAccountExistAsync(null);

            //Act and Assert
            exists.ShouldBe(false);
        }
        
        private IAccountRepository CreateServiceUnderTest() => _mocker.CreateInstance<AccountRepository>();
    }
}