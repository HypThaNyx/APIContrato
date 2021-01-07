using System;
using System.ComponentModel.DataAnnotations;

namespace APIContrato.Models
{
    public class Contrato
    {
        [Key]
        public int Id { get {return Id;} private set{ } }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public DateTime DataContratacao { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public int QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public float ValorFinanciado { get; set; }

        public Prestacao[] Prestacoes { get; set; }
    }
}