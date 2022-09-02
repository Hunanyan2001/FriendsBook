using System.ComponentModel.DataAnnotations;

namespace IProject.ViewModels
{
    public class RegisterViewModels
    {
        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
        [Display(Name = "RemomberMe?")]
        public bool RememberMe { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }

    }
}
