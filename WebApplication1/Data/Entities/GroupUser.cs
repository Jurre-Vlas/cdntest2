namespace Eindopdrachtcnd2.Data.Entities
{
    public class GroupUser
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string Role { get; set; }
    }
}
