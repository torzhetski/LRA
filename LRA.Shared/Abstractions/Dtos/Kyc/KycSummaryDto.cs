namespace LRA.Shared.Abstractions.Dtos.Kyc;

public class KycSummaryDto
{
    public required Guid JobApplicationId { get; set; }
    public required Guid AccountId { get; set; }
    public required string Email { get; set; }
    public required string Status { get; set; }
}
