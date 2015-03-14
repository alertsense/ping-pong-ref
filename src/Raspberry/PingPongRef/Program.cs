using System;
using System.Threading;
using AlertSense.PingPong.Raspberry.IO;

namespace AlertSense.PingPong.Raspberry
{
    class Program
    {
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
            Console.WriteLine("Bounce: {{Side: {0}, Elapsed: {1}}}", e.Type, e.ElapsedMilliseconds);
        }
    }
}
