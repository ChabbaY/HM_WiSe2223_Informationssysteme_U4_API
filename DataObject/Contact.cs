using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Contact {
        [Key]
        public int Id { get; set; }
    }
}