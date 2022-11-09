using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for positions.
    /// </summary>
    [Route("api/customers/{cid}/requests/{rid}/positions")]
    [ApiController]
    public class PositionController : ControllerBase {
        private Context context;
        public PositionController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all positions.
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Position[]> GetAllPositions([FromRoute] int cid, [FromRoute] int rid) {
            var requests = from v in context.Requests
                           where v.CustomerId == cid
                           select v.Id;
            if (!requests.Contains(rid)) {
                ModelState.AddModelError("validationError", "Request to Customer not found");
                return NotFound(ModelState);
            }

            return Ok(context.Positions.Where(v => v.RequestId == rid).ToArray());
        }

        /// <summary>
        /// Returns the position with a given id.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet("{pid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Position> GetPosition(int pid) {
            var value = context.Positions.Where(v => v.Id == pid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a position.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Position>> AddPosition([FromBody] Position value) {
            if (ModelState.IsValid) {
                //test if position already exists
                if (context.Positions.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                    return Conflict(); //position with id already exists, we return a conflict

                context.Positions.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the position
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Position
        }
    }
}