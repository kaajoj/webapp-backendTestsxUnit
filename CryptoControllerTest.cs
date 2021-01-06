using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using VSApi.Controllers;
using VSApi.Data;
using VSApi.Interfaces;
using VSApi.Models;
using VSApi.Services;
using Xunit;

namespace VSApi.Tests
{
    public class CryptoControllerTest : IClassFixture<ContextFixture>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICoinMarketCapApiService _coinMarketCapApiService;
        private readonly ICryptoService _cryptoService;

        public CryptoControllerTest(ContextFixture contextFixture)
        {
            _cryptoRepository = new CryptoRepository(contextFixture.ApiContext);
            _cryptoService = new CryptoService(_cryptoRepository);
            _coinMarketCapApiService = new CoinMarketCapApiService(_cryptoRepository);
        }

        [Fact]
        public void ItShouldReturnCryptos()
        {
            #region Arrange
            var cryptoController = new CryptoController(_cryptoService, _coinMarketCapApiService);
            #endregion

            #region Act
            var response = cryptoController.Get() as OkObjectResult;
            var cryptos = response.Value as IEnumerable<Crypto>;
            #endregion

            #region Assert
            Assert.Equal(3, cryptos.Count());
            #endregion
        }

        [Fact]
        public void GivenCryptoFoundThenItShouldReturnCrypto()
        {
            #region Arrange
            var cryptoController = new CryptoController(_cryptoService, _coinMarketCapApiService);
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
            var cryptoController = new CryptoController(_cryptoService, _coinMarketCapApiService);
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
            var cryptoController = new CryptoController(_cryptoService, _coinMarketCapApiService);
            #endregion

            #region Act
            var response = await cryptoController.GetCmcApi() as OkObjectResult;
            var cmcCryptos = response.Value as List<Crypto>;
            #endregion

            #region Assert
            Assert.Equal(15, cmcCryptos.Count());
            #endregion
        }

        [Fact]
        public async void GivenCryptoAddedThenItShouldReturnCryptosInfo()
        {
            #region Arrange
            var cryptoController = new CryptoController(_cryptoService, _coinMarketCapApiService);
            #endregion

            #region Act
            var crypto = new Crypto()
            {
                Rank = 10,
                Name = "TestPost",
                Symbol = "POST",
                Price = "100,00",
                Change24h = "0",
                Change7d = "0",
                OwnFlag = 0
            };

            var postResponse = await cryptoController.Post(crypto) as CreatedAtRouteResult;
            var cryptoAdded = postResponse.Value as Crypto;

            var response = cryptoController.Get() as OkObjectResult;
            var cryptos = response.Value as IEnumerable<Crypto>;
            #endregion

            #region Assert
            Assert.Equal(10, cryptoAdded.Rank);
            Assert.Equal("TestPost", cryptoAdded.Name);
            Assert.Equal(4, cryptos.Count());
            #endregion
        }

        [Fact]
        public async void GivenCryptoFoundByIdThenItShouldReturnUpdatedCryptoWithSelectedOwnFlag()
        {
            #region Arrange
            var cryptoController = new CryptoController(_cryptoService, _coinMarketCapApiService);
            #endregion

            #region Act
            var cryptoWithOwnFlag0 = await cryptoController.Edit(2, 1) as OkObjectResult;
            var cryptoWithOwnFlag0SetTo1 = cryptoWithOwnFlag0.Value as Crypto;

            var cryptoWithOwnFlag1 = await cryptoController.Edit(4, 0) as OkObjectResult;
            var cryptoWithOwnFlag1SetTo0 = cryptoWithOwnFlag1.Value as Crypto;
            #endregion

            #region Assert
            Assert.True(true);
            Assert.Equal(1, cryptoWithOwnFlag0SetTo1.OwnFlag);
            Assert.Equal(0, cryptoWithOwnFlag1SetTo0.OwnFlag);
            #endregion
        }
    }
}
