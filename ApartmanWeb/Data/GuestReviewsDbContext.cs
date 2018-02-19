using System.Data.Entity;
using ApartmanWeb.Models;

namespace ApartmanWeb.Data
{
    public class GuestReviewsDbContext : DbContext
    {
        public GuestReviewsDbContext(string cnnstr) : base(cnnstr)
        {
        }

        public IDbSet<GuestReview> GuestReviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuestReview>().HasKey(t => t.Id);
            modelBuilder.Entity<GuestReview>().Property(t => t.Approved).IsRequired();
            modelBuilder.Entity<GuestReview>().Property(t => t.DateCreated).IsRequired();
            modelBuilder.Entity<GuestReview>().Property(t => t.GuestPermission).IsRequired();
            modelBuilder.Entity<GuestReview>().Property(t => t.GuestUserId).IsRequired();
            modelBuilder.Entity<GuestReview>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<GuestReview>().Property(t => t.Country).IsRequired();
            modelBuilder.Entity<GuestReview>().Property(t => t.Score).IsRequired();
        }
    }
}