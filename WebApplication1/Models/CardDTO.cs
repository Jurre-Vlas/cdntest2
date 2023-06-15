using static Eindopdrachtcnd2.Helper.DbEnumeration;

namespace Eindopdrachtcnd2.Models.DTO
{
    public class CardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CardStatus Status { get; set; }
        public DateTime? Deadline { get; set; }
        public int GroupId { get; set; }
        public int? Sprint { get; set; }

    }
}
