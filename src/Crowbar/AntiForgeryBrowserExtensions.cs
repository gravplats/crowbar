using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserContext"/> type.
    /// </summary>
    public static class AntiForgeryBrowserExtensions
    {
        /// <summary>
        /// Adds an anti-forgery request token to the request.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        /// <param name="username">The username.</param>
        /// <param name="salt">The salt string.</param>
        /// <param name="domain">The domain of the web application that the request is submitted from.</param>
        /// <param name="path">The virtual root path of the web application that the request is submitted from.</param>
        /// <param name="applicationPath">The application path.</param>
        public static void AntiForgeryRequestToken(this BrowserContext context, string username = "", string salt = "", string domain = null, string path = null, string applicationPath = "/")
        {
            // From AntiForgeryWorker.GetAntiForgeryTokenAndSetCookie() in the ASP.NET MVC framework.
            var serializer = new AntiForgeryDataSerializer();

            var cookieToken = AntiForgeryData.NewToken();
            string cookieValue = serializer.Serialize(cookieToken);

            string cookieName = AntiForgeryData.GetAntiForgeryTokenName(applicationPath);
            var cookie = new HttpCookie(cookieName, cookieValue) { Domain = null, HttpOnly = true };

            if (!string.IsNullOrEmpty(path))
            {
                cookie.Path = path;
            }

            context.Cookie(cookie);

            var formToken = new AntiForgeryData(cookieToken)
            {
                Salt = salt,
                Username = username
            };

            string formValue = serializer.Serialize(formToken);
            context.FormValue(AntiForgeryData.AntiForgeryTokenFieldName, formValue);
        }

        /// <summary>
        /// From ASP.NET MVC framework.
        /// </summary>
        internal class AntiForgeryDataSerializer
        {
            public virtual string Serialize(AntiForgeryData token)
            {
                if (token == null)
                {
                    throw new ArgumentNullException("token");
                }

                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(token.Salt);
                    writer.Write(token.Value);
                    writer.Write(token.CreationDate.Ticks);
                    writer.Write(token.Username);

                    return Encoder(stream.ToArray());
                }
            }

            internal Func<byte[], string> Encoder =
                bytes => HexToBase64(MachineKey.Encode(bytes, MachineKeyProtection.All).ToUpperInvariant());

            private static int HexValue(char digit)
            {
                return digit > '9' ? digit - '7' : digit - '0';
            }

            private static string HexToBase64(string hex)
            {
                int size = hex.Length / 2;
                byte[] bytes = new byte[size];

                for (int idx = 0; idx < size; idx++)
                {
                    bytes[idx] = (byte)((HexValue(hex[idx * 2]) << 4) + HexValue(hex[idx * 2 + 1]));
                }

                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// From ASP.NET MVC framework.
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
}