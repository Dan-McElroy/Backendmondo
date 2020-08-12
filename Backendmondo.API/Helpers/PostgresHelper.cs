using System.Text.RegularExpressions;

namespace Backendmondo.API.Helpers
{
    internal static class PostgresHelper
    {
        public static string ConvertUrlToConnectionString(string url)
        {
            const string pattern = @"(postgres\:\/\/)(.+)\:(.*)@(.*):(\d+)\/(.+)";

            var match = Regex.Match(url, pattern);

            return $"User ID={match.Groups[2]};Password={match.Groups[3]};Host={match.Groups[4]};Port={match.Groups[5]};Database={match.Groups[6]};";
        }
    }
}
