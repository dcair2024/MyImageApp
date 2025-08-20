using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyImageApp.Models
{
    public class ImageHistory
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }        
        
        [Required]
        public string? Prompt { get; set; }         
        [Required]
        public string? ImageUrl { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    }
}
