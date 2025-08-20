using System.ComponentModel.DataAnnotations;

namespace MyImageApp.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navegação para o usuário
        public ApplicationUser? User { get; set; }
    }
}