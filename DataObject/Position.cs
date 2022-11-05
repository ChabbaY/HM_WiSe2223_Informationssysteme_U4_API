using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace API.DataObject {
    public class Position : Object {
        [Required]
        public int Pos { get; set; }

        [Required]
        public SqlMoney Count { get; set; }
    }
}