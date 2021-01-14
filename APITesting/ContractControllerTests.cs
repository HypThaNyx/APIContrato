using APIService.Data;
using APIService.Models;
using APIService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace APITesting
{
    public class ContractControllerTests
    {
        [Fact]
        public void Test1()
        {

        }

        private DbContextOptions<DataContext> CreateInMemoryDB()
        {
            return new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ContractDatabase")
                .Options;
        }
    }
}
