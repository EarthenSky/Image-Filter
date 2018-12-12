namespace ImGuiNET.ImageFilter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (var app = new App()) { app.Run(); }  // This code is safer than just using a variable.
        }
    }
}