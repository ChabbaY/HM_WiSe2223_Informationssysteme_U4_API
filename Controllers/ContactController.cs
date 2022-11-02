using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for contacts.
    /// </summary>
    [Route("api/contacts")]
    [ApiController]
    public class ContactController : ControllerBase {
        private Context context;
        public ContactController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all contacts.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Contact[]> GetAllContacts() {
            return Ok(context.Contacts.ToArray());
        }

        /// <summary>
        /// Returns the contact with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Contact> GetContact(int id) {
            var value = context.Contacts.Where(v => v.Id == id).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a contact.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Contact>> AddCustomer([FromBody] Contact value) {
            if (ModelState.IsValid) {
                //test if contact already exists
                if (context.Contacts.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                    return Conflict(); //contact with id already exists, we return a conflict

                context.Contacts.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the contact
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Contact
        }
    }
}