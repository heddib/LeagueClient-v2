using RtmpSharp.IO;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal interface IRequirable { }

    internal abstract class Service : IRequirable {
        protected abstract string Destination { get; }
        private Session session;

        protected Service(Session rtmp) {
            this.session = rtmp;
        }

        protected async Task<T> InvokeAsync<T>(string method, params object[] args) {
            var ret = await session.Invoke<T>(Destination, method, args);
            Session.RtmpLogInvoke(Destination, method, args, ret);
            return ret;
        }

        public static SerializationContext RegisterObjects() {
            var context = new SerializationContext();
            var ass = Assembly.GetExecutingAssembly();

            var x = from type in ass.GetTypes()
                    where type.GetCustomAttributes<SerializableAttribute>().Any()
                    where type.GetCustomAttributes<SerializedNameAttribute>().Any()
                    select type;

            foreach (var type in x)
                context.Register(type);

            return context;
        }
    }
}
