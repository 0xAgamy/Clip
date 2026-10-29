using Microsoft.AspNetCore.Identity;
using System;

namespace Clips_api.Models
{
    public class User:IdentityUser
    {

        public DateTime CreateedOn { get; set; }= DateTime.UtcNow;

    }
}
