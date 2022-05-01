using System;
using HarmonyLib;

namespace NertsPlus.patches
{
    [HarmonyPatch]
    public class ChangeStartMusic
    {
        public enum StartMusic
        {
            Normal,
            Countdown,
            None
        };

        private const GEnum0 SOUND_COUNTDOWN = (GEnum0) 0x80000000;
        public static StartMusic START_MUSIC = StartMusic.Countdown;

        [HarmonyPatch(typeof(ServerMessageBuilder), "method_8")]
        [HarmonyPrefix]
        static bool ServerMessageBuilderPlaySound(ref GEnum0 genum0_1)
        {
            // Avoid changing anything in public games
            if (NertsPlus.CurrentLobbyType != NertsPlus.LobbyType.OWNER_PRIVATE)
            {
                return true;
            }

            if (genum0_1 == GEnum0.Start)
            {
                if (START_MUSIC == StartMusic.Countdown)
                {
                    // Change to our custom Countdown sound
                    genum0_1 = SOUND_COUNTDOWN;
                    return true;
                }
                else if (START_MUSIC == StartMusic.None)
                {
                    // Don't send any sound at all
                    return false;
                }
            }

            // For all other sounds, run the real implementation as normal
            return true;
        }

        [HarmonyPatch(typeof(GameScreen), "method_8")]
        [HarmonyPrefix]
        static bool GameScreenPlaySound(GEnum0 genum0_0)
        {
            // Custom sounds
            if (genum0_0 == SOUND_COUNTDOWN)
            {
                GClass94.smethod_0("sounds/fx/vo_321go").smethod_2(false, 0);
                return false;
            }

            // For all other sounds, run the real implementation as normal
            return true;
        }

        [HarmonyPatch(typeof(GameServer), "method_4")]
        [HarmonyPrefix]
        static bool GameServerTick(GameServer __instance)
        {
            NertsPlusGameServer server = new NertsPlusGameServer(__instance);

            // If we're in the intro game state...
            if (server.GetState() == NertsPlusGameState.Intro && START_MUSIC != StartMusic.Normal)
            {
                // Skip to the game if the intro music is disabled or our custom sound has finished
                if (START_MUSIC == StartMusic.None || server.GetTimeElapsed() > Utils.DurationSeconds(2.3))
                {
                    server.SetState(NertsPlusGameState.InProgress);
                }
            }

            return true;
        }
    }
}
