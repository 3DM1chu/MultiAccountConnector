using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiAccountCreatorBackend.Data
{
    [Table("Services")]
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        public List<Account> Accounts { get; set; } = [];
    }

    public class UberService : Service
    {
        public string ProfileDetails { get; set; } = "";
    }
}
