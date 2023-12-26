using Microsoft.AspNetCore.Mvc;


namespace BankingTests
{
    [TestClass]
    public class RegistrationControllerTests
    {
        private Mock<IRegistrationService> _mockRegistrationService;
        private RegistrationController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _mockRegistrationService = new Mock<IRegistrationService>();
            _controller = new RegistrationController(_mockRegistrationService.Object);
        }

        [TestMethod]
        // ���������, ��� ��� ������ GET-������ Registrate ������������ ������ ���� ViewResult
        public void Registrate_GET_ReturnsViewResult()
        {
            // Arrange

            // Act
            var result = _controller.Registrate() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        // ���������, ��� ��� �������� ����������� ������� � POST-����� Registrate ���������� ��������������� �� �������� Login
        // � ����������� Login.
        public void Registrate_POST_WithMatchingPasswords_RedirectsToLoginAction()
        {
            // Arrange
            var nickname = "testuser";
            var email = "test@example.com";
            var password = "password";
            var confirmPassword = "password";
            var phoneNumber = "1234567890";
            var dateBirth = new DateTime(2000, 1, 1);

            // Act
            var result = _controller.Registrate(nickname, email, password, confirmPassword, phoneNumber, dateBirth) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ActionName);
            Assert.AreEqual("Login", result.ControllerName);
        }

        [TestMethod]
        // ���������, ��� ��� �������� ������������� ������� � POST-����� Registrate ������������ ������ ���� BadRequestObjectResult � ��������������� ����������
        public void Registrate_POST_WithMismatchingPasswords_ReturnsBadRequest()
        {
            // Arrange
            var nickname = "testuser";
            var email = "test@example.com";
            var password = "password";
            var confirmPassword = "differentpassword";
            var phoneNumber = "1234567890";
            var dateBirth = new DateTime(2000, 1, 1);

            // Act
            var result = _controller.Registrate(nickname, email, password, confirmPassword, phoneNumber, dateBirth) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Password unconfirm", result.Value);
        }
    }
}