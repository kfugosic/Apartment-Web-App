using System.Data.Entity.Migrations;
using System.Linq;
using ApartmanWeb.Models;

namespace ApartmanWeb.Data
{
    public class ApplicationSettingsSqlRepository : IApplicationSettingsRepository
    {

        private readonly ApplicationSettingsDbContext _dbContext;

        public ApplicationSettingsSqlRepository(ApplicationSettingsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApplicationSettings Get()
        {
            return _dbContext.AppSettings.FirstOrDefault();            
        }

        public bool Add(ApplicationSettings appSettings)
        {
            if (this.Get() != null)
            {
                return false;
            }
            _dbContext.AppSettings.Add(appSettings);
            _dbContext.SaveChanges();
            return true;

        }

        public bool Remove(int id)
        {
            var item = _dbContext.AppSettings.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return false;
            }
            _dbContext.AppSettings.Remove(item);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Update(ApplicationSettings newSettings)
        {
            var item = _dbContext.AppSettings.FirstOrDefault(t => t.Id == newSettings.Id);
            if (item == null)
            {
                return false;
            }
            _dbContext.AppSettings.AddOrUpdate(newSettings);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
