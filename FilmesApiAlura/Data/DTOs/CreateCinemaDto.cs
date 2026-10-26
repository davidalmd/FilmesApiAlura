using System.ComponentModel.DataAnnotations;

namespace FilmesApiAlura.Data.DTOs;

public class CreateCinemaDto
{
    [Required(ErrorMessage = "O campo de nome é obrigatório.")]
    public string Nome { get; set; }
}
