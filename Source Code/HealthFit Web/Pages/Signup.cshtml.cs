﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthFit_Web.Pages
{
    public class SignupModel : PageModel
    {
        private readonly ILogger<SignupModel> _logger;

        public SignupModel(ILogger<SignupModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}