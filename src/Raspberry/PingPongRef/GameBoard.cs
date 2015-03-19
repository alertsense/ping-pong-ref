using AlertSense.PingPong.Raspberry.Models;
using AlertSense.PingPong.Raspberry.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.Raspberry
{
    public class GameBoard : IGameBoard
    {
        private IDictionary<string, Coordinates> _coordinates;
        private Coordinates _outputCoords;

        public GameBoard()
        {
            _coordinates = new Dictionary<string, Coordinates>();
            _outputCoords = new Coordinates {Left = 0, Top = 23};
        }
        
        public void DrawInititalScreen(Table table1, Table table2)
        {
            _coordinates.Add(table1.Name, new Coordinates { Top = 10, Left = 5, Width = 30, Height = 6 });
            _coordinates.Add(table2.Name, new Coordinates { Top = 10, Left = 40, Width = 30, Height = 6 });

            Console.Clear();
            Console.Title = "Ping Pong Ref";
            Console.CursorVisible = false;
            Console.WriteLine(Resources.Banner);

            DrawTable(table1);
            UpdateTable(table1);

            DrawTable(table2);
            UpdateTable(table2);

            ResetCursor();
        }

        public void DrawTable(Table table)
        {
            var coords = _coordinates[table.Name];
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(coords.Left, coords.Top);
            Console.Write("┌" + new String('─', coords.Width - 2) + "╖");
            for (int y = (coords.Top + 1); y < (coords.Top + coords.Height); y++)
            {
                Console.SetCursorPosition(coords.Left, y);
                Console.Write("│");
                Console.SetCursorPosition(coords.Left + coords.Width - 1, y);
                Console.Write("║");
            }
            Console.SetCursorPosition(coords.Left, coords.Top + coords.Height);
            Console.Write("╘" + new String('─', coords.Width - 2) + "╝");
            ResetCursor();
        }

        public void UpdateTable(Table table)
        {
            Coordinates coords;

            if (!_coordinates.TryGetValue(table.Name, out coords))
            {
                ShowWarning("Failed to find coordinates for " + table.Name);
                return;
            }

            Console.SetCursorPosition(coords.Left + 3, coords.Top + 2);
            Console.ForegroundColor = table.ButtonState ? ConsoleColor.Cyan : ConsoleColor.White;
            Console.Write("[BTN-" + table.Settings.ButtonPin + "]");

            Console.SetCursorPosition(coords.Left + 3, coords.Top + 4);
            Console.ForegroundColor = table.ServiceLight ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write("[LED-" + table.Settings.LedPin + "]");

            Console.SetCursorPosition(coords.Left + 16, coords.Top + 3);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[BNC-" + table.Settings.BouncePin + "]");

            ResetCursor();
        }

        public void UpdateGame(Game game)
        {
            Console.SetCursorPosition(0, 20);
            Console.Write("Game Id: {0}", game.Id.ToString("D"));
            ResetCursor();
        }

        private class Coordinates
        {
            public int Top { get; set; }
            public int Left { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }

        public void ShowWarning(string message)
        {
            Console.SetCursorPosition(0, 18);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            ResetCursor();
        }

        public void ShowMessage(string message)
        {
            Console.SetCursorPosition(0, 18);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            ResetCursor();
        }

        public void ResetCursor()
        {
            Console.ResetColor();
            Console.SetCursorPosition(_outputCoords.Left, _outputCoords.Top);
        }
    }
}
