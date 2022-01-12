using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeCrudSimple.Models
{
    public class CustomUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
