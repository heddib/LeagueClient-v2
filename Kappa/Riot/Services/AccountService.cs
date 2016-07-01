using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class AccountService : Service {
        protected override string Destination => "accountService";

        public AccountService(Session session) : base(session) { }

        /// <summary>
        /// Gets the state for the current account
        /// </summary>
        /// <returns>Return the accounts state</returns>
        public Task<string> GetAccountState() {
            return InvokeAsync<string>("getAccountStateForCurrentSession");
        }
    }
}
