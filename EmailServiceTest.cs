using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using VSApi.Data;
using VSApi.Services;
using Xunit;

namespace VSApi.Tests
{
    public class EmailServiceTest
    {
        private readonly EmailService _emailEmailService;

        public EmailServiceTest()
        {
            _emailEmailService = new EmailService();
        }

        [Fact]
        public async Task GivenCryptoData_ThenItShouldSendNotificationEmail()
        {
            #region Arrange
            const string buyStr = "Test email";
            const string price = "240,00";
            const string oldPrice = "200,00";
            const string change = "20,00";
            #endregion

            #region Act
            var message = _emailEmailService.PrepareMessage(buyStr, price, oldPrice, change);
            await _emailEmailService.SendMessage(message);
            #endregion
            
            #region Assert
            Assert.Equal("Price alert notification", message.Subject);
            Assert.Equal("\"CryptoWebApp\" <karol.testm@gmail.com>", message.From.ToString());
            #endregion
        }
    }
}
