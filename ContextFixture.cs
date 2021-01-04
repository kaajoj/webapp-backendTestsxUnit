using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VSApi.Data;
using VSApi.Models;

namespace VSApi.Tests
{
    public class ContextFixture : IDisposable
    {
        public ApiContext ApiContext { get; }
        public ContextFixture()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "WebApp")
                .Options;

            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());

            ApiContext = new ApiContext(options, operationalStoreOptions);

            if (!ApiContext.Cryptos.Any())
            {
                ApiContext.Add(new Crypto()
                {
                    Rank = 2,
                    Name = "Test",
                    Symbol = "XYZ",
                    Price = "1234,99",
                    Change24h = null,
                    Change7d = null,
                    OwnFlag = 0
                }); 
                ApiContext.Add(new Crypto()
                {
                    Rank = 4,
                    Name = "Test2",
                    Symbol = "ABC",
                    Price = "789,00",
                    Change24h = "10",
                    Change7d = "20",
                    OwnFlag = 1
                });
                ApiContext.Add(new Crypto()
                {
                    Rank = 7,
                    Name = "Test3",
                    Symbol = "12C",
                    Price = "51,00",
                    Change24h = "-10",
                    Change7d = "-20",
                    OwnFlag = 0
                });
                ApiContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            ApiContext.Dispose();;
        }
    }
}
