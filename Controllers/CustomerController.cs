using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// This endpoint manages all operations for customers.
    /// </summary>
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Customer> GetCustomer(int id) {
            var customer = context.Customers.Where(c => c.Id == id).FirstOrDefault();
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        /// <summary>
        /// Adds a customer.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Customer>> AddCustomer([FromBody] Customer customer) {
            if (ModelState.IsValid) {

                //test if customer already exists
                if (context.Customers.Where(c => c.Id == customer.Id).FirstOrDefault() != null)
                    return Conflict(); //customer with id already exists, we return a conflict

                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                return Ok(customer); //we return the customer
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Customer
        }
    }
}