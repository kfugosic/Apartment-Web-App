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
