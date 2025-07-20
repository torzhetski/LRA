using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IEmailVerificationRepository
{
    public Task<string> CreateVerificationTokenAsync(string email, CancellationToken cancellationToken);
    public Task<bool> VerifyTokenAsync(string token, string email, CancellationToken cancellationToken);
}
