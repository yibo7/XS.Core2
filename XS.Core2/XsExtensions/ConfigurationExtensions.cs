
namespace XS.Core2.XsExtensions
{
    using System.Collections.Specialized;

    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets value for the specified key.
        /// </summary>
        public static string GetSettings(this NameValueCollection appSettings, string key)
        {
            string val = appSettings[key];
            if (string.IsNullOrWhiteSpace(val))
            {
                int pos = key.IndexOf(':');
                if (pos > 0)
                {
                    string subKey = key.Substring(pos + 1);
                    val = appSettings["atm:" + subKey];

                    if (string.IsNullOrWhiteSpace(val))
                    {
                        val = appSettings["wp:" + subKey];
                    }
                }
            }

            return val;
        }
    }
}
