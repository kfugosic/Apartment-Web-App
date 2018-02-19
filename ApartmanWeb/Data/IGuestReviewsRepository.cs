using System;
using System.Collections.Generic;
using ApartmanWeb.Models;

namespace ApartmanWeb.Data
{
    public interface IGuestReviewsRepository
    {
        GuestReview Get(Guid guestId);

        bool Remove(Guid id);
        bool RemoveForUser(Guid userId);

        void AddOrUpdate(GuestReview review);

        bool setApproved(Guid id);

        bool setDisapproved(Guid id);

        List<GuestReview> getAll();

        List<GuestReview> getApproved();

        List<GuestReview> getApprovedAndWithPermission();


    }
}
