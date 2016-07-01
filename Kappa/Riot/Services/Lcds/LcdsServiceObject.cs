using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Services.Lcds {
    public abstract class LcdsServiceObject : JSONValuable {
        public string MessageId { get; }
        public string Status { get; }
        public object Payload { get; }

        protected LcdsServiceObject(string messageId, string status, object payload) {
            MessageId = messageId;
            Payload = payload;
            Status = status;
        }

        public static LcdsServiceObject GetObject(LcdsServiceProxyResponse lcds) {
            if (lcds.CompressedPayload) {
                var bytes = Convert.FromBase64String(lcds.Payload);
                using (var src = new MemoryStream(bytes))
                using (var dst = new MemoryStream())
                using (var gzip = new GZipStream(src, CompressionMode.Decompress)) {
                    gzip.CopyTo(dst);
                    lcds.Payload = Encoding.UTF8.GetString(dst.ToArray());
                }
            }
            var arg = lcds.Payload == null ? null : JSONParser.Parse(lcds.Payload);

            var type = Services.SingleOrDefault(t => t.Service == lcds.ServiceName && t.Method == lcds.MethodName);
            if (type == null) {
                if(Debugger.IsAttached) Debugger.Break();
                throw new NotImplementedException($"Service {lcds.ServiceName} method {lcds.MethodName} not implemented");
            }

            return (LcdsServiceObject) Activator.CreateInstance(type.ObjectType, lcds.MessageId, lcds.Status, arg);
        }

        JSONValue JSONValuable.ToJSON() => (JSONValue) Payload;

        private static readonly List<LcdsServiceObjectType> Services = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                                                        where !t.IsNested
                                                                        let method = t.GetCustomAttribute<LcdsServiceAttribute>()
                                                                        where method != null
                                                                        select new LcdsServiceObjectType(method, t)).ToList();

        private class LcdsServiceObjectType {
            public string Service { get; }
            public string Method { get; }
            public Type ObjectType { get; }
            public LcdsServiceObjectType(LcdsServiceAttribute att, Type t) {
                Service = att.Service;
                Method = att.Method;
                ObjectType = t;
            }
        }
    }

    public class LcdsServiceAttribute : Attribute {
        public string Service { get; }
        public string Method { get; }
        public LcdsServiceAttribute(string service, string method) {
            Service = service;
            Method = method;
        }
    }
}
