using System.ComponentModel.DataAnnotations;
using static Eindopdrachtcnd2.Helper.DbEnumeration;

namespace Eindopdrachtcnd2.Data.Entities
{
    public class Card
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public CardStatus Status { get; set; }
       
        public DateTime? Deadline { get; set; }

        public int GroupId { get; set; }

        public int? Sprint { get; set; }

        public Group Group { get; set; }
        public IList<CardTask> CardTasks { get; set; }
        public IList<CardUser> CardUsers { get; set; }

    }


}
