using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Archi.Library.Data;

namespace Archi.API.Models;

public class PizzaModel : BaseModel
{


    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "{0} cannot be longer than {1} characters")]
    public string? Base { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long")]
    [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters")]
    //[Column("UnMaxDeViande")]
    public string Composition { get; set; } = string.Empty;

    [Range(0.0, 100.0, ErrorMessage = "{0} must be between {1} and {2}")]
    public decimal Price { get; set; }

}