using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthFit.Object_Provider.Model
{
    [Serializable]
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [ValidateNever]
        public string? Address { get; set; }

        [ValidateNever]
        public string? City { get; set; }

        [ValidateNever]
        public string? State { get; set; }

        [ValidateNever]
        public string? Country { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [ValidateNever]
        public string PhoneNo { get; set; }

        private string _Website = string.Empty;
        [ValidateNever]
        public string? Website { get { return _Website; } set { _Website = value; } }

        [Required]
        public byte UserType { get; set; }

        [Required]
        public string UserName { get; set; }

        private string _HashedPassword = string.Empty;
        [ValidateNever]
        public string HashedPassword { get { return _HashedPassword; } set { _HashedPassword = value; } }

        private string _PasswordSalt = string.Empty;
        [ValidateNever]
        public string PasswordSalt { get { return _PasswordSalt; } set { _PasswordSalt = value; } }

        private bool _isActive = true;
        [ValidateNever]
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        [ValidateNever]
        [NotMapped]
        public ICollection<UserSubscriptionsDetails> Journals { get; set; }
    }
}