﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Contact
{
    public class GetContactDTO
    {
        public string Key { get; set; } = null!;
        public List<string>? Values { get; set; }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
