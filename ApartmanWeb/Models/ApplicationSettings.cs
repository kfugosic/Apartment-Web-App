using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmanWeb.Models
{
    public class ApplicationSettings
    {
        public int Id { get; set; }
        public bool DirectReservation { get; set; }
        public string Order { get; set; }

        /// <summary>
        /// Emptry constructor, should not be used. (Only for EF)
        /// </summary>
        public ApplicationSettings()
        {
        }

        public ApplicationSettings(bool directEnabled, string imagesOrder)
        {
            Id = 0;
            DirectReservation = directEnabled;
            Order = imagesOrder;
        }

        public override bool Equals(object obj)
        {
            var settings = obj as ApplicationSettings;
            return settings != null &&
                   Id.Equals(settings.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
