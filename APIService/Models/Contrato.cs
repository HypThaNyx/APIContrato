using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIService.Models
{
    public class Contrato
    {
        [Key]
        public int Id { get; private set; }

        public DateTime DataContratacao { get; private set; }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public int QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public float ValorFinanciado { get; set; }

        public List<Prestacao> Prestacoes { get; private set; }

        public void SetDataContratacao(DateTime data) { DataContratacao = data; }

        public void SetPrestacoes(List<Prestacao> prestacoes) { Prestacoes = prestacoes; }

    }
}