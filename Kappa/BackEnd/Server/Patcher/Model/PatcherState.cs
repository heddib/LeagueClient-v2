using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Patcher.Model {
    [JSONSerializable]
    public class PatcherState {
        public PatcherPhase Phase { get; set; }

        public long Current { get; set; }
        public long Total { get; set; }
    }

    public enum PatcherPhase {
        NONE,
        PATCHING
    }
}
