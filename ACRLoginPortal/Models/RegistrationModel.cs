using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACRLoginPortal.Models
{
    public class RegistrationModel
    {
        public ProfileModel profile { get; set; }
        public CredentialsModel credentials { get; set; }

        [JsonIgnore]
        public string Key { get; set; }
    }

    public class ProfileModel
    {
        [Display(Name = "First Name:")]
        [Required(ErrorMessage = "First Name is required.")]
        public string firstName { get; set; }

        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string lastName { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression("^([\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4})?$", ErrorMessage = "Enter a valid email.")]
        public string email { get; set; }
        
        [Display(Name = "Phone Number:")]
        [Phone(ErrorMessage = "Please enter valid phone number.")]
        public string primaryPhone { get; set; }

        [Display(Name = "Institution Name")]

        public string organization { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Institution Address")]
        public string institutionAddress { get; set; }

        [Display(Name = "Department")]
        public string department { get; set; }


        public string login { get; set; }

        
    }

    public class CredentialsModel
    {
        public PasswordModel password { get; set; }
    }
    public class PasswordModel
    {
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        
        public string value { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("value")]
        [JsonIgnore]
        public string ConfirmPassword { get; set; }
    }
}
