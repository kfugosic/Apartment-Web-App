using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using ApartmanWeb.Models;

namespace ApartmanWeb.Data
{
    public class GuestReviewsSqlRepository : IGuestReviewsRepository
    {
        private GuestReviewsDbContext _dbContext;

        public GuestReviewsSqlRepository(GuestReviewsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GuestReview Get(Guid guestId)
        {
            return _dbContext.GuestReviews.FirstOrDefault(t => t.GuestUserId == guestId);
        }

        public bool Remove(Guid id)
        {
            GuestReview review = _dbContext.GuestReviews.FirstOrDefault(t => t.Id == id);
            if (review == null)
            {
                return false;
            }
            _dbContext.GuestReviews.Remove(review);
            _dbContext.SaveChanges();
            return true;
        }

        public bool RemoveForUser(Guid userId)
        {
            GuestReview review = _dbContext.GuestReviews.FirstOrDefault(t => t.GuestUserId == userId);
            if (review == null)
            {
                return false;
            }
            _dbContext.GuestReviews.Remove(review);
            _dbContext.SaveChanges();
            return true;
        }

        public void AddOrUpdate(GuestReview review)
        {
            _dbContext.GuestReviews.AddOrUpdate(review);
            _dbContext.SaveChanges();
        }

        public bool setApproved(Guid id)
        {
            GuestReview review = _dbContext.GuestReviews.FirstOrDefault(t => t.Id == id);
            if (review == null)
            {
                return false;
            }
            review.Approved = true;
            _dbContext.SaveChanges();
            return true;
        }

        public bool setDisapproved(Guid id)
        {
            GuestReview review = _dbContext.GuestReviews.FirstOrDefault(t => t.Id == id);
            if (review == null)
            {
                return false;
            }
            review.Approved = false;
            _dbContext.SaveChanges();
            return true;
        }

        public List<GuestReview> getAll()
        {
            return _dbContext.GuestReviews
                .OrderBy(t => t.DateCreated)
                .ToList();
        }

        public List<GuestReview> getApproved()
        {
            return _dbContext.GuestReviews
                .Where(t => t.Approved)
                .OrderBy(t => t.DateCreated)
                .ToList();
        }

        public List<GuestReview> getApprovedAndWithPermission()
        {
            return _dbContext.GuestReviews
                .Where(t => t.Approved && t.GuestPermission)
                .OrderBy(t => t.DateCreated)
                .ToList();
        }
    }
}
