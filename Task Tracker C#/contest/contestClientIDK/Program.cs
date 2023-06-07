using contestNetworking;
using contestService;

namespace contestClientIDK
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IContestService server = new ContestProtoProxy("127.0.0.1", 55588);
            ContestClientController ctrl = new ContestClientController(server);
            Application.Run(new Login(ctrl));
        }
    }
}