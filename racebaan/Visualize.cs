using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace racebaan
{
    enum Direction
    {
        North,
        East,
        South,
        West
    }
    
    public static class Visualize
    {
        #region graphics

        private static string[] _startGridHorizontal = {"----", "# # ", " # #", "----"};
        private static string[] _finishHorizontal = {"----", "  # ", "  # ", "----"};
        private static string[] _straightTrackHorizontal = {"----", "    ", "    ", "----"};
        private static string[] _rightCornerHorizontal = {"--\\ ", "   \\", "   |", "|  |"};
        private static string[] _leftCornerHorizontal = {" /-- ", "/   ", "|  /", "|  |"};
        
        private static string[] _startGridVertical = {"|# |", "| #|", "|# |", "| #|"};
        private static string[] _finishVertical = {"|  |", "|  |", "|##|", "|  |"};
        private static string[] _straightTrackVertical = {"|  |", "|  |", "|  |", "|  |"};
        private static string[] _rightCornerVertical = {"|  |", "|   ", "\\   ", " \\--"};
        private static string[] _leftCornerVertical = {"|  |", "|   ", "|   ", " \\--"};
        
        #endregion
        
        private static bool isHorizontal = true;
        private static int x = 0;
        private static int y = 0;
        
        private static Direction _direction = Direction.East;

        public static void Initialize()
        {
            // ?
        }

        public static void DrawTrack(Track track)
        {
            foreach (var section in track.Sections)
            {
                Console.SetCursorPosition(x, y);
                
                Console.WriteLine(_direction);

                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        DrawSection(isHorizontal ? _startGridHorizontal : _startGridVertical);
                        break;
                    case SectionTypes.Finish:
                        DrawSection(isHorizontal ? _finishHorizontal : _finishVertical);
                        break;
                    case SectionTypes.Straight:
                        DrawSection(isHorizontal ? _straightTrackHorizontal : _straightTrackVertical);
                        break;
                    case SectionTypes.RightCorner:
                        DrawSection(isHorizontal ? _rightCornerHorizontal : _rightCornerVertical);
                        isHorizontal = !isHorizontal;
                        break;
                    case SectionTypes.LeftCorner:
                        DrawSection(isHorizontal ? _leftCornerHorizontal : _leftCornerVertical);
                        isHorizontal = !isHorizontal;
                        break;
                }
                
                DetermineDirection(_direction, section.SectionType);
                if (isHorizontal) x += 4;
                if (!isHorizontal) y += 4;
            }
        }

        private static void DetermineDirection(Direction direction, SectionTypes sectionType)
        {
            Dictionary<int, Direction> intDirection = new Dictionary<int, Direction>()
            {
                {0, Direction.North},                
                {1, Direction.East},
                {2, Direction.South},
                {3, Direction.West},
            };
            
            Dictionary<Direction, int> directionInt = new Dictionary<Direction, int>()
            {
                {Direction.North, 0},
                {Direction.East, 1}, 
                {Direction.South, 2}, 
                {Direction.West, 3}, 
            };

            // var key = dictionary.FirstOrDefault(x => x.Value == direction).Key;
            // int newDirection = (dictionary.TryGetValue(key) % 4 + 4) % 4;
            //
            // dictionary.TryGetValue((_direction % 4 + 4) % 4);

            // int newDirection _direction = (_direction % 4 + 4) % 4;;
        }
        
        private static void DrawSection(string[] lines)
        {
            int y2 = y;
            int x2 = x;

            foreach (var line in lines)
            {
                y2 += 1;
                
                Console.SetCursorPosition(x2, y2);
                Console.Write(line);
            }
        }
    }
}