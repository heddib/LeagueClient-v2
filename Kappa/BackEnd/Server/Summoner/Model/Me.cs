using System.Collections.Generic;
using Kappa.Riot.Domain;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Summoner.Model {
    public class Me : JSONValuable {
        private LoginDataPacket Packet { get; }
        private AccountSummary Account { get; }

        public IEnumerable<GameTypeConfigDTO> GameTypeConfigs => Packet.GameTypeConfigs;

        public int ProfileIcon => Packet.AllSummonerData.Summoner.ProfileIconId;
        public string Name => Packet.AllSummonerData.Summoner.Name;
        public string Username => Account.Username;

        public string InternalName => Packet.AllSummonerData.Summoner.InternalName;
        public long SummonerId => Packet.AllSummonerData.Summoner.SummonerId;
        public long AccountId => Packet.AllSummonerData.Summoner.AccountId;

        public MasteryBookDTO Masteries => Packet.AllSummonerData.MasteryBook;
        public SpellBookDTO Runes => Packet.AllSummonerData.SpellBook;
        public int Level => Packet.AllSummonerData.SummonerLevel.Level;

        public Me(LoginDataPacket packet, AccountSummary account) {
            this.Account = account;
            this.Packet = packet;
        }

        JSONValue JSONValuable.ToJSON() {
            return new JSONObject {
                ["name"] = Name,
                ["icon"] = ProfileIcon,
                ["ip"] = Packet.IpBalance,
                ["rp"] = Packet.RpBalance,
                ["summonerId"] = SummonerId,
                ["accountId"] = AccountId
            };
        }

        internal void OnBalance(StoreAccountBalanceNotification balance) {
            Packet.IpBalance = balance.IP;
            Packet.RpBalance = balance.RP;
        }
    }
}
