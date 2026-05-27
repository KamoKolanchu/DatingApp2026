using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class BlockRequestDto
{
    [Required(ErrorMessage = "Reason is required")]
    [MinLength(1, ErrorMessage = "Reason must be at least 1 character")]
    [MaxLength(200, ErrorMessage = "Reason cannot exceed 200 characters")]
    public required string Reason { get; set; }
}