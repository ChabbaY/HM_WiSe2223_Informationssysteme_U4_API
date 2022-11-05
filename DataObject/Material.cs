using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace API.DataObject {
    public class Material : Object {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        public SqlMoney Price { get; set; }
    }
}