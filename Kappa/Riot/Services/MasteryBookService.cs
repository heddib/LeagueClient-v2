using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
    internal class MasteryBookService : Service {
        protected override string Destination => "masteryBookService";

        public MasteryBookService(Session session) : base(session) { }

        /// <summary>
        /// Saves the mastery book
        /// </summary>
        /// <param name="masteryBookPage">The mastery book information</param>
        /// <returns>Returns the mastery book</returns>
        public Task<MasteryBookDTO> SaveMasteryBook(MasteryBookDTO masteryBookPage) {
            return InvokeAsync<MasteryBookDTO>("saveMasteryBook", masteryBookPage);
        }
        /// <summary>
        /// Saves the mastery book
        /// </summary>
        /// <param name="summonerId">The Summoner id</param>
        /// <returns>Returns the mastery book</returns>
        public Task<MasteryBookDTO> GetMasteryBook(long summonerId) {
            return InvokeAsync<MasteryBookDTO>("getMasteryBook", summonerId);
        }

        /// <summary>
        /// Selects the default mastery book
        /// </summary>
        /// <param name="page">The mastery book to select</param>
        /// <returns></returns>
        public Task<object> SelectDefaultMasteryBookPage(MasteryBookPageDTO page) {
            return InvokeAsync<object>("selectDefaultMasteryBookPage", page);
        }
    }
}
