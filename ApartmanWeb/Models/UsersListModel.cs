using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmanWeb.Models
{
    public class UsersListModel
    {
        public List<(ApplicationUser, bool)> UserIsAdminTupleList = new List<(ApplicationUser, bool)>();
    }
}
