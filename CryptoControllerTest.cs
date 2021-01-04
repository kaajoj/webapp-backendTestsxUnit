using System;
using System.Collections.Generic;
using System.Linq;
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
        public void ItShouldReturnCryptos()
        {
            #region Arrange
            var cryptoController = new CryptoController(new CryptoRepository(_contextFixture.ApiContext), new CoinMarketCapApiService());
            #endregion

            #region Act
            var response = cryptoController.Get() as OkObjectResult;
            var cryptos = response.Value as IOrderedEnumerable<Crypto>;
            #endregion

            #region Assert
            Assert.Equal(3, cryptos.Count());
            #endregion
        }

        [Fact]
        public void GivenCryptoFoundThenItShouldReturnCrypto()
        {
            #region Arrange
            var cryptoController = new CryptoController(new CryptoRepository(_contextFixture.ApiContext), new CoinMarketCapApiService());
            #endregion

            #region Act
            var cryptoExists = cryptoController.Get(1) as OkObjectResult;
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
            var cryptoController = new CryptoController(new CryptoRepository(_contextFixture.ApiContext), new CoinMarketCapApiService());
            #endregion

            #region Act
            var cryptoNotExists = cryptoController.Get(-1) as OkObjectResult;
            var cryptoNotExistsValue = cryptoNotExists.Value as Crypto;
            #endregion

            #region Assert
            Assert.Null(cryptoNotExistsValue);
            #endregion
        }

        [Fact]
        public async void ItShouldReturnCryptosFromCmcApi()
        {
            #region Arrange
            var cryptoController = new CryptoController(new CryptoRepository(_contextFixture.ApiContext), new CoinMarketCapApiService());
            #endregion

            #region Act
            var response = await cryptoController.GetCmcApi() as OkObjectResult;
            var cmcCryptos = response.Value as List<Crypto>;
            #endregion

            #region Assert
            Assert.Equal(15, cmcCryptos.Count());
            #endregion
        }

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
