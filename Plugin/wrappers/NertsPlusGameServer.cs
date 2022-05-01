using System.Collections.Generic;
using HarmonyLib;

namespace NertsPlus
{
    public class NertsPlusGameServer
    {
        private GameServer server;

        public NertsPlusGameServer(GameServer server)
        {
            this.server = server;
        }

        public NertsPlusGameState GetState()
        {
            return (NertsPlusGameState) new Traverse(server).Field("genum118_0").GetValue<GEnum118>();
        }

        public void SetState(NertsPlusGameState state)
        {
            new Traverse(server).Field("genum118_0").SetValue((GEnum118)state);
        }

        public GStruct12 GetTimeElapsed()
        {
            return Time.Now() - new Traverse(server).Field("time_0").GetValue<Time>();
        }

        public List<Player> GetPlayers()
        {
            return new Traverse(server).Field("list_0").GetValue<List<Player>>();
        }

        public void ForceShuffle()
        {
            // Unfortunately there's no "shuffle" function and this logic is
            // inlined with the timer handling code. To force a shuffle,
            // decrement the internal counter of shuffles performed (so this
            // forced shuffle doesn't move us closer to game end) and then
            // advance the timer.
            Traverse shuffleCount = Traverse.Create(this.server).Field("int_1");
            shuffleCount.SetValue(shuffleCount.GetValue<int>() - 1);

            Traverse startTime = Traverse.Create(this.server).Field("time_2");
            startTime.SetValue(startTime.GetValue<Time>() - GStruct12.smethod_0(200));
        }

        public void EndRound()
        {
            // See ForceShuffle implementation above
            Traverse shuffleCount = Traverse.Create(this.server).Field("int_1");
            shuffleCount.SetValue(2);

            Traverse startTime = Traverse.Create(this.server).Field("time_2");
            startTime.SetValue(startTime.GetValue<Time>() + GStruct12.smethod_0(600));
        }
    }
}
