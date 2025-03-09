using ECommerce.EmailAPI.Messaging;

namespace ECommerce.EmailAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer? _ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            return app; //Adding this here while i don't have valid token.
            _ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();

            if (_ServiceBusConsumer == null)
            {
                throw new InvalidOperationException("IAzureServiceBusConsumer service is not registered.");
            }

            var hostApplicationLifeTime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            if (hostApplicationLifeTime == null) return app;

            hostApplicationLifeTime.ApplicationStarted.Register(OnStart);
            hostApplicationLifeTime.ApplicationStopped.Register(OnStop);

            return app;
        }

        private static void OnStart()
        {
            _ServiceBusConsumer?.start();
        }
        private static void OnStop()
        {
            _ServiceBusConsumer?.stop();
        }
    }
}
