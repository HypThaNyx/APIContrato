using System;
using System.ComponentModel.DataAnnotations;

namespace APIService.Models
{
    public class Prestacao
    {
        [Key]
        public int Id { get; set; }

        public int IdContrato { get ;  set; }

        public Contrato Contrato { get ;  set; }

        public DateTime DataVencimento { get; set; }

        public string DataPagamento { get; set; }

        public float Valor { get; set; }

        public string Status { get; set; }
    }
}