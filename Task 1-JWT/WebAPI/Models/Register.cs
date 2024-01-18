using System.ComponentModel.DataAnnotations;

namespace Signup.Models {
    public class Register {
        [Key]
        public int Userid { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^(?:[A-Z][a-z]+(?:\s|$))+[A-Z](?:\s[A-Z])*$", ErrorMessage = "Name must be in this format 'Name Initial'")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Id is required")]
        [RegularExpression(@"^[a-z][a-z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[7-9][0-9]{9}$", ErrorMessage = "Please enter a valid mobile number.")]

        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Age is required")]
        [Range(25, int.MaxValue, ErrorMessage = "Age must be above 18.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)(?!.*\s).{8,15}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, one symbol, and be 8-15 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string Confirmpassword { get; set; }
    }
}
