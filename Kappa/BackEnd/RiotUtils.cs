namespace Kappa.BackEnd {
    public static class RiotUtils {
        public static bool TeamIdToIsBlue(int teamId) => teamId == 100;
        public static int IsBlueToTeamId(bool isBlue) => isBlue ? 100 : 200;
    }
}
