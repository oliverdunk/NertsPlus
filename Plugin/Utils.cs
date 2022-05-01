using System;
using System.Reflection;

namespace NertsPlus
{
    public class Utils
    {
        public static void PlaySoundLocal(GameScreen screen, GEnum0 sound)
        {
            MethodInfo PlaySound = typeof(GameScreen).GetMethod("method_8", BindingFlags.NonPublic | BindingFlags.Instance);
            PlaySound.Invoke(screen, new object[] { sound });
        }

        public static Color White()
        {
            return GClass50.color_2;
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

        public static Texture LoadTexture(String path)
        {
            Texture result = TextureLoader.smethod_1(path);
            TextureLoader.smethod_4(result, (GEnum133)0, () => { });
            return result;
        }

        public static void DrawColor(Color color, Bounds2 bounds)
        {
            GClass91.smethod_43(color, bounds);
        }

        public static void DrawTexture(Texture texture, Vector2 position)
        {
            GClass91.smethod_25(texture, position);
        }

        public static bool IsKeyPressed(Key key)
        {
            return GClass109.smethod_14(key);
        }

        public static Vector2 GetCursorPosition()
        {
            return GClass109.smethod_25();
        }

        public static bool IsMouseDown()
        {
            return GClass109.smethod_29((GEnum1)1);
        }

        public static GStruct12 DurationSeconds(double seconds)
        {
            return GStruct12.smethod_0(seconds);
        }

        public static Bounds2 GetScreenBounds()
        {
            return GClass109.smethod_3();
        }
    }
}
