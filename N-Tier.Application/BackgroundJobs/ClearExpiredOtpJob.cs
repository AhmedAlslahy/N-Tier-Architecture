using System;
using System.Collections.Generic;
using System.Text;

namespace N_Tier.Application.BackgroundJobs;

public sealed class ClearExpiredOtpJob(SarhneDbContext context)
{
    public async Task ExecuteAsync()
    {
        var now = DateTime.UtcNow;

        await context.Users
            .Where(u => u.OTP != null &&
                        u.OTPExpire != null &&
                        u.OTPExpire <= now)
            .ExecuteUpdateAsync(x => x
                .SetProperty(u => u.OTP, (string?)null)
                .SetProperty(u => u.OTPExpire, (DateTime?)null));
    }
}