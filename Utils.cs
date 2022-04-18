using System.Reflection;
using HarmonyLib;

namespace NertsPlus
{
    public class Utils
    {
        public static void ForceShuffle(GameServer server)
        {
            // Unfortunately there's no "shuffle" function and this logic is
            // inlined with the timer handling code. To force a shuffle,
            // decrement the internal counter of shuffles performed (so this
            // forced shuffle doesn't move us closer to game end) and then
            // advance the timer.
            Traverse shuffleCount = Traverse.Create(server).Field("int_1");
            shuffleCount.SetValue(shuffleCount.GetValue<int>() - 1);

            Traverse startTime = Traverse.Create(server).Field("time_2");
            startTime.SetValue(startTime.GetValue<Time>() - GStruct12.smethod_0(200));
        }

        public static void PlaySoundLocal(GameScreen screen, GEnum0 sound)
        {
            MethodInfo PlaySound = typeof(GameScreen).GetMethod("method_8", BindingFlags.NonPublic | BindingFlags.Instance);
            PlaySound.Invoke(screen, new object[] { sound });
        }

        public static TextDrawingOptions NewTextDrawingOptions()
        {
            TextDrawingOptions result = default(TextDrawingOptions);
            result.LineSpacing = 1f;
            result.DoubleNewlineSpacing = 0.6f;
            result.WrapWidth = float.MaxValue;
            result.TruncateWidth = float.MaxValue;
            result.RenderCharCount = int.MaxValue;
            result.EnableMeasuring = false;
            result.EnableDrawing = true;
            result.Font = GClass62.smethod_1().gclass12_7;
            return result;
        }

        public static void DrawBannerText(string text)
        {
            NewTextDrawingOptions().WithText(text.ToUpper())
                .WithPosition(new Vector2(1920f, 2206))
                .WithColor(GClass50.color_2.WithAlpha(0.7f))
                .WithTransform(GClass50.matrix4_0 * (Matrix4.smethod_1(new Vector2(0f, 23f))))
                .AlignedCenter()
                .Draw();
        }
    }
}
