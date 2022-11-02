using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for relations.
    /// </summary>
    [Route("api/relations")]
    [ApiController]
    public class RelationController : ControllerBase {
        private Context context;
        public RelationController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all relations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Relation[]> GetAllRelations() {
            return Ok(context.Relations.ToArray());
        }

        /// <summary>
        /// Returns the relation with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Relation> GetRelation(int id) {
            var value = context.Relations.Where(v => v.Id == id).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a relation.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Relation>> AddCustomer([FromBody] Relation value) {
            if (ModelState.IsValid) {
                //test if relation already exists
                if (context.Relations.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                    return Conflict(); //relation with id already exists, we return a conflict

                context.Relations.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the relation
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Relation
        }
    }
}