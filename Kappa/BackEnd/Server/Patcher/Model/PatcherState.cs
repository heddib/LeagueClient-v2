using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Patcher.Model {
    public class PatcherState : JSONSerializable {
        [JSONField("phase")]
        public PatcherPhase Phase { get; set; }

        [JSONField("current")]
        public long Current { get; set; }

        [JSONField("total")]
        public long Total { get; set; }
    }

    public enum PatcherPhase {
        NONE,
        PATCHING
    }
}
