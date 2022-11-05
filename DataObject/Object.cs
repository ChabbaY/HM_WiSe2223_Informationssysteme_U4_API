using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Object {
        [Key]
        public int Id { get; set; }
    }
}