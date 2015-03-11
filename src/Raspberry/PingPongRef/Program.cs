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
                
                table.Open();

                Console.WriteLine(Resources.Instructions);
                Console.ReadLine();
            }
        }
    }
}
