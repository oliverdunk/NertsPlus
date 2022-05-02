using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace NertsPlus
{
    [HarmonyPatch]
    public class NertsPlus
    {
        private static string versionString;
        private static readonly bool SHOULD_REGENERATE_TEXTURES = false;
        private static List<String> LOADING_MESSAGES = new List<String>
        {
            "Shuffling cards",
            "Loading mods",
            "Starting flux capacitor",
            "Reversing polarity",
            "Announcing BGP routes",
            "Fixing chameleon circuit",
            "Rewriting Rust in JS",
            "Setting password to hunter2",
        };

        public enum LobbyType
        {
            PUBLIC,
            OWNER_PUBLIC,
            OWNER_PRIVATE
        }

        // Keep track of if we are the lobby owner (and therefore if NertsPlus
        // should be enabled)
        public static LobbyType CurrentLobbyType = LobbyType.PUBLIC;

        public void Load()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            versionString = $"{version.Major}.{version.Minor}.{version.Build}";

            Harmony harmony = new Harmony("NertsPlus");
            harmony.PatchAll();

            if (SHOULD_REGENERATE_TEXTURES)
            {
                typeof(GClass7).GetField("bool_8", BindingFlags.Static | BindingFlags.Public).SetValue(null, true);
            }

            Console.WriteLine($"NertsPlus {versionString} is loaded! 🎉");
        }

        [HarmonyPatch(typeof(TitleScreen), "Update")]
        [HarmonyPostfix]
        static void TitleScreenUpdate()
        {
            DrawModVersion();
        }

        private static void DrawModVersion()
        {
            Utils.NewTextDrawingOptions()
                   .WithText($"Running NertsPlus {versionString}")
                   .WithColor(Utils.White().WithAlpha(0.7f))
                   .WithPosition(new Vector2(20, 20))
                   .Draw();
        }

        [HarmonyPatch(typeof(GClass81), "OnActivate")]
        [HarmonyPrefix]
        static bool LobbyScreenConstructor(GClass81 __instance)
        {
            if (new Traverse(__instance).Field("bool_0").GetValue<bool>())
            {
                CurrentLobbyType = LobbyType.OWNER_PRIVATE;
            }
            else
            {
                CurrentLobbyType = LobbyType.PUBLIC;
            }

            return true;
        }

        [HarmonyPatch(typeof(Steam), "smethod_10")]
        [HarmonyPrefix]
        static bool SetLobbyType(uint elobbyType_0)
        {
            CurrentLobbyType = elobbyType_0 == 0 || elobbyType_0 == 1
                ? LobbyType.OWNER_PRIVATE
                : LobbyType.OWNER_PUBLIC;

            return true;
        }

        [HarmonyPatch(typeof(GameScreen), "Update")]
        [HarmonyPostfix]
        static void GameScreenUpdate(GameScreen __instance)
        {
            NertsPlusGameScreen gameScreen = new NertsPlusGameScreen(__instance);

            if (!gameScreen.IsConnected() || CurrentLobbyType != LobbyType.OWNER_PRIVATE)
            {
                return;
            }

            Texture texture = Utils.LoadTexture("logo_button");
            Texture texture_hover = Utils.LoadTexture("logo_button_hover");
            Vector2 position = GClass109.smethod_3().TopRight + new Vector2(-220f, -93);
            Bounds2 bounds = Bounds2.WithSize(position, texture.vector2_0);
            bool hover = bounds.Contains(Utils.GetCursorPosition());
            GClass91.smethod_24(hover ? texture_hover : texture, Color.White.WithAlpha(0.6f), position);

            if (hover && Utils.IsMouseDown())
            {
                NertsPlusScreenStack.Push(new ConfigScreen(gameScreen), true);
            }
        }

        [HarmonyPatch(typeof(GClass5), "method_1")]
        [HarmonyPostfix]
        static void UpdateLoadingScreen()
        {
            DrawModVersion();
        }

        [HarmonyPatch(typeof(GClass5), "method_1")]
        [HarmonyTranspiler]
        static void UpdateLoadingScreen(IEnumerable<CodeInstruction> instructions)
        {
            String LoadingMessage = LOADING_MESSAGES[new Random().Next() % LOADING_MESSAGES.Count];

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr)
                {
                    instruction.operand = LoadingMessage.ToUpper() + "...";
                }
            }
        }

    }
}
