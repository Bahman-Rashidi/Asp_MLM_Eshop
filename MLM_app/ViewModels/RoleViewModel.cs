using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MLM_app.Models;

namespace MLM_app.ViewModels
{
    // Using these classes, roles can be categorized.
    public class RoleEditModel
    {
        public ApplicationRole Role { get; set; }
        // Distinguish between members and non-members in a role by using these classes
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
    public class RoleModificationModel
    {
        [Required(ErrorMessage ="Enter the Role Name")]
        public string RoleName { get; set; }


        // To add a role to a user
        public string[] IdsToAdd { get; set; }


        // To delete a role from a user
        public string[] IdsToDelete { get; set; }
    }
}