using System;
using System.Collections.Generic;

namespace ApartmanWeb.Models
{
    public class GuestReview
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Review { get; set; }
        public string Suggestions { get; set; }
        public int Score { get; set; }
        public bool GuestPermission { get; set; }
        public bool Approved { get; set; }
        public Guid  GuestUserId { get; set; }
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Emptry constructor, should not be used. (Only for EF)
        /// </summary>
        public GuestReview()
        {
        }

        public GuestReview(string name, string country, string review, string suggestions, int score, bool guestPermission, bool approved, Guid guestId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Country = country;
            Review = review;
            Suggestions = suggestions;
            Score = score;
            GuestPermission = guestPermission;
            Approved = approved;
            GuestUserId = guestId;
            DateCreated = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            var review = obj as GuestReview;
            return review != null &&
                   Id.Equals(review.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }
    }
}
