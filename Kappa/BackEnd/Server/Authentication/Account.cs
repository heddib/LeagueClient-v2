using System;
using System.Security.Cryptography;
using System.Text;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Authentication {
    public class Account : JSONSerializable {
        public int ProfileIcon { get; set; }
        public string SummonerName { get; set; }
        public string EncodedPassword { get; set; }

        public string GetPassword() {
            var encoded = Convert.FromBase64String(EncodedPassword);
            var bytes = ProtectedData.Unprotect(encoded, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(bytes);
        }

        public void SetPassword(string value) {
            var bytes = Encoding.UTF8.GetBytes(value);
            var raw = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            EncodedPassword = Convert.ToBase64String(raw);
        }
    }
}
