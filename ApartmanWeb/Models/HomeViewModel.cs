﻿using System.Collections.Generic;

namespace ApartmanWeb.Models
{
    public class HomeViewModel
    {
        //public int ImageCounter;
        public bool DirectReservation;
        public List<int> ImageOrder;
        public List<GuestReview> ReviewsList = new List<GuestReview>();

    }
}