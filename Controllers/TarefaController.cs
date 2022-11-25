using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Models.Dto;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            Tarefa tarefa = _context.Tarefas.Find(id);

            if (tarefa is null)
            {
                return NotFound();
            }
            return Ok(tarefa);

        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            List<Tarefa> tarefas = _context.Tarefas.ToList();
       
            return Ok(tarefas);
       
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            Tarefa tarefa = _context.Tarefas.Where(x => x.Titulo.Equals(titulo)).FirstOrDefault();
            
            if (tarefa is null)
            {
                return NotFound();
            }

            return Ok(tarefa);

        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            List<Tarefa> tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();
            
            if (tarefas is null)
            {
                return NotFound();
            }

            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            List<Tarefa> tarefas = _context.Tarefas.Where(x => x.Status == status).ToList();
            
            if (tarefas is null)
            {
                return NotFound();
            }

            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult Criar(TarefaDTO tarefaDTO)
        {
            var tarefa = new Tarefa {
                Titulo = tarefaDTO.Titulo,
                Descricao = tarefaDTO.Descricao,
                Data = tarefaDTO.Data,
                Status = tarefaDTO.Status
            };

            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, TarefaDTO tarefaDTO)
        {            
            Tarefa tarefaBanco = _context.Tarefas.Find(id);
            
            if (tarefaBanco == null)        
                return NotFound($"Tarefa com id {id} não encontrado");
            
            if (tarefaDTO.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            
            if (tarefaDTO.Titulo == String.Empty)
                return BadRequest(new { Erro = "O título da tarefa não pode ser vazia" });

            if (tarefaDTO.Descricao == String.Empty)
                return BadRequest(new { Erro = "A descrição da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefaDTO.Titulo;
            tarefaBanco.Descricao = tarefaDTO.Descricao;
            tarefaBanco.Data = tarefaDTO.Data;
            tarefaBanco.Status = tarefaDTO.Status;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            Tarefa tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
            {
                return NotFound();
            }
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
