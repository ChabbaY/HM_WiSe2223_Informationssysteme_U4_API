using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for addresses.
    /// </summary>
    [Route("api/adresses")]
    [ApiController]
    public class AddressController : ControllerBase {
        private Context context;
        public AddressController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all addresses.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Address[]> GetAllAddresses() {
            return Ok(context.Addresses.ToArray());
        }

        /// <summary>
        /// Returns the address with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Address> GetAddress(int id) {
            var value = context.Addresses.Where(v => v.Id == id).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds an address.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Address>> AddAddress([FromBody] Address value) {
            if (ModelState.IsValid) {
                //test if address already exists
                if (context.Addresses.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                    return Conflict(); //address with id already exists, we return a conflict

                context.Addresses.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the address
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Address
        }
    }
}