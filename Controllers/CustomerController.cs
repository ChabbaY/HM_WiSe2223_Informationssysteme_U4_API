using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for customers.
    /// </summary>
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase {
        private Context context;
        public CustomerController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all customers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Customer[]> GetAllCustomers() {
            return Ok(context.Customers.ToArray());
        }

        /// <summary>
        /// Returns the customer with a given id.
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet("{cid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Customer> GetCustomer([FromRoute] int cid) {
            var value = context.Customers.Where(v => v.Id == cid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a customer.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Customer>> AddCustomer([FromBody] Customer value) {
            if (ModelState.IsValid) {
                //test if customer already exists
                if (context.Customers.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                    return Conflict(); //customer with id already exists, we return a conflict

                context.Customers.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the customer
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Customer
        }
    }
}