using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xunit;
using WebApiPerson.Controllers;

namespace WebApiPerson.Tests.Controllers
{
    public class LoginControllerTests
    {
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            // Mock IConfiguration para la sección "Jwt"
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "ThisIsASecretKeyForJwtToken12345"},  // clave simulada
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new LoginController(configuration);
        }

        [Theory]
        [InlineData("admin", "1234", true)]
        [InlineData("user", "1234", true)]
        [InlineData("admin", "wrongpass", false)]
        [InlineData("unknown", "1234", false)]
        public void Login_TestCredentials_ReturnsExpected(string username, string password, bool isSuccess)
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var result = _controller.Login(request);

            if (isSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.NotNull(okResult.Value);

                // La respuesta es un objeto con Token string
                var tokenProperty = okResult.Value.GetType().GetProperty("Token");
                Assert.NotNull(tokenProperty);

                var tokenValue = tokenProperty.GetValue(okResult.Value) as string;
                Assert.False(string.IsNullOrEmpty(tokenValue));
            }
            else
            {
                Assert.IsType<UnauthorizedResult>(result);
            }
        }
    }
}
