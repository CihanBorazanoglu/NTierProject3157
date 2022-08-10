using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
    public class UserMap : BaseMap<AppUser>
    {
        public UserMap()
        {
            HasOptional(x => x.Profile).WithRequired(x => x.User);
        }
    }
}
