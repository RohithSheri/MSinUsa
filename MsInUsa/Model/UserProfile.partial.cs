using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MsInUsa.Model
{
    [MetadataType(typeof(UserProfileMetadata))]
    public partial class UserProfile
    {
        [Compare("Password")][Required(ErrorMessage="Confirm Password is required")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Enter the Email")]
        [Remote("isUserAvailable", "Account", ErrorMessage = "Email Id already registered")]
        //[RegularExpression(@"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$",ErrorMessage="Enter a valid format")]
        public string Email_IdRegister{ get; set; }

        public int DOBday { get; set; }
        public int DOBYear { get; set; }
        public string DOBMonth { get; set; }
        public List<SelectListItem> doBirthYear { get; set; }
    }

    public class UserProfileMetadata{
        [Required(ErrorMessage="Enter the First Name")]
        [RegularExpression(@"^[A-z]+$",ErrorMessage="Enter the Valid Name with only alphabets")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter the Last Name")]
        [RegularExpression(@"^[A-z]+$", ErrorMessage = "Enter the Valid Name with only alphabets")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter the Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Minimum six characters needed")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter the Email")]
        //[RegularExpression(@"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$",ErrorMessage="Enter a valid format")]
        public string Email_Id { get; set; }
        
        [Required(ErrorMessage = "Enter the Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string Phone_Number { get; set; }
     }

}