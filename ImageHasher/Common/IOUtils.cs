namespace ImageHasher.Common
{
    using System;
    using System.Text.RegularExpressions;

    public static class IOUtils
    {
        public static string MapServerPath(string virtualPath)
        {
            string result = "";

            if (!string.IsNullOrEmpty(virtualPath))
            {
                string baseDir = Regex.Replace(
                    AppDomain.CurrentDomain.BaseDirectory, "bin.*", (m) => "");

                result = virtualPath.Replace("/", "\\").Replace("~\\", baseDir);
            }

            return result;
        }
    }
}
