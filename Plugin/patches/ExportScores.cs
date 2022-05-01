using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json;

namespace NertsPlus
{
    [HarmonyPatch]
    public class ExportScores
    {
        private static Time lastExport = Time.MinValue;

        private class PlayerScoreJson
        {
            public String name;
            public sbyte[] rounds;
            public int total;

            public PlayerScoreJson(String name, sbyte[] rounds, int total)
            {
                this.name = name;
                this.rounds = rounds;
                this.total = total;
            }
        }

        private static PlayerMessage[] GetPlayerMessages(CompletionScreen completionScreen)
        {
            return new Traverse(completionScreen).Field("playerMessage_0").GetValue<PlayerMessage[]>();
        }

        private static String GetName(NetID id)
        {
            // Reflection to avoid bundling Steamworks library
            Assembly steam = Assembly.LoadFile("Steamworks.NET.dll");

            Type steamIDType = steam.GetType("Steamworks.CSteamID");
            Object steamID = Activator.CreateInstance(steamIDType, id.ID);

            Type steamFriends = steam.GetType("Steamworks.SteamFriends");
            return (String)steamFriends.GetMethod("GetFriendPersonaName", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { steamID });
        }

        [HarmonyPatch(typeof(CompletionScreen), "Update")]
        [HarmonyPrefix]
        static bool Update(CompletionScreen __instance)
        {
            float rounds = GetPlayerMessages(__instance)[0].sbyte_0.Length;
            float width = 1310f + (rounds - 1) * 120;

            Vector2 bottomRight = Utils.GetScreenBounds().Center - new Vector2((width / 2), 0);

            Bounds2 csvBounds = Utils.NewTextDrawingOptions()
                .WithPosition(new Vector2(bottomRight.float_0, 500))
                .WithColor(Utils.White().WithAlpha(0.7f))
                .WithText("CSV")
                .DrawAndMeasure();

            if (csvBounds.Contains(Utils.GetCursorPosition()) && Utils.IsMouseDown())
            {
                String desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                StreamWriter scoresFile = new(Path.Combine(desktopPath, $"nerts_scores_{Time.Now().Ticks}.csv"), false);

                foreach (PlayerMessage message in GetPlayerMessages(__instance))
                {
                    String escapedName = GetName(message.netID_0).Replace("\"", "\"\"");
                    scoresFile.Write($"\"{escapedName}\"");

                    foreach (sbyte points in message.sbyte_0)
                    {
                        scoresFile.Write("," + points);
                    }

                    scoresFile.WriteLine();
                }

                scoresFile.Close();

                lastExport = Time.Now();
            }

            Bounds2 jsonBounds = Utils.NewTextDrawingOptions()
                .WithPosition(new Vector2(bottomRight.float_0, 500) + new Vector2(csvBounds.Width + 30, 0))
                .WithColor(Utils.White().WithAlpha(0.7f))
                .WithText("JSON")
                .DrawAndMeasure();

            if (jsonBounds.Contains(Utils.GetCursorPosition()) && Utils.IsMouseDown())
            {
                String desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                List<PlayerScoreJson> results = new();

                foreach (PlayerMessage message in GetPlayerMessages(__instance))
                {
                    results.Add(new PlayerScoreJson(
                        GetName(message.netID_0),
                        message.sbyte_0,
                        message.short_2
                    ));
                }

                StreamWriter file = new(Path.Combine(desktopPath, $"nerts_scores_{Time.Now().Ticks}.json"), false);
                file.WriteLine(JsonConvert.SerializeObject(results));
                file.Close();

                lastExport = Time.Now();
            }

            if (Time.Now() - lastExport < Utils.DurationSeconds(1))
            {
                Utils.NewTextDrawingOptions()
                    .WithPosition(new Vector2(20, 20))
                    .WithColor(Utils.White().WithAlpha(0.7f))
                    .WithText("Exported to desktop.")
                    .Draw();
            }

            return true;
        }

        [HarmonyPatch(typeof(GClass109), "smethod_29")]
        [HarmonyPrefix]
        static bool IsMouseDown(CompletionScreen __instance)
        {
            // Terrible workaround to avoid the completion screen seeing
            // the mouse is down out of bounds and dismissing the window
            if (Time.Now() - lastExport < Utils.DurationSeconds(1))
            {
                // This isn't actually returning false, it just tells Harmony
                // to skip the real implementation and the default value (also
                // false) is returned
                return false;
            }

            // Run the real implementation
            return true;
        }
    }
}
