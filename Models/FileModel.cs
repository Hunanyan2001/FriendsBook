using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IProject.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }
    }
}
