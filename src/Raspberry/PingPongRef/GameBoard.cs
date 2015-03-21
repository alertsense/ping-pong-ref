using AlertSense.PingPong.Raspberry.Models;
using AlertSense.PingPong.Raspberry.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace AlertSense.PingPong.Raspberry
{
    public class GameBoard : IGameBoard
    {
        private readonly IDictionary<string, Coordinates> _coordinates;
        private readonly Coordinates _topLogCoordinates;

        public GameBoard()
        {
            _coordinates = new Dictionary<string, Coordinates>();
            _topLogCoordinates = new Coordinates {Left = 0, Top = 18};
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

            Console.ResetColor();
            Console.SetCursorPosition(_topLogCoordinates.Left, _topLogCoordinates.Top);
        }

        public void DrawTable(Table table)
        {
            SaveCursor();
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
            RestoreCursor();
        }

        public void UpdateTable(Table table)
        {
            SaveCursor();
            Coordinates coords;

            if (!_coordinates.TryGetValue(table.Name, out coords))
            {
                LogError("Failed to find coordinates for " + table.Name);
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

            RestoreCursor();
        }

        private int _cursorTop;
        private int _cursorLeft;

        private void SaveCursor()
        {
            _cursorTop = Console.CursorTop;
            _cursorLeft = Console.CursorLeft;
        }

        private void RestoreCursor()
        {
            Console.SetCursorPosition(_cursorLeft, _cursorTop);
        }

        public void UpdateGame(Game game)
        {
            LogInfo(String.Format("Game Id: {0}", game.Id.ToString("D")));
        }

        private class Coordinates
        {
            public int Top { get; set; }
            public int Left { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }

        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
        }

        public void LogDebug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
        }
    }
}
