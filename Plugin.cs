using System;
using BepInEx;
using BepInEx.NetLauncher;
using HarmonyLib;

namespace NertsPlus
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        private static readonly bool SKIP_START_MUSIC = true;
        private static readonly bool ALLOW_FORCE_SHUFFLE = true;

        public override void Load()
        {
            Harmony.CreateAndPatchAll(typeof(Plugin));

            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPatch(typeof(ServerMessageBuilder), "method_8")]
        [HarmonyPrefix]
        static bool ServerMessageBuilderPlaySound(GEnum0 genum0_1)
        {
            // Ignore any requests to send the start music as part of a
            // server message
            if (genum0_1 == GEnum0.Start && SKIP_START_MUSIC)
            {
                return false;
            }

            // For all other sounds, run the real implementation as normal
            return true;
        }


        [HarmonyPatch(typeof(TitleScreen), "Update")]
        [HarmonyPostfix]
        static void TitleScreenUpdate()
        {
            // Draw watermark
            Utils.NewTextDrawingOptions()
                   .WithText($"Running NertsPlus {PluginInfo.PLUGIN_VERSION}")
                   .WithColor(GClass50.color_2.WithAlpha(0.7f))
                   .WithPosition(new Vector2(20, 20))
                   .Draw();
        }

        [HarmonyPatch(typeof(GameScreen), "Update")]
        [HarmonyPostfix]
        static void GameScreenUpdate()
        {

        }

        [HarmonyPatch(typeof(GameServer), "method_4")]
        [HarmonyPrefix]
        static bool GameServerTick(GameServer __instance)
        {
            GEnum118 state = new Traverse(__instance).Field("genum118_0").GetValue<GEnum118>();

            // Skip game state 1 (intro music) if SKIP_START_MUSIC is set
            if (state == (GEnum118) 1 && SKIP_START_MUSIC)
            {
                new Traverse(__instance).Field("genum118_0").SetValue(2);
            }

            // Force shuffle when Enter key is pressed
            if (GClass109.smethod_14(Key.ANY_ENTER) && ALLOW_FORCE_SHUFFLE)
            {
                Console.WriteLine("Shuffling!");
                Utils.ForceShuffle(__instance);
            }

            return true;
        }
    }
}
