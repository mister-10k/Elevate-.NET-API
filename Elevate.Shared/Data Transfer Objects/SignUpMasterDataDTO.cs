﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class SignUpMasterDataDTO
    {
        public List<ComapnyDTO> Companies { get; set; }
        public List<UserTypeDTO> UserTypes { get; set; }
    }
}