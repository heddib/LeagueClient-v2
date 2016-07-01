using System;
using RtmpSharp.IO;

namespace Kappa.Riot.Domain {
  [Serializable]
  [SerializedName("com.riotgames.platform.client.dynamic.configuration.ClientDynamicConfigurationNotification")]
  public class ClientDynamicConfigurationNotification {
    [SerializedName("configs")]
    public string Configs { get; set; }

    [SerializedName("delta")]
    public bool Delta { get; set; }
  }
}
