using System.Collections.Generic;
using MFroehlich.Parsing.JSON;

namespace Kappa.Riot.Domain.JSON {
  public class LoginQueueDto : JSONSerializable {
    [JSONField("rate")]
    public int Rate { get; set; }

    [JSONField("token")]
    public string Token { get; set; }

    [JSONField("reason")]
    public string Reason { get; set; }

    [JSONField("delay")]
    public int Delay { get; set; }

    [JSONField("inGameCredentials")]
    public InGameCredentials InGameCredentials { get; set; }

    [JSONField("user")]
    public string User { get; set; }

    [JSONField("idToken")]
    public string IdToken { get; set; }

    [JSONField("vcap")]
    public int VCap { get; set; }

    [JSONField("status")]
    public string Status { get; set; }

    [JSONField("gasToken")]
    public JSONObject GasToken { get; set; }

    [JSONField("node")]
    public int Node { get; set; }

    [JSONField("tickers")]
    public List<JSONObject> Tickers { get; set; }

    [JSONField("backlog")]
    public int Backlog { get; set; }

    [JSONField("champ")]
    public int Champ { get; set; }
  }

  public class InGameCredentials : JSONSerializable {
    [JSONField("inGame")]
    public bool InGame { get; set; }

    [JSONField("summonerId")]
    public long SummonerId { get; set; }

    [JSONField("serverIp")]
    public string ServerIp { get; set; }

    [JSONField("serverPort")]
    public int ServerPort { get; set; }

    [JSONField("encryptionKey")]
    public string EncryptionKey { get; set; }

    [JSONField("handshakeToken")]
    public string HandshakeToken { get; set; }
  }
}
