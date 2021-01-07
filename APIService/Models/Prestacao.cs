using System;
using System.ComponentModel.DataAnnotations;

namespace APIContrato.Models
{
    public class Prestacao
    {
        [Key]
        public int IdContrato { get ;  set; }

        public DateTime DataVencimento { get; set; }

        public DateTime DataPagamento { get; set; }

        public float Valor { get; set; }

        public string Status { get; set; }
    }
}