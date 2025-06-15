namespace API.Core.Extensions.Auth
{
    public static class GoogleAuthentication
    {
        public static IServiceCollection AddGoogleAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var googleSettings = config.GetSection("GoogleAPI");

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = googleSettings["ClientId"];
                options.ClientSecret = googleSettings["Key"];
                options.CallbackPath = "/signedin-google";
            });

            return services;
        }
    }
}
