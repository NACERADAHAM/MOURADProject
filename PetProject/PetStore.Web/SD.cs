using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Web
{
    public static class SD
    {

        public static string ProductApiBase { get; set; }
        public static string   ShoppingApiBase  { get; set; }
        public static string CouponApiBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE


        }
    }
}
