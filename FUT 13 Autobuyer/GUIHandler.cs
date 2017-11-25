namespace WindowsFormsApplication1
{
    public class GuiHandler
    {
        private static GuiHandler _instance;
        public static string LoginSuccess;

        private GuiHandler() { }
        public static GuiHandler Instance => _instance ?? (_instance = new GuiHandler());
    }
}
