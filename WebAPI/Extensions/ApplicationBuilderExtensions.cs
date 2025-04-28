namespace WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseApplicationMiddleware(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
