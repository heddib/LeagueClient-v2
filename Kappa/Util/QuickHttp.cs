using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MFroehlich.Parsing;
using MFroehlich.Parsing.JSON;

namespace Kappa.Util {
    public class QuickHttp {
        private HttpWebRequest req;

        private QuickHttp(string url, string method) {
            req = WebRequest.CreateHttp(url);
            req.Method = method;
        }

        public QuickHttp Send(string data) => Send(data.GetBytes());
        public QuickHttp Send(byte[] data) {
            using (var stream = req.GetRequestStream())
                stream.Write(data);
            return this;
        }

        public async Task<byte[]> Bytes() {
            WebResponse res;

            try {
                res = await req.GetResponseAsync();
            } catch (WebException x) {
                res = x.Response;
            }

            if (res == null) return new byte[0];

            using (res)
            using (var stream = res.GetResponseStream()) {
                return stream.ReadFully();
            }
        }

        public async Task<string> String() => Encoding.UTF8.GetString(await Bytes());
        public async Task<JSONArray> JSONArray() => JSONParser.ParseArray(await Bytes());
        public async Task<JSONObject> JSONObject() => JSONParser.ParseObject(await Bytes());

        public static QuickHttp Request(string method, string url) => new QuickHttp(url, method);
        public static QuickHttp Request(string method, Uri url) => new QuickHttp(url.AbsoluteUri, method);

        public static Uri Uri(string host, string path, bool https = false) {
            string prefix = https ? "https://" : "http://";
            if (path.StartsWith("/")) path = "/" + path;
            return new Uri(prefix + host + path);
        }
    }
}
