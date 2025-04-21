﻿using Core.Entities.Role;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
        public class AppRole : IdentityRole<string>
        {
            public AppRole()
            {
                Id = Guid.NewGuid().ToString();
            }
            public RoleModel RoleModel { get; set; }
    }
}
