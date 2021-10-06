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

        private static string[] _startGridHorizontal = { "----", "# # ", " # #", "----" };
        private static string[] _finishHorizontal = { "----", "  # ", "  # ", "----" };
        private static string[] _straightTrackHorizontal = { "----", "    ", "    ", "----" };
        private static string[] _rightCornerHorizontal = { "---|", "   |", "   |", "|  |" };
        private static string[] _leftCornerHorizontal = { "|  |", "   |", "   |", "---|" };

        private static string[] _startGridVertical = { "|# |", "| #|", "|# |", "| #|" };
        private static string[] _finishVertical = { "|  |", "|  |", "|##|", "|  |" };
        private static string[] _straightTrackVertical = { "|  |", "|  |", "|  |", "|  |" };
        private static string[] _rightCornerVertical = { "|  |", "   |", "   |", "---|" };
        private static string[] _leftCornerVertical = { "|  |", "|  |", "|   ", "|---" };

        #endregion

        private static int _x = 8;
        private static int _y = 8;
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

                var isHorizontal = IsHorizontal();

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
                        DetermineDirection(section.SectionType);
                        break;
                    case SectionTypes.LeftCorner:
                        DrawSection(isHorizontal ? _leftCornerHorizontal : _leftCornerVertical);
                        DetermineDirection(section.SectionType);
                        break;
                    default:
                        throw new Exception("Unsupported section type");
                }

                switch (_direction)
                {
                    case Direction.North:
                        _y -= 4;
                        break;
                    case Direction.East:
                        _x += 4;
                        break;
                    case Direction.South:
                        _y += 4;
                        break;
                    case Direction.West:
                        _x -= 4;
                        break;
                    default:
                        throw new Exception("Unsupported direction");
                }
            }
        }

        private static bool IsHorizontal()
        {
            return _direction == Direction.East || _direction == Direction.West;
        }

        private static bool ShouldReverse()
        {
            return _direction == Direction.North || _direction == Direction.West;
        }

        public static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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

        private static void DrawSection(string[] lines)
        {
            int y2 = _y;
            int x2 = _x;
            bool shouldReverse = ShouldReverse();

            if (shouldReverse) Array.Reverse(lines);

            foreach (var line in lines)
            {
                Console.SetCursorPosition(x2, y2);
                Console.Write(shouldReverse ? ReverseString(line) : line);

                y2++;
            }

            if (shouldReverse) Array.Reverse(lines);
        }
    }
}