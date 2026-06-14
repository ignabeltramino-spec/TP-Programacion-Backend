using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tp_ProgramacionIII.DTOs
{
    public class TransactionDTO
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string CryptoCode { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Action { get; set; }

        public int ClientId { get; set; }
        [Range(0.00000001, double.MaxValue, ErrorMessage = "El valor no puede ser menor a 0" )]
        public decimal CryptoAmount { get; set; }
        public decimal Money { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateTime DateTime { get; set; }
    }
}
