﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PetStore.Web.SD;
namespace PetStore.Web.Models
{
    public class ApiRequest
    {

        public ApiType ApiType { get; set; } = ApiType.GET;


        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }



    }
}
