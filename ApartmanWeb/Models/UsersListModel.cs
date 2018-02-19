using System.Collections.Generic;

namespace ApartmanWeb.Models
{
    public class UsersListModel
    {
        public List<(ApplicationUser, bool)> UserIsAdminTupleList = new List<(ApplicationUser, bool)>();
    }
}
