using RtmpSharp.IO;
using System;

namespace Kappa.Riot.Domain {
    public class RiotException {
        [SerializedName("message")]
        public string Message { get; set; }
        [SerializedName("suppressed")]
        public object[] Suppressed { get; set; }
        [SerializedName("rootCauseClassname")]
        public string RootCauseClassname { get; set; }
        [SerializedName("localizedMessage")]
        public string LocalizedMessage { get; set; }
        [SerializedName("cause")]
        public object Cause { get; set; }
        [SerializedName("substitutionArguments")]
        public object[] SubstitutionArguments { get; set; }
        [SerializedName("errorCode")]
        public string ErrorCode { get; set; }
    }

    [Serializable, SerializedName("com.riotgames.platform.login.impl.ClientVersionMismatchException")]
    public class ClientVersionMismatchException : RiotException { }

    [Serializable, SerializedName("com.riotgames.platform.messaging.UnexpectedServiceException")]
    public class UnexpectedServiceException : RiotException { }

    [Serializable, SerializedName("com.riotgames.platform.game.PlayerAlreadyInGameException")]
    public class PlayerAlreadyInGameException : RiotException { }
}
