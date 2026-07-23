using Hangfire;

namespace N_Tier.Application.BackgroundJobs;

public static class HangfireScheduler
{
    public static void RegisterJobs()
    {
        RecurringJob.AddOrUpdate<DeleteOldNotificationsJob>(
          "delete-old-notifications",
          job => job.ExecuteAsync(),
          Cron.Daily);

        RecurringJob.AddOrUpdate<ClearExpiredOtpJob>(
         "clear-expired-otp",
          x => x.ExecuteAsync(),
          Cron.MinuteInterval(10));
    }
}