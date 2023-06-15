using static Eindopdrachtcnd2.Helper.DbEnumeration;

namespace Eindopdrachtcnd2.Models.DTO
{
    public class CardTaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CardStatus Status { get; set; }
        public int CardId { get; set; }
    }
}
