using IProject.Models;

namespace IProject.ViewModels
{
    public class FriendViewModel
    {
        public ICollection<User>? Users { get; set; }
        public ICollection<UserFriendShip>? Friends { get; set; }

        public ICollection<FileModel>? FileModels { get; set; }
    }
}
