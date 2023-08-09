using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace msa_nasa_project.Models
{
    public class Images
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string UserID { get; set; }
        
    }
}
