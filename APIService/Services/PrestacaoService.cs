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
using System.Linq;

namespace APIService.Services
{
    public class PrestacaoService : IPrestacaoService
    {
        private DataContext _context;
        public PrestacaoService(DataContext context)
        {
            _context = context;
        }

        #region Contrato
        public async Task<Contrato> GerarNovoContrato(Contrato contrato)
        {
            contrato.SetDataContratacao(DateTime.Now);
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            List<Prestacao> prestacoes = await GerarPrestacoes(contrato);
            foreach (Prestacao prestacao in prestacoes)
            {
                _context.Prestacoes.Add(prestacao);
            }

            contrato.SetPrestacoes(prestacoes);
            _context.Contratos.Update(contrato);
            await _context.SaveChangesAsync();
            return contrato;
        }

        public async Task<List<Contrato>> ListContratos()
        {
            return await _context.Contratos
                .Include(x => x.Prestacoes)
                .ToListAsync();
        }

        public async Task<Contrato> GetContractById(int id)
        {            
            var contrato = await _context.Contratos
                .Include(x => x.Prestacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return contrato;
        }

        public async Task<bool> RemoverContrato(int id)
        {
            var contrato = await _context.Contratos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (contrato == null)
                return false;

            var prestacoes = await _context.Prestacoes
                .AsNoTracking()
                .Where(x => x.IdContrato == contrato.Id)
                .ToListAsync();

            _context.RemoveRange(prestacoes);
            
            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Prestação
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

        public async Task<Prestacao> CriarPrestacao(int contractId, DateTime dataVencimento, float valorPrestacao)
        {
            Prestacao prestacao = new Prestacao();
            prestacao.IdContrato = contractId;
            prestacao.DataVencimento = dataVencimento;
            prestacao.DataPagamento = null;
            prestacao.Valor = valorPrestacao;
            prestacao.Status = "Aberta";
            await Task.Delay(0);
            return prestacao;
        }

        public async Task<string> CheckStatusPrestacao(string dataPagamento, DateTime dataVencimento)
        {
            await Task.Delay(0);

            if (!string.IsNullOrEmpty(dataPagamento))
            {
                return "Baixada";
            }
            else if (dataVencimento.CompareTo(DateTime.Now) < 0)
            {
                return "Atrasada";
            }
            else return "Aberta";
        }

        #endregion

        #region Funções
        public async Task<TimeSpan> TempoAteAmanha()
        {
            await Task.Delay(0);

            DateTime amanha = DateTime.Today.AddDays(1);

            TimeSpan tempoAteAmanha = amanha - DateTime.Now;
            
            return tempoAteAmanha;
        }

        #endregion
    }

    public interface IPrestacaoService
    {
        Task<Contrato> GerarNovoContrato(Contrato model);
        Task<List<Contrato>> ListContratos();
        Task<Contrato> GetContractById(int id);
        Task<bool> RemoverContrato(int id);
        Task<List<Prestacao>> GerarPrestacoes(Contrato model);
        Task<string> CheckStatusPrestacao(string dataPagamento, DateTime dataVencimento);
        Task<TimeSpan> TempoAteAmanha();
    }
}