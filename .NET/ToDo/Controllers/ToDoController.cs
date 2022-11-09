using Microsoft.AspNetCore.Mvc;
using TaskManager.Model.ToDo;
using TaskManager.Services;
using TaskManager.Services.DTO;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoService _service;
        public ToDoController(ToDoService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<QueryToDo>>> GetToDo(CancellationToken token)
        {
            IEnumerable<QueryToDo> toDos = await _service.GetToDo(token);
            return Ok(toDos);
        }

        [HttpGet("{id}", Name="GetToDoById")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDoById(int id, CancellationToken token)
        {
            IEnumerable<QueryToDo> toDos = await _service.GetToDoById(id, token);
            if (!toDos.Any())
            {
                return NotFound();
            }
            return Ok(toDos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDo(int id, CancellationToken token)
        {
            bool flag = await _service.DeleteToDo(id, token);
            if (!flag)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<QueryToDo>>> InsertToDo(MutateToDo dto, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<QueryToDo> todo = await _service.InsertToDo(dto, token);
                if (todo.Any())
                {
                    return CreatedAtAction(nameof(GetToDoById), new {todo.First().Id}, todo.First() );
                }
                return StatusCode(500);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateToDo(int id, MutateToDo dto, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _service.UpdateToDo(id, dto, token);
                if (!flag)
                {
                    return NotFound();
                }
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> ChangeStatusToDo(int id, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _service.ChangeStatusToDo(id, token);
                if (!flag)
                {
                    return NotFound();
                }
                return NoContent();
            }
            return BadRequest();
        }
    }
}
