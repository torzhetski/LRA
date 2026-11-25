using LRA.Account.Domain.Enums;
using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Account.Domain.Entities;

public class KycEntity : IBaseEntity
{
    public Guid Id {  get; set; }
    public required Guid AccountId { get; set; }
    public Guid? AdminIdWhoReview { get; set; }
    public string? RejectMessage { get; set; }
    public required string IdentityDocumentUrl { get; set; }
    public required string MedicalCertificationUrl { get; set; }
    public required string SelfieWithIdentityDocumentUrl { get; set; }
    public required KycStatus Status { get; set; }
}
