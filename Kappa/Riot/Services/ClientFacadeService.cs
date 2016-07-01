using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class ClientFacadeService : Service {
        protected override string Destination => "clientFacadeService";

        public ClientFacadeService(Session session) : base(session) { }

        /// <summary>
        /// Gets the login packet for the user with all the information for the user.
        /// </summary>
        /// <returns>Returns the login data packet</returns>
        public Task<LoginDataPacket> GetLoginDataPacketForUser() {
            return InvokeAsync<LoginDataPacket>("getLoginDataPacketForUser");
        }

        /// <summary>
        /// Report a player
        /// </summary>
        /// <param name="jsonInformation"></param>
        /// <returns>Json Data about kudos</returns>
        public Task<LcdsResponseString> CallKudos(string jsonInformation) {
            return InvokeAsync<LcdsResponseString>("reportPlayer", jsonInformation);
        }
    }
}
