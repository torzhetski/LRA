using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRA.Shared.Abstractions.Models.Auth;

public class TwoFactorLoginModel : LoginModel
{
    public required string TwoFactorToken { get; set; }
}
