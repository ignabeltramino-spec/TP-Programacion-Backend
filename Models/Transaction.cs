using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp_ProgramacionIII.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string CryptoCode { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; 
        public int ClientId { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal CryptoAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Money { get; set; } 
        public DateTime DateTime { get; set; }

    }
}
