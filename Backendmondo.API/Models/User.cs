using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backendmondo.API.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool MatchesEmailAddress(string email)
        {
            return string.Compare(Email.Trim(), email.Trim(), true) == 0;
        }
    }
}
