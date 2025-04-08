using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "UserName is Required !!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FirstName is Required !!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required !!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and ConfirmPassword do not match !!")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
