using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ExternalAuthentication.Areas.Identity.Data;

// Add profile data for application users by adding properties to the OAuth_IdentityUser class
public class OAuth_IdentityUser : IdentityUser
{
}

