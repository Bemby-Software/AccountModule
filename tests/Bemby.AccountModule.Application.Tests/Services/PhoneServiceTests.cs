using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Application.Services;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;

namespace Bemby.AccountModule.Application.Tests.Services
{
    [TestFixture]
    public class PhoneServiceTests
    {
        private AutoMocker _mocker; 
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [TestCase("+447703 622 646", true)]
        [TestCase("+441482 797 262", true)]
        [TestCase("+447703622646", true)]
        [TestCase("+441482797262", true)]
        [TestCase("01482797262", false)]
        public void IsValidPhoneNumber(string phoneNumber, bool expected)
        {
            //Arrange
            var serviceUnderTest = CreateServiceUnderTest();
            
            //Act
            var isValid = serviceUnderTest.IsValidPhoneNumber(phoneNumber);
            
            //Arrange
            isValid.ShouldBe(expected);
        }
        
        private IPhoneService CreateServiceUnderTest() => _mocker.CreateInstance<PhoneService>();
    }
}