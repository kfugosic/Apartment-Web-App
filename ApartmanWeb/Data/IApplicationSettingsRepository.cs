using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApartmanWeb.Models;

namespace ApartmanWeb.Data
{
    public interface IApplicationSettingsRepository
    {
        ApplicationSettings Get();

        bool Add(ApplicationSettings appSettings);

        bool Remove(int id);

        bool Update(ApplicationSettings newSettings);

    }
}
