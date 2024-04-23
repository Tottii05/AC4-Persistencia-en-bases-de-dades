using Microsoft.Extensions.Configuration;

namespace ac3
{
    public class NpgsqlUtils
    {
        public static string OpenConnection()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonPath = Path.Combine(basePath, "appsetting.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(jsonPath, optional: false, reloadOnChange: true)
                .Build();

            return config.GetConnectionString("MyPostgresConn");
        }
    }
}
