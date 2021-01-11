using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIService.Data;
using APIService.Models;
using System;

namespace APIService.Services
{
    public class PrestacaoService : IPrestacaoService
    {
        public async Task<List<Prestacao>> GerarPrestacoes(Contrato model)
        {
            List<Prestacao> prestacoes = new List<Prestacao>();
            int qtdParcelas = model.QuantidadeParcelas;
            float valorPrestacao = model.ValorFinanciado / (float) qtdParcelas;

            for(int i = 0; i < qtdParcelas; i++)
            {
                DateTime dataContratacao = model.DataContratacao;
                DateTime dataVencimento = dataContratacao.AddMonths(i+1);
                prestacoes.Add(await CriarPrestacao(model.Id, dataVencimento, valorPrestacao));
            }
            
            return prestacoes;
        }

        public async Task<Prestacao> CriarPrestacao(int modelId, DateTime dataVencimento, float valorPrestacao)
        {
            Prestacao prestacao = new Prestacao();
            prestacao.IdContrato = modelId;
            prestacao.DataVencimento = dataVencimento;
            prestacao.Valor = valorPrestacao;
            prestacao.Status = "Aberta";
            await Task.Delay(0);
            return prestacao;
        }
    }

    public interface IPrestacaoService
    {
        Task<List<Prestacao>> GerarPrestacoes(Contrato model);
    }
}