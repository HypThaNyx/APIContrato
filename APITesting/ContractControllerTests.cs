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
        public async Task GerarNovoContrato_ContratoValido_DadosValidos()
        {
            var options = await CreateInMemoryDB();

            //Create mocked Context by seeding Data
            using (var context = new DataContext(options))
            {
                context.Contratos.Add(new Contrato
                {
                    QuantidadeParcelas = 5,
                    ValorFinanciado = 5000
                });
                context.SaveChanges();
                
                PrestacaoService prestacaoService = new PrestacaoService(context);
                var contratos = await prestacaoService.ListContratos();

                //ASSERT
                Assert.NotEmpty(contratos);
                foreach (var contrato in contratos)
                {
                    Assert.Equal(5, contrato.QuantidadeParcelas);
                    Assert.Equal(5000, contrato.ValorFinanciado);
                }
            }
        }

        private async Task<DbContextOptions<DataContext>> CreateInMemoryDB()
        {
            await Task.Delay(0);
            return new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ContractDatabase")
                .Options;
        }
    }
}
