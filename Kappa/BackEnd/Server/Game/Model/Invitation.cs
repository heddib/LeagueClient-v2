using Kappa.Riot.Domain;
using Kappa.Riot.Domain.JSON;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game.Model {
    public class Invitation : JSONValuable {
        public string Id => Base.InvitationId;
        public InvitationRequest Base { get; }
        public InvitationMetaData MetaData { get; }

        public Invitation(InvitationRequest invite) {
            Base = invite;
            MetaData = JSONDeserializer.Deserialize<InvitationMetaData>(JSONParser.ParseObject(invite.GameMetaData));
        }

        JSONValue JSONValuable.ToJSON() {
            return new JSONObject {
                ["state"] = Base.InvitationState,
                ["type"] = Base.InviteType,
                ["id"] = Id,

                ["from"] = new JSONObject {
                    ["name"] = Base.Owner.SummonerName,
                    ["id"] = Base.Owner.SummonerId,
                },

                ["game"] = new JSONObject {
                    ["map"] = MetaData.MapId,
                    ["type"] = MetaData.GameType,
                    ["mode"] = MetaData.GameMode,
                    ["queue"] = MetaData.QueueId,
                    ["config"] = MetaData.GameTypeConfigId,
                },
            };
        }
    }
}
