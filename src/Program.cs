using System;

namespace Chaotx.Mgx.Demo {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new DemoGame())
                game.Run();
        }
    }
}
