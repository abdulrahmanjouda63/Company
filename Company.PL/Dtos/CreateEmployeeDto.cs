// Ignore Spelling: Dto Dtos Emp

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [RegularExpression(@"^[0-9]{1,10}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}$",
           ErrorMessage = "Address must be like '123-Street-Region-City' ")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }
        [DisplayName("Create At")]
        public DateTime CreateAt { get; set; }
        public int? DepartmentId { get; set; }
    }
}
