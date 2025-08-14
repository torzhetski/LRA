namespace LRA.Shared.Abstractions.Dtos.Kyc;

public class KycDetailsDto
{
    public required Guid AccountId { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string IdentityDocumentUrl { get; set; }
    public required string MedicalCertificationUrl { get; set; }
    public required string SelfieWithIdentityDocumentUrl { get; set; }
    public string? RejectMessage { get; set; }
    public required string Status { get; set; }
}
