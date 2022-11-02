using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for address information.
    /// </summary>
    [Route("api/adressinformation")]
    [ApiController]
    public class AddressInformationController : ControllerBase {
        private Context context;
        public AddressInformationController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all address information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<AddressInformation[]> GetAllAddressInformation() {
            return Ok(context.AddressInformation.ToArray());
        }

        /// <summary>
        /// Returns the address information with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AddressInformation> GetAddressInformation(int id) {
            var value = context.AddressInformation.Where(v => v.Id == id).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds an address information.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AddressInformation>> AddAddressInformation([FromBody] AddressInformation value) {
            if (ModelState.IsValid) {
                //test if address information already exists
                if (context.AddressInformation.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                    return Conflict(); //address information with id already exists, we return a conflict

                context.AddressInformation.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the address information
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Address Information
        }
    }
}