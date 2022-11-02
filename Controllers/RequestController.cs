using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for requests.
    /// </summary>
    [Route("api/requests")]
    [ApiController]
    public class RequestController : ControllerBase {
        private Context context;
        public RequestController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all requests.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Request[]> GetAllRequests() {
            return Ok(context.Requests.ToArray());
        }

        /// <summary>
        /// Returns the request with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Request> GetRequest(int id) {
            var value = context.Requests.Where(v => v.Id == id).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a request.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Request>> AddCustomer([FromBody] Request value) {
            if (ModelState.IsValid) {
                //test if request already exists
                if (context.Requests.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                    return Conflict(); //request with id already exists, we return a conflict

                context.Requests.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the request
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Request
        }
    }
}