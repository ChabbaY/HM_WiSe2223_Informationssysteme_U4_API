using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Request : Object {
        [MaxLength(100)]
        public string FromDate { get; set; }

        [MaxLength(100)]
        public string UntilDate { get; set; }
    }
}