using System.ComponentModel.DataAnnotations;

namespace Eindopdrachtcnd2.Data.Entities
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public IList<GroupUser> GroupUsers { get; set; }

        public IList<Card> Cards { get; set; }

    }
}
