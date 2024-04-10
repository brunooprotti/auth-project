namespace auth_backend.services
{
    public static class ServiceExtensions
    {
        //This class will override some methods from Service class of the builder.
        //This class is going to be used for declare all services methods, for maintain the program.cs class clean
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors( options => 
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
    }
}
