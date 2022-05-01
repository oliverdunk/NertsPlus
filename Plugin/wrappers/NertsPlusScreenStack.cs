namespace NertsPlus
{
    public class NertsPlusScreenStack
    {
        public static void Pop(bool WithSound)
        {
            if (WithSound)
            {
                GClass62.smethod_2().gclass64_0.sound_0.smethod_1();
            }

            ScreenStack.smethod_12();
        }

        public static void Push(IScreen screen, bool WithSound)
        {
            if (WithSound)
            {
                GClass62.smethod_2().gclass64_0.sound_0.smethod_1();
            }

            ScreenStack.smethod_9(screen);
        }
    }
}
