using HarmonyLib;

namespace NertsPlus
{
    public class NertsPlusGameScreen
    {
        private GameScreen gameScreen;

        public NertsPlusGameScreen(GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }

        public NertsPlusGameServer GetGameServer()
        {
            GStruct0<GameServer> maybe = new Traverse(gameScreen)
                .Field("gstruct0_0")
                .GetValue<GStruct0<GameServer>>();

            return maybe.method_0()
                ? new NertsPlusGameServer(maybe.method_2())
                : null;
        }

        public bool IsConnected()
        {
            return gameScreen.method_3().method_0();
        }
    }
}
