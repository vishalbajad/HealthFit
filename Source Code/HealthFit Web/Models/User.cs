using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HealthFit_Web.Models
{
    public class User
    {
        HealthFit.Object_Provider.Model.User _UserDetails = new HealthFit.Object_Provider.Model.User();

        [BindProperty]
        public HealthFit.Object_Provider.Model.User UserDetails { get { return _UserDetails; } set { _UserDetails = value; } }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [BindProperty]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
