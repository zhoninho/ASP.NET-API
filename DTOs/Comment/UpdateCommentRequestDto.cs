using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment;

public class UpdateCommentRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Заголовок должен состоять из 5 символов")]
    [MaxLength(280, ErrorMessage = "Заголовок не может быть длиннее 280 символов")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "Содержание должено состоять мз 5 символов")]
    [MaxLength(280, ErrorMessage = "Содержание не может быть длиннее 280 символов")]
    public string Content { get; set; } = string.Empty;
}