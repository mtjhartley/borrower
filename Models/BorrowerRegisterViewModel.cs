using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace borrower.Models
{
    public class BorrowerRegisterViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"[a-zA-Z]+$")]
        public string FirstName {get;set;}

        [Required]
        [MinLength(2)]
        [RegularExpression(@"[a-zA-Z]+$")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirmation {get;set;}

        [Required]
        public int Amount {get;set;}

        [Required]
        public string Reason {get;set;}
        [Required]
        [MinLength(5, ErrorMessage = "Enter at least 5 characters")]
        public string Description{get;set;}
    }
}