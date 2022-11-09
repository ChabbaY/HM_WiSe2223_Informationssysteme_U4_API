using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Relation : Object {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ContactId { get; set; }
    }
}