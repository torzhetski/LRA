namespace LRA.Shared.Abstractions.Models;

public class EmailMessageModel
{
    public required string RecipientMail { get; set; }
    public required string Content { get; set; }
}
