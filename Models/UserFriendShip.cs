namespace IProject.Models
{
    public class UserFriendShip
    {
        public string? UserId { get; set; }  
        public User? User { get; set; }

        public string? UserFriendId { get;set; }
        public User? UserFriend { get; set; }

    }
}
