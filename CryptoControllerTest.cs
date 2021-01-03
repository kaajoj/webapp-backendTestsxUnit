using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using VSApi.Controllers;
using VSApi.Data;
using VSApi.Models;
using VSApi.Services;
using Xunit;

namespace VSApi.Tests
{
    public class CryptoControllerTest : IClassFixture<ContextFixture>
    {
        private readonly ContextFixture _contextFixture;

        public CryptoControllerTest(ContextFixture contextFixture)
        {
            _contextFixture = contextFixture;
        }

        [Fact]
        public void GivenCryptoFoundThenItShouldReturnCrypto()
        {
            #region Arrange
            var controller = new CryptoController(new CryptoRepository(_contextFixture.ApiContext), new CoinMarketCapApiService());
            #endregion

            #region Act
            var cryptoExists = controller.Get(1) as OkObjectResult;
            var cryptoExistsValue = cryptoExists.Value as Crypto;
            #endregion

            #region Assert
            Assert.NotNull(cryptoExists);
            Assert.Equal(1, cryptoExistsValue.Id);
            Assert.Equal(2, cryptoExistsValue.Rank);
            Assert.Equal("Test", cryptoExistsValue.Name);
            #endregion
        }

        [Fact]
        public void GivenCryptoNotFound()
        {
            #region Arrange
            var controller = new CryptoController(new CryptoRepository(_contextFixture.ApiContext), new CoinMarketCapApiService());
            #endregion

            #region Act
            var cryptoNotExists = controller.Get(-1) as OkObjectResult;
            var cryptoNotExistsValue = cryptoNotExists.Value as Crypto;
            #endregion

            #region Assert
            Assert.Null(cryptoNotExistsValue);
            #endregion
        }

        // [Fact]
        // public async void GetCmcApi()
        // {
        //
        // }

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
