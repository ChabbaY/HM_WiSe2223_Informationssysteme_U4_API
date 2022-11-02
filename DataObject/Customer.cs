using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.DataObject {
    public class Customer  {

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// This property is not required
        /// </summary>
        public string PhoneNumber { get; set; }

    }
}
