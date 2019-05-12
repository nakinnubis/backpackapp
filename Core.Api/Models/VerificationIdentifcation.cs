using System;

namespace Core.Api.Models
{
    public class VerificationIdentifcation
    {
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public int Nationality { get; set; }
        public int IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string base64Img { get; set; }
    }
}