using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Navigation Properties
        public virtual IList<Cargo> SentCargos { get; set; }
        public virtual IList<Cargo> ReceivedCargos { get; set; }
        public virtual IList<Address> Addresses { get; set; }
    }
}
