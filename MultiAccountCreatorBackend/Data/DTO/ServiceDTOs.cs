namespace MultiAccountCreatorBackend.Data.DTO
{
    public record CreateServiceDTO
    {
        public required string Name { get; set; }
    }

    public record ServiceDTO
    {
        public required string Name { get; set; }
        public List<AccountDTO>? Accounts { get; set; }
    }
}
