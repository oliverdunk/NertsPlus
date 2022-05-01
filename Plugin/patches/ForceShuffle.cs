using System;
using HarmonyLib;

namespace NertsPlus.patches
{
    [HarmonyPatch]
    public class ForceShuffle
    {
        [HarmonyPatch(typeof(GameServer), "method_4")]
        [HarmonyPrefix]
        static bool GameServerTick(GameServer __instance)
        {
            // Avoid changing anything in public games
            if (NertsPlus.CurrentLobbyType != NertsPlus.LobbyType.OWNER_PRIVATE)
            {
                return true;
            }

            NertsPlusGameServer server = new NertsPlusGameServer(__instance);

            // Force shuffle when Enter key is pressed
            if (Utils.IsKeyPressed(Key.SEMICOLON))
            {
                Console.WriteLine("Shuffling!");
                server.ForceShuffle();
            }

            return true;
        }
    }
}
