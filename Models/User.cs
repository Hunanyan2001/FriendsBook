using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IProject.Models
{
    public class User:IdentityUser
    {
        [Key]
        public int Year { get; set; }
        public ICollection<FileModel>? FileModels { get; set; }

        public ICollection<PhotoCovers>? PhotoCovers { get; set; }

    }
}
