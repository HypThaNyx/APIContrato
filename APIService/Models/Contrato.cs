using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIService.Models
{
    public class Contrato
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public DateTime DataContratacao { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public int QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public float ValorFinanciado { get; set; }

        public List<Prestacao> Prestacoes { get; set; }
    }
}