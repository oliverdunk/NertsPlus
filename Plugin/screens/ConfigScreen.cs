using System;
using HarmonyLib;
using NertsPlus.patches;

namespace NertsPlus
{
    public class ConfigScreen : IScreen
    {
        private static Traverse optionsScreenTraversal
            = new Traverse(new OptionsScreen(false, false));

        private NertsPlusGameScreen gameScreen;

        public ConfigScreen(NertsPlusGameScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }

        public bool IsFullscreen => false;

        public void OnActivate(bool beingPushed) { }

        public void OnReload() { }

        private static Vector2 GetLabelPosition(int index)
        {
            return new Vector2(1333f, 1730 - 140 * index);
        }

        private static Vector2 DrawLabel(int index, String label)
        {
            Vector2 position = GetLabelPosition(index);

            optionsScreenTraversal
                .Method("method_0", new object[] { 0, false, "" })
                .GetValue(new object[] { position.float_1, true, label });

            return position;
        }

        private static Vector2 GetButtonPosition(int index)
        {
            Vector2 labelPosition = GetLabelPosition(index);
            return new Vector2(1838f, labelPosition.float_1);
        }

        private static void DrawDropdown(String label, int index, OptionsListItem[] options)
        {
            Vector2 labelPosition = DrawLabel(index, label);

            optionsScreenTraversal
                .Method("method_1", new object[] { 0, false, new OptionsListItem[] { } })
                .GetValue(new object[] { labelPosition.float_1, true, options });
        }

        private static bool DrawButton(float index, String text)
        {
            return optionsScreenTraversal
                .Method("method_4", new object[] { 0, false, "" })
                .GetValue<bool>(new object[] { index, true, text.ToUpper() });
        }

        private static bool DrawFooterButton(Vector2 position, String text)
        {
            return OptionsScreen.smethod_1(position, text.ToUpper());
        }

        public void Update(float timeDelta)
        {
            Texture modalBackground = GClass62.smethod_0().gclass130_0.texture_0;

            Texture line = GClass62.smethod_0().gclass130_0.texture_12;
            float lineWidth = line.method_0();

            // Draw backdrop
            Utils.DrawColor(Color.Black.WithAlpha(0.5f), Utils.GetScreenBounds());

            // Draw background
            Bounds2 bounds = Bounds2.WithSize(Utils.GetScreenBounds().Center - modalBackground.vector2_0 / 2, modalBackground.vector2_0);
            Utils.DrawTexture(modalBackground, bounds.Min);

            // Draw title
            Utils.NewTextDrawingOptions()
                .WithText("NERTSPLUS OPTIONS")
                .WithPosition(bounds.TopCenter + new Vector2(0, -150))
                .AlignedCenter()
                .WithFont(GClass62.smethod_1().gclass12_10)
                .WithColor(Utils.White().WithAlpha(0.6f))
                .Draw();

            // Draw lines
            Utils.DrawTexture(line, bounds.BottomCenter + new Vector2(-lineWidth / 2f, 1581));
            Utils.DrawTexture(line, bounds.BottomCenter + new Vector2(-lineWidth / 2f, 236));

            DrawDropdown("Intro", 0, new OptionsListItem[] {
                new OptionsListItem("Normal", ChangeStartMusic.START_MUSIC == ChangeStartMusic.StartMusic.Normal, () => {
                    ChangeStartMusic.START_MUSIC = ChangeStartMusic.StartMusic.Normal;
                }),
                new OptionsListItem("Countdown", ChangeStartMusic.START_MUSIC == ChangeStartMusic.StartMusic.Countdown, () => {
                    ChangeStartMusic.START_MUSIC = ChangeStartMusic.StartMusic.Countdown;
                }),
                new OptionsListItem("None", ChangeStartMusic.START_MUSIC == ChangeStartMusic.StartMusic.None, () => {
                    ChangeStartMusic.START_MUSIC = ChangeStartMusic.StartMusic.None;
                })
            });

            Vector2 endRoundLabelPosition = DrawLabel(1, "End Round");

            if (DrawButton(GetButtonPosition(1).float_1, "END ROUND")) {
                gameScreen.GetGameServer().EndRound();
                NertsPlusScreenStack.Pop(true);
            }

            if (DrawFooterButton(bounds.BottomCenter + new Vector2(45, 78), "RETURN TO GAME")  || Utils.IsKeyPressed(Key.ESCAPE) || (!bounds.Contains(Utils.GetCursorPosition()) && Utils.IsMouseDown()))
            {
                NertsPlusScreenStack.Pop(true);
            }
        }
    }
}
