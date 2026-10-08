using System.ComponentModel.DataAnnotations;

namespace FilmesApiAlura.Data.DTOs;

public class ReadFilmeDTO
{
    public string Titulo { get; set; }
    public string Diretor { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }
    public DateTime HoraConsulta { get; set; } = DateTime.UtcNow;
}
