using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using VSApi.Controllers;
using VSApi.Data;
using VSApi.Models;
using VSApi.Services;
using Xunit;

namespace VSApi.Tests
{
    public class CryptoControllerTest
    {
        // Test Get() method
        [Fact]
        public async void Get()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "WebApp")
                .Options;

            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());

            await using (var context = new ApiContext(options, operationalStoreOptions))
            {
                context.Add(new Crypto()
                {
                    Rank = 2,
                    Name = "Test",
                    Symbol = "XYZ",
                    Price = "1234,99",
                    Change24h = null,
                    Change7d = null,
                    OwnFlag = 0
                });
                await context.SaveChangesAsync();
            }

            IActionResult cryptoExist;
            OkObjectResult crypto;

            #endregion

            #region Act

            await using (var context = new ApiContext(options, null))
            {
                var controller = new CryptoController(new CryptoRepository(context), new CoinMarketCapApiService());
                cryptoExist = controller.Get(2);
                crypto = controller.Get(2) as OkObjectResult; 
            }
            #endregion

            #region Assert
            Assert.NotNull(cryptoExist);
            Assert.Equal("200", crypto.StatusCode.Value.ToString());
            #endregion
        }


        // Test  GetCmcApi() method
        // [Fact]
        // public async void GetCmcApi()
        // {
        //
        // }

        // Test  Post() method
        // [Fact]
        // public async void Post()
        // {
        //
        // }

        // Test  Edit(int? id, int flag) method
        // [Fact]
        // public async void Edit()
        // {
        //
        // }


    }
}
