using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiAccountCreatorBackend.Data
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Email { get; set; } = "";

        public int? ServiceId { get; set; } // Foreign Key
        public Service? Service { get; set; }
    }
}
