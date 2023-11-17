using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MLInstagram.Data;

// Add profile data for application users by adding properties to the MLInstagramUser class
public class MLInstagramUser : IdentityUser
{
    public string? FullName { get; set; }

    public string? IGHandle {  get; set; }
    public string? ProfilePicUrl { get; set; }
}

