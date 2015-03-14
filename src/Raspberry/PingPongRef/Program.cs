using System;
using System.Threading;
using AlertSense.PingPong.Raspberry.IO;

namespace AlertSense.PingPong.Raspberry
{
    class Program
    {
        private static bool leftLedOn = false;

        static void Main(string[] args)
        {
            Console.WriteLine(Resources.Banner);

            ConnectionFactory.AddConnection<ITableConnection, TableConnection>();

            using (var table = ConnectionFactory.GetTableConnection()) {

                table.Bounce += table_Bounce;
                table.Open();

                Console.WriteLine(Resources.Instructions);
                Console.ReadLine();
            }
        }

        static void table_Bounce(object sender, BounceEventArgs e)
        {
            var table = (ITableConnection)sender;
            leftLedOn = !leftLedOn;
            table.Led(leftLedOn);

            Console.WriteLine("Bounce");
        }
    }
}
