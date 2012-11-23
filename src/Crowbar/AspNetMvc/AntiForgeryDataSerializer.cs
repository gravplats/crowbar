using System;
using System.IO;
using System.Web.Security;

namespace Crowbar.AspNetMvc
{
    /// <summary>
    /// Taken from the ASP.NET MVC 3 source code (just the bare essentials of the class).
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
}