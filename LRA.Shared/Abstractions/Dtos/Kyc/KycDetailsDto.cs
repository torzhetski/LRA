namespace LRA.Shared.Abstractions.Dtos.Kyc;

public class KycDetailsDto
{
    public required Guid JobApplicationId { get; set; }
    public required Guid AccountId { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string IdentityDocumentUrl { get; set; }
    public required string MedicalCertificationUrl { get; set; }
    public required string SelfieWithIdentityDocumentUrl { get; set; }
    public string? IdentityDocumentBase64 { get; set; }
    public string? MedicalCertificationBase64 { get; set; }
    public string? SelfieWithIdentityDocumentBase64 { get; set; }
    public string? RejectMessage { get; set; }
    public required string Status { get; set; }
}
