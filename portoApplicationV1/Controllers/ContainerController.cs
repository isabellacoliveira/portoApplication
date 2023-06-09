using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portoApplicationV1.Data;
using portoApplicationV1.DTOs.Container;
using setorPortuario.Models;

namespace portoApplicationV1.Controllers
{
[ApiController]
[Route("[controller]")]
    public class ContainerController : ControllerBase
    {
    private PortoContext _context;
    private IMapper _mapper;

        public ContainerController(PortoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpPost]
        public IActionResult adicionaContainer([FromBody] CreateContainerDTO containerDto)
        {
            Container container = _mapper.Map<Container>(containerDto);
            _context.Containers.Add(container); 
            _context.SaveChanges();
            return CreatedAtAction(nameof(buscarContainerPorId), new { id = container.Id }, container);

        }

    [HttpGet]
    public IEnumerable<ContainerResponseDTO> buscarMovimentacao([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var containers = _context.Containers
            .Include(c => c.Movimentacoes)
            .Skip(skip)
            .Take(take)
            .ToList();

        return _mapper.Map<List<ContainerResponseDTO>>(containers);
    }


    [HttpGet("{id}")]
    public IActionResult buscarContainerPorId(int id)
    {
        var container = _context.Containers.FirstOrDefault(container => container.Id == id);
        if(container == null) return NotFound();
        var containerDto = _mapper.Map<ContainerResponseDTO>(container);
        return Ok(containerDto);
    }


    [HttpPut("{id}")]
        public IActionResult AtualizaContainer(int id, 
                [FromBody] UpdateContainerDTO containerDTO)
        {
            var container = _context.Containers.FirstOrDefault(
                container => container.Id == id);
                if(container == null) return NotFound();
                _mapper.Map(containerDTO, container);
                _context.SaveChanges();
            return NoContent(); 
        }

    [HttpDelete("{id}")]
        public IActionResult DeletaContainer(int id)
        {
            var container = _context.Containers.FirstOrDefault(
            container => container.Id == id);
            if(container == null) return NotFound();

            _context.Remove(container); 
            _context.SaveChanges();
            return NoContent();
        }


    }
}


