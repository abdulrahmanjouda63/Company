using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and ConfirmPassword do not match !!")]
        public string ConfirmNewPassword { get; set; }
    }
}
