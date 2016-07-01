using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON {
  public class InvitationMetaData : JSONSerializable {
    [JSONField("mapId")]
    public int MapId { get; set; }

    [JSONField("gameMode")]
    public string GameMode { get; set; }

    [JSONField("gameMutators")]
    public string[] GameMutators { get; set; }

    [JSONField("gameType")]
    public string GameType { get; set; }

    [JSONField("rankedTeamName")]
    public string RankedTeamName { get; set; }

    [JSONField("rankedTeamId")]
    public string RankedTeamId { get; set; }

    [JSONField("queueId")]
    public int QueueId { get; set; }

    [JSONField("isRanked")]
    public bool IsRanked { get; set; }

    [JSONField("botDifficulty")]
    public string BotDifficulty { get; set; }

    [JSONField("gameTypeConfigId")]
    public int GameTypeConfigId { get; set; }

    [JSONField("groupFinderId")]
    public string GroupFinderId { get; set; }

    [JSONField("gameId")]
    public long GameId { get; set; }
  }
}
