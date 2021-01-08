using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using VSApi.Data;
using VSApi.Services;
using Xunit;

namespace VSApi.Tests
{
    public class CoinMarketCapApiServiceTest : IClassFixture<ContextFixture>
    {
        private readonly CoinMarketCapApiService _coinMarketCapApiService;

        public CoinMarketCapApiServiceTest(ContextFixture contextFixture)
        {
            var cryptoRepository = new CryptoRepository(contextFixture.ApiContext);
            _coinMarketCapApiService = new CoinMarketCapApiService(cryptoRepository);
        }

        [Fact]
        public void CmcGet_ReturnsData()
        {
            #region Act
            var response = _coinMarketCapApiService.CmcGet();
            #endregion

            #region Assert
            Assert.Contains("data", response);
            #endregion
        }

        [Fact]
        public void CmcJsonParse_ReturnsParsedCrypto()
        {
            #region Arrange
            var response = _coinMarketCapApiService.CmcGet();
            #endregion

            #region Act
            var cryptoTemp = _coinMarketCapApiService.CmcJsonParse(response, 0);
            #endregion

            #region Assert
            Assert.Contains("BTC", cryptoTemp.Symbol);
            #endregion
        }
    }
}
