using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRA.Shared.Abstractions.Dtos.Verification;

public class VerificationDto
{
    public bool IsVerificationRequired { get; set; }
    public KeycloakTokenDto? Tokens { get; set; }
}
