using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmanWeb.Data
{
    public static class GlobalSettings
    {

        public static readonly object OrderLock = new object();

        private static int _order;

        public static int Order
        {
            get
            {
                lock (OrderLock)
                {
                    return _order;
                }
            }
            set
            {
                lock (OrderLock)
                {
                    _order = value;
                }
            }
        }

    }
}
