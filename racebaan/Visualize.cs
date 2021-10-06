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
        private static string[] _rightCornerHorizontal = {"---|", "   |", "   |", "|  |"};
        private static string[] _leftCornerHorizontal = {"|  |", "   |", "   |", "---|"};
        
        private static string[] _startGridVertical = {"|# |", "| #|", "|# |", "| #|"};
        private static string[] _finishVertical = {"|  |", "|  |", "|##|", "|  |"};
        private static string[] _straightTrackVertical = {"|  |", "|  |", "|  |", "|  |"};
        private static string[] _rightCornerVertical = {"|  |", "   |", "   |", "---|"};
        private static string[] _leftCornerVertical = {"|  |", "|  |", "|   ", "|---"};
        
        #endregion
        
        private static bool _isHorizontal = true;
        private static int _x, _y;
        private static Direction _direction = Direction.East;

        public static void Initialize()
        {
            // ?
        }

        public static void DrawTrack(Track track)
        {
            foreach (var section in track.Sections)
            {
                Console.SetCursorPosition(_x, _y);

                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        DrawSection(_isHorizontal ? _startGridHorizontal : _startGridVertical);
                        break;
                    case SectionTypes.Finish:
                        DrawSection(_isHorizontal ? _finishHorizontal : _finishVertical);
                        break;
                    case SectionTypes.Straight:
                        DrawSection(_isHorizontal ? _straightTrackHorizontal : _straightTrackVertical);
                        break;
                    case SectionTypes.RightCorner:
                        DrawSection(_isHorizontal ? _rightCornerHorizontal : _rightCornerVertical);
                        DetermineDirection(section.SectionType);
                        _isHorizontal = !_isHorizontal;
                        break;
                    case SectionTypes.LeftCorner:
                        DrawSection(_isHorizontal ? _leftCornerHorizontal : _leftCornerVertical);
                        DetermineDirection(section.SectionType);
                        _isHorizontal = !_isHorizontal;
                        break;
                }

                if (_isHorizontal)
                {
                    _x += 4;
                }
                else
                {
                    _y += 4;
                }
            }
        }

        private static void DetermineDirection(SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case SectionTypes.RightCorner:
                    _direction = _direction switch
                    {
                        Direction.East => Direction.South,
                        Direction.South => Direction.West,
                        Direction.West => Direction.North,
                        Direction.North => Direction.East
                    };
                    break;
                case SectionTypes.LeftCorner:
                    _direction = _direction switch
                    {
                        Direction.East => Direction.North,
                        Direction.North => Direction.West,
                        Direction.West => Direction.South,
                        Direction.South => Direction.East
                    };
                    break;
            }  
        }

        private static bool IsReversed()
        {
            return _direction == Direction.North || _direction == Direction.West;
        }
        
        public static string ReverseString( string s )
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
        
        private static void DrawSection(string[] lines)
        {
            int y2 = _y;
            int x2 = _x;

            if (IsReversed()) Array.Reverse(lines);

            foreach (var line in lines)
            {
                if (!IsReversed())
                {
                    y2++;
                }
                Console.SetCursorPosition(x2, y2);
                Console.Write(IsReversed() ? ReverseString(line) : line);
            }

            if (IsReversed()) Array.Reverse(lines);
        }
    }
}