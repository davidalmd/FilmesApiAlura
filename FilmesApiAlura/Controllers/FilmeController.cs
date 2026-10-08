using AutoMapper;
using FilmesApiAlura.Data;
using FilmesApiAlura.Data.DTOs;
using FilmesApiAlura.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApiAlura.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a filmes
/// </summary>
[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private readonly FilmeContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor da classe FilmeController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os dados necessários do filme para adição</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Filme criado com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDTO filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperarFilmePorId), new { id = filme.Id }, filme);
    }

    /// <summary>
    /// Recupera uma lista de filmes do banco de dados com paginação
    /// </summary>
    /// <param name="skip">Número de registros a serem ignorados</param>
    /// <param name="take">Número de registros a serem recuperados</param>
    /// <returns>Lista de filmes</returns>
    [HttpGet]
    public IEnumerable<ReadFilmeDTO> RecuperarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var filmes = _context.Filmes.Skip(skip).Take(take);
        return _mapper.Map<IEnumerable<ReadFilmeDTO>>(filmes);
    }

    /// <summary>
    /// Recupera um filme específico do banco de dados pelo seu ID
    /// </summary>
    /// <param name="id">Valor do ID do filme a ser recuperado</param>
    /// <returns>Objeto com os dados do filme</returns>
    [HttpGet("{id}")]
    public IActionResult RecuperarFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDTO>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza um filme existente no banco de dados pelo seu ID
    /// </summary>
    /// <param name="id">Valor do ID do filme a ser atualizado</param>
    /// <param name="filmeDto">Objeto com os dados necessários do filme para atualização</param>
    /// <returns>Informação de que o filme foi atualizado com sucesso</returns>
    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDTO filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza parcialmente um filme existente no banco de dados pelo seu ID
    /// </summary>
    /// <param name="id">Valor do ID do filme a ser atualizado</param>
    /// <param name="patch">Documento de patch com as alterações a serem aplicadas</param>
    /// <returns>Informação de que o filme foi atualizado com sucesso</returns>
    [HttpPatch("{id}")]
    public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDTO> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDTO>(filme);

        patch.ApplyTo(filmeParaAtualizar);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta um filme existente no banco de dados pelo seu ID
    /// </summary>
    /// <param name="id">Valor do ID do filme a ser deletado</param>
    /// <returns>Informação de que o filme foi deletado com sucesso</returns>
    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
