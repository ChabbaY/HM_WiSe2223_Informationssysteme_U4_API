using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace API.DataObject {
    public class Material {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        public SqlMoney Price { get; set; }
    }
}