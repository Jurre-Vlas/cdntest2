namespace Eindopdrachtcnd2.Data.Entities
{
    public class CardUser
    {
        public int CardId { get; set; }
        public Card Card { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}