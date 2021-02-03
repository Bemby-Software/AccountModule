using System.Collections.Generic;
using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Application.Services;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;

namespace Bemby.AccountModule.Application.Tests.Services
{
    [TestFixture]
    public class PBKDF2PasswordServiceTests
    {
        private AutoMocker _mocker;
        
        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();
        }
        
        [TestCase("password", "bG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3VtbG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3VtbG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3VtbG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3Vt", "vAWrMigRkELESVrQyTs1opCX5mCRGgnMaeCrGDQg4nI=bG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3VtbG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3VtbG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3VtbG9yZW1pcHN1bWxvcmVtaXBzdW1sb3JlbWlwc3Vt6AMAAA==", 1000)]
        [TestCase("!_Password12", "dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=", "vnRmndMZlafbwA2gKMkNXV8srcmAtiXtCWolwBzn3lc=dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=ECcAAA==", 10000)]
        [TestCase("_!Pass2112word12", "dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=", "T4DKDBEEFZBTpYP0pWtnMvaHBagzEg/yXsm5b4ffskU=dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=6AMAAA==", 1000)]
        [TestCase("P_a_s_s2112wo!!!rd12", "dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=", "84b8aVqhS7SNeRYY0/2EdXJUgva515zR8LOhA5AENbg=dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=6AMAAA==", 1000)]
        public void HashPassword(string password, string salt, string expectedHash, int numberOfIterations)
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            var hashedPassword = serviceUnderTest.HashPassword(password, salt, numberOfIterations);
            
            //Assert
            hashedPassword.ShouldBe(expectedHash);
        }
        
        [TestCase("password", false)]
        [TestCase("password1", false)]
        [TestCase("password12", false)]
        [TestCase("__password12", false)]
        [TestCase("__p_a1_2ssword12", false)]
        [TestCase("!_Password12", true)]
        [TestCase("_!Pass2112word12", true)]
        [TestCase("P_a_s_s2112wo!!!rd12", true)]
        public void IsStrong(string input, bool expected)
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            var isStrong = serviceUnderTest.IsStrongPassword(input);
            
            //Assert
            isStrong.ShouldBe(expected);
        }
        
        [TestCase("wrongpassword", "vnRmndMZlafbwA2gKMkNXV8srcmAtiXtCWolwBzn3lc=dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=ECcAAA==", false)]
        [TestCase("!_Password12", "vnRmndMZlafbwA2gKMkNXV8srcmAtiXtCWolwBzn3lc=dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=ECcAAA==", true)]
        [TestCase("wrongpassword", "84b8aVqhS7SNeRYY0/2EdXJUgva515zR8LOhA5AENbg=dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=6AMAAA==", false)]
        [TestCase("P_a_s_s2112wo!!!rd12", "84b8aVqhS7SNeRYY0/2EdXJUgva515zR8LOhA5AENbg=dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3R0ZXN0dGVzdHRlc3Q=6AMAAA==", true)]
        public void CheckPassword(string attempt, string hashResult, bool isCorrect)
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            var passwordsMatch = serviceUnderTest.CheckPassword(attempt, hashResult);
            
            //Assert
            passwordsMatch.ShouldBe(isCorrect);
        }
        
        [Test]
        public void GenerateSalt()
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            List<string> salts = new List<string>();
            for (int i = 0; i < 1000; i++)
                salts.Add(serviceUnderTest.GenerateSalt());

            //Assert
            for (int i = 0; i < salts.Count - 1; i++)
                for (int j = i + 1; j < salts.Count; j++)
                    salts[i].ShouldNotBe(salts[j]);
        }
        
        private IPasswordService CreateServiceUnderTest() => _mocker.CreateInstance<PBKDF2PasswordService>();
    }
}