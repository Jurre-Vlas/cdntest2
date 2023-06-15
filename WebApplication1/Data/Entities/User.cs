using Microsoft.AspNetCore.Identity;

namespace Eindopdrachtcnd2.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public IList<GroupUser> GroupUsers { get; set; }
        public IList<CardUser> CardUsers { get; set; }

    }
}
