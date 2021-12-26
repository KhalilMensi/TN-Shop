using System.ComponentModel.DataAnnotations;

namespace AngularAspCore.Models.Entity
{
    public class User
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Veuillez saisir un mail valide")]
        public string? Email { get; set; }
        public string? EmailConfirmed { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime CodeExpirationDate { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Country { get; set; }
        public string? Adress { get; set; }
        public string? PostalCode { get; set; }
        public string? Role { get; set; }
        public IFormFile? Photo { get; set; }
        public string? PhotoFileName { get; set; }
        public DateTime AddedOn { get; set; }

        public User()
        {

        }

        public User(long id, string code, string email, string emailConfirmed, string emailConfirmationCode, DateTime codeExpirationDate, string password, string phone, string country, string adress, string postalCode, string profil, IFormFile photo, string photoFileName, DateTime addedOn)
        {
            Id = id;
            Code = code;
            Email = email;
            EmailConfirmed = emailConfirmed;
            EmailConfirmationCode = emailConfirmationCode;
            CodeExpirationDate = codeExpirationDate;
            Password = password;
            Phone = phone;
            Country = country;
            Adress = adress;
            PostalCode = postalCode;
            Role = profil;
            Photo = photo;
            PhotoFileName = photoFileName;
            AddedOn = addedOn;
        }
    }
}
