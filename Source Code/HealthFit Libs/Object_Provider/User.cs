using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace HealthFit.Object_Provider.Model
{
    [Serializable]
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        [Required]
        public byte UserType { get; set; }
        [Required]
        public string UserName { get; set; }
        
        private string _HashedPassword = string.Empty;
        public string HashedPassword { get { return _HashedPassword; } set { value = _HashedPassword; } }
        
        private string _PasswordSalt = string.Empty;
        public string PasswordSalt { get { return _PasswordSalt; } set { value = _PasswordSalt; } }
        
        private bool _isActive = true;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    }
}