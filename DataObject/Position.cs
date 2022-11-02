using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace API.DataObject {
    public class Position {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Pos { get; set; }

        [Required]
        public SqlMoney Count { get; set; }
    }
}