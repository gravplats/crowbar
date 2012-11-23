using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Crowbar.AspNetMvc
{
    /// <summary>
    /// Taken from the ASP.NET MVC 3 source code (just the bare essentials of the class).
    /// </summary>
    internal sealed class AntiForgeryData
    {
        public const string AntiForgeryTokenFieldName = "__RequestVerificationToken";

        private const int TokenLength = 128 / 8;
        private readonly static RNGCryptoServiceProvider prng = new RNGCryptoServiceProvider();

        private DateTime creationDate = DateTime.UtcNow;
        private string salt;
        private string username;
        private string value;

        public AntiForgeryData() { }

        public AntiForgeryData(AntiForgeryData token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            CreationDate = token.CreationDate;
            Salt = token.Salt;
            Username = token.Username;
            Value = token.Value;
        }

        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        public string Salt
        {
            get { return salt ?? string.Empty; }
            set { salt = value; }
        }

        public string Username
        {
            get { return username ?? string.Empty; }
            set { username = value; }
        }

        public string Value
        {
            get { return value ?? string.Empty; }
            set { this.value = value; }
        }

        private static string Base64EncodeForCookieName(string s)
        {
            byte[] rawBytes = Encoding.UTF8.GetBytes(s);
            string base64String = Convert.ToBase64String(rawBytes);

            // replace base64-specific characters with characters that are safe for a cookie name
            return base64String.Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }

        private static string GenerateRandomTokenString()
        {
            byte[] tokenBytes = new byte[TokenLength];
            prng.GetBytes(tokenBytes);

            return Convert.ToBase64String(tokenBytes);
        }

        // If the app path is provided, we're generating a cookie name rather than a field name, and the cookie names should
        // be unique so that a development server cookie and an IIS cookie - both running on localhost - don't stomp on each other.
        internal static string GetAntiForgeryTokenName(string appPath)
        {
            if (string.IsNullOrEmpty(appPath))
            {
                return AntiForgeryTokenFieldName;
            }

            return AntiForgeryTokenFieldName + "_" + Base64EncodeForCookieName(appPath);
        }

        internal static string GetUsername(IPrincipal user)
        {
            if (user != null)
            {
                var identity = user.Identity;
                if (identity != null && identity.IsAuthenticated)
                {
                    return identity.Name;
                }
            }

            return string.Empty;
        }

        public static AntiForgeryData NewToken()
        {
            string tokenString = GenerateRandomTokenString();
            return new AntiForgeryData
            {
                Value = tokenString
            };
        }

    }
}