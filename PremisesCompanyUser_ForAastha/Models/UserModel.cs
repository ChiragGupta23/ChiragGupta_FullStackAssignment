using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ChiragGupta_FullStackAssignment.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserModelDTO
    {
        [Key]
        public int UserID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string password { get; set; }
        
    }

    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }


        [Column(TypeName = "nvarchar(10)")]
        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }


        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

    }

    public class LoginDTO
    {
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string password { get; set; }
    }

    public class Login
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }


    }


}
