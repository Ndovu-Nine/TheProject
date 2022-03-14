using System;
using TemboShared.Service;
using TemboAI.Core.Conscious;

namespace TemboAI
{
    class Program
    {
        static void Main(string[] args)
        {
            var dc=CN.Db.Asset.All();
            /*args=new string[5];
            args[0] = @"fromFile#EURUSD#M1#C:\ForexData\EURUSD.csv#01/01/2000";*/
            if (args.Length <= 0)
            {
                "You must provide a launch command".Log(1);
                Console.ReadKey();
                return;
            }

            var commands = args[0].Split('#');
            if (commands.Length <= 0)
            {
                "You must provide a launch command".Log(1);
                Console.ReadKey();
                Console.ReadKey();
                return;
            }

            if (commands[0] == "fromFile")
            {
                var asset1 = commands[1];
                var duration1 = commands[2];
                var launch1 = new TemboRaw(asset1, duration1);
                launch1.RunFromFile(commands[3],commands[4]);
                "Program Exited.".Log(1, true);
            }

            else
            {
                var asset = commands[0];
                var duration = commands[1];
                var branches =int.Parse(commands[2]);
                var leaves = int.Parse(commands[3]);
                var launch = new TemboRaw(asset, duration,branches,leaves);
                launch.Run();
                "Program Exited.".Log(1, true);
                Console.ReadKey();
                Console.ReadKey();
            }
        }

        static string ToPercentage(double a, double b)
        {
            return (a / b).ToString("P");
        }
    }
}
