using System;
using Controller;
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

        private static string[] _startGridHorizontal = {"----", "  L  ", "   R", "----"};
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

        private static int _x = 8;
        private static int _y = 4;
        private static Direction _direction = Direction.East;
        private static Section _currentSection;

        public static void Initialize()
        {
            // ?
        }

        public static void DrawTrack(Track track)
        {
            foreach (var section in track.Sections)
            {
                _currentSection = section;
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

        private static string ReverseString(string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static string PlaceParticipants(string input, IParticipant p1, IParticipant p2)
        {
            return input.Replace("L", p1.Name.Substring(0, 1))
                .Replace("R", p2.Name.Substring(0, 1));
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
                        Direction.North => Direction.East,
                        _ => throw new Exception("Unsupported prev direction")
                    };
                    break;
                case SectionTypes.LeftCorner:
                    _direction = _direction switch
                    {
                        Direction.East => Direction.North,
                        Direction.North => Direction.West,
                        Direction.West => Direction.South,
                        Direction.South => Direction.East,
                        _ => throw new Exception("Unsupported prev direction")
                    };
                    break;
            }
        }

        private static void DrawSection(string[] lines)
        {
            var y2 = _y;
            var x2 = _x;
            var shouldReverse = ShouldReverse();

            if (shouldReverse) Array.Reverse(lines);

            foreach (var line in lines)
            {
                string newLine = line;
                if (_currentSection.SectionType == SectionTypes.StartGrid)
                {
                    newLine = PlaceParticipants(line, Data.CurrentRace.GetSectionData(_currentSection).Left,
                        Data.CurrentRace.GetSectionData(_currentSection).Right);
                }

                Console.SetCursorPosition(x2, y2);
                Console.Write(shouldReverse ? ReverseString(newLine) : newLine);

                y2++;
            }

            if (shouldReverse) Array.Reverse(lines);
        }
    }
}