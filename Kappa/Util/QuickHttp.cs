using System;
using System.Net;
using System.Text;
using MFroehlich.Parsing;
using MFroehlich.Parsing.JSON;

namespace Kappa.Util {
    public class QuickHttp {
        public HttpStatusCode StatusCode { get; }
        public byte[] Content { get; }

        public QuickHttp(HttpWebResponse res) {
            using (res) {
                StatusCode = res.StatusCode;
                using (var stream = res.GetResponseStream())
                    Content = stream.ReadFully();
            }
        }

        public string AsString() => Encoding.UTF8.GetString(Content);
        public JSONObject AsJSON() => JSONParser.ParseObject(Content);

        public static QuickHttp Get(string url) => Get(new Uri(url));
        public static QuickHttp Get(Uri uri) {
            var req = WebRequest.CreateHttp(uri);
            return new QuickHttp((HttpWebResponse) req.GetResponse());
        }
    }
}
