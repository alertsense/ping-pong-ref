using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry.Models.Interfaces
{
    public interface IGameBoard
    {
        void DrawInititalScreen(Table table1, Table table2);
        void DrawTable(Table table);
        void UpdateTable(Table table);
        void LogError(string message);
    }
}
