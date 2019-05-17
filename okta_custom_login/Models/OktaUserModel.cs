﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace okta_custom_login.Models
{
    public class OktaUserModel
    {
        public ProfileModel profile { get; set; }

        [JsonIgnore]
        public string Key { get; set; }
    }
}
