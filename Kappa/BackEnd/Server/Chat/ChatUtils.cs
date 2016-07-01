using agsXMPP;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Kappa.BackEnd.Server.Chat {
    internal class ChatUtils {
        public static long GetSummonerId(string user) {
            string sub = user.Substring(3);
            long l;
            if (!long.TryParse(sub, out l)) throw new Exception("HI");
            return l;
        }

        public static string GetObfuscatedChatroomName(string subject, string type) {
            byte[] data = Encoding.UTF8.GetBytes(subject.ToLower()), result;
            using (var sha = new SHA1CryptoServiceProvider())
                result = sha.ComputeHash(data);
            string obfuscatedName = string.Empty;
            foreach (byte bitHack in result) {
                obfuscatedName = obfuscatedName + Convert.ToString(((uint) (bitHack & 240) >> 4), 16);
                obfuscatedName = obfuscatedName + Convert.ToString(bitHack & 15, 16);
            }
            obfuscatedName = Regex.Replace(obfuscatedName, @"/\s+/gx", string.Empty);
            obfuscatedName = Regex.Replace(obfuscatedName, @"/[^a-zA-Z0-9_~]/gx", string.Empty);
            return type + "~" + obfuscatedName;
        }

        public static Jid GetChatroomJID(string subject, string type, bool isPublic, string pass = null) {
            string obf = GetObfuscatedChatroomName(subject, type);
            string raw;
            if (!isPublic) raw = obf + "@sec.pvp.net";
            else if (pass == null) raw = obf + "@lvl.pvp.net";
            else raw = obf + "@conference.pvp.net";
            return new Jid(raw);
        }
    }
}
