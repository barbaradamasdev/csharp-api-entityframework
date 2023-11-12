using DesafioListaDeTarefas.Context;
using DesafioListaDeTarefas.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioListaDeTarefas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context) {
            _context = context;
        }

        // Endpoint Obter Tarefa por Id
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        // Endpoint Obter todas as tarefas
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var contatos = _context.Tarefas;
            return Ok(contatos);
        }

        // Endpoint Obter tarefa por titulo
        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefa = _context.Tarefas.Where( x=> x.Titulo.Contains(titulo));
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        // Endpoint Obter tarefa por data
        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        // Endpoint Obter tarefa por status
        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }
        
        // Endpoint Criar tarefa
        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        // Endpoint Atualizar tarefa
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null) return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Add(tarefa);
            _context.SaveChanges();
            return Ok(tarefaBanco);
        }

        // Endpoint Apagar tarefa
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null) return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}