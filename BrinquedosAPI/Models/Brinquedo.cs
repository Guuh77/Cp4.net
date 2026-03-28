using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrinquedosAPI.Models
{
    [Table("TDS_TB_Brinquedos")]
    public class Brinquedo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_brinquedo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome_brinquedo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tipo_brinquedo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Classificacao { get; set; }

        [Required]
        [MaxLength(20)]
        public string Tamanho { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }
    }
}
