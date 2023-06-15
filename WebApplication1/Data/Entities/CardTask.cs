using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using static Eindopdrachtcnd2.Helper.DbEnumeration;

namespace Eindopdrachtcnd2.Data.Entities
{
    public class CardTask
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public CardStatus Status { get; set; }
        
        public int CardId { get; set; }
        public Card Card { get; set; }


    }
}
