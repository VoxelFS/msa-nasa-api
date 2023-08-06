using System.ComponentModel.DataAnnotations;

namespace msa_nasa_project.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Key]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }
}
