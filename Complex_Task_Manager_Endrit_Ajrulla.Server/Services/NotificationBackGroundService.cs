namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NotificationBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
                        await notificationService.SendTaskReminders();
                    }

                    // Wait for 1 hour before sending the next reminder
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            
        }
    }
}
