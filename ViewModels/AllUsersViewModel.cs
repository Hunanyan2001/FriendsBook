using IProject.Models;

namespace IProject.ViewModels
{
    public class AllUsersViewModel
    {
        public ICollection<User>? Users { get; set; }
        public ICollection<FileModel>? Photo { get; set; }

        public ICollection<PhotoCovers>? CoversPhoto { get; set; }

        public ICollection <UserFriendShip>? Friends { get; set; }
    }
}
