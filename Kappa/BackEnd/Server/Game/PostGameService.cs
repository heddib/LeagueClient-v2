using System;
using System.Linq;
using Kappa.BackEnd.Server.Chat;
using Kappa.BackEnd.Server.Collection;
using Kappa.BackEnd.Server.Collection.Model;
using Kappa.BackEnd.Server.Game.Model;
using Kappa.Riot.Domain;
using Kappa.Riot.Domain.JSON;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Game {
    public class PostGameService : JSONService {
        private Session session;
        private ChatRoomService rooms;

        private PostGameState state;

        [Async("/state")]
        public event EventHandler<PostGameState> State;

        public PostGameService(ChatRoomService rooms, Session session) : base("/playloop/postgame") {
            this.rooms = rooms;
            this.session = session;

            var messages = new MessageConsumer(session);

            messages.Consume<EndOfGameStats>(OnEndOfGameStats);
            messages.Consume<SimpleDialogMessage>(OnSimpleDialogMessage);
            messages.Consume<PlayerCredentialsDto>(OnPlayerCredentials);

            Reset();
        }

        private bool OnPlayerCredentials(PlayerCredentialsDto creds) {
            Reset();
            return false;
        }

        private bool OnSimpleDialogMessage(SimpleDialogMessage msg) {
            switch (msg.TitleCode) {
            case "championMastery":
                var arg = JSONParser.ParseObject((string) msg.Params[0]);
                var rawMastery = JSONDeserializer.Deserialize<EogChampionMasteryDTO>(arg);

                if (rawMastery.LevelUpList.Any()) { }

                var stats = new PostGameChampionMastery {
                    Champion = rawMastery.ChampionId,
                    Grade = rawMastery.PlayerGrade,
                    Before = new ChampionMasteryState {
                        Level = rawMastery.ChampionLevelUp ? rawMastery.ChampionLevel - 1 : rawMastery.ChampionLevel,
                        PointsInLevel = rawMastery.ChampionPointsSinceLastLevelBeforeGame + rawMastery.ChampionPointsUntilNextLevelBeforeGame,
                        PointsSinceLevel = rawMastery.ChampionPointsSinceLastLevelBeforeGame,
                        TotalPoints = rawMastery.ChampionPointsBeforeGame
                    },
                    After = new ChampionMasteryState {
                        Level = rawMastery.ChampionLevel,
                        PointsInLevel = rawMastery.ChampionPointsUntilNextLevelAfterGame + (rawMastery.ChampionPointsGained - rawMastery.ChampionPointsUntilNextLevelBeforeGame) + 1,
                        PointsSinceLevel = rawMastery.ChampionPointsGained - rawMastery.ChampionPointsUntilNextLevelBeforeGame + 1,
                        TotalPoints = rawMastery.ChampionPointsBeforeGame + rawMastery.ChampionPointsGained
                    }
                };
                if (!rawMastery.ChampionLevelUp) {
                    stats.After.PointsInLevel = stats.Before.PointsInLevel;
                    stats.After.PointsSinceLevel = rawMastery.ChampionPointsSinceLastLevelBeforeGame + rawMastery.ChampionPointsGained;
                }

                state.ChampionMastery = stats;
                OnStateChanged();
                return true;

            case "championMasteryLootGrant":
                arg = JSONParser.ParseObject((string) msg.Params[0]);
                var rawLoot = JSONDeserializer.Deserialize<ChampionMasteryLootGrant>(arg);

                HextechService.Add(state.Hextech, true, rawLoot.LootName, 1, null);
                OnStateChanged();
                break;
            }

            return false;
        }

        private bool OnEndOfGameStats(EndOfGameStats stats) {
            this.state.IpEarned = stats.IpEarned;
            this.state.IpTotal = session.Me.IP;
            this.state.IpLifetime = stats.IpTotal;
            this.state.Chatroom = rooms.JoinPostGame(stats);
            OnStateChanged();

            return true;
        }

        private void Reset() {
            if (state != null && state.Chatroom != Guid.Empty)
                rooms.LeaveRoom(state.Chatroom);

            state = new PostGameState {
                Hextech = new HextechInventory()
            };
        }

        [Endpoint("/leave")]
        public void Leave() {
            Reset();
        }

        private void OnStateChanged() {
            State?.Invoke(this, state);
        }
    }
}
