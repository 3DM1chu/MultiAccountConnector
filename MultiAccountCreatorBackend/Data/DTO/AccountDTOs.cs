namespace MultiAccountCreatorBackend.Data.DTO
{
    public record AccountDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? ServiceName { get; set; }
    }

    public record AccountCreatedDTO
    {
        public required string Name { get; set; }
    }
}
