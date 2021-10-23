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

        private static string[] _startGridHorizontal = { "----", "L    ", "R   ", "----" };
        private static string[] _finishHorizontal = { "----", "L # ", "R # ", "----" };
        private static string[] _straightTrackHorizontal = { "----", "L   ", "R   ", "----" };
        private static string[] _rightCornerHorizontal = { "---|", "L  |", "R  |", "|  |" };
        private static string[] _leftCornerHorizontal = { "|  |", "L  |", "R  |", "---|" };

        private static string[] _startGridVertical = { "|RL|", "|  |", "|  |", "|  |" };
        private static string[] _finishVertical = { "|RL|", "|  |", "|##|", "|  |" };
        private static string[] _straightTrackVertical = { "|RL|", "|  |", "|  |", "|  |" };
        private static string[] _rightCornerVertical = { "|RL|", "   |", "   |", "---|" };
        private static string[] _leftCornerVertical = { "|RL|", "|  |", "|   ", "|---" };

        #endregion

        private static int _x = 8;
        private static int _y = 4;
        private static Direction _direction = Direction.East;
        private static Section _currentSection;

        private static Race _currentRace = Data.CurrentRace;

        public static void Initialize()
        {
            _currentRace.DriversChanged += OnDriversChanged;
        }

        public static void DrawTrack(Track track)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(track.Name);

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
            String str = (string)input.Clone();

            return str.Replace("L", p1?.Name?.Substring(0, 1) ?? " ")
                .Replace("R", p2?.Name?.Substring(0, 1) ?? " ");
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

                var p1 = Data.CurrentRace.GetSectionData(_currentSection).Left;
                var p2 = Data.CurrentRace.GetSectionData(_currentSection).Right;

                newLine = PlaceParticipants(line, p1, p2);

                Console.SetCursorPosition(x2, y2);
                Console.Write(shouldReverse ? ReverseString(newLine) : newLine);

                y2++;
            }

            if (shouldReverse) Array.Reverse(lines);
        }

        private static void DrawLaps()
        {
            Console.SetCursorPosition(0, 1);
            Console.Write("Laps: ");

            foreach (var (participant, laps) in _currentRace.DrivenLaps)
            {
                Console.Write($"{participant.Name}: {laps} - ");
            }
        }

        private static void DrawFinished()
        {
            Console.SetCursorPosition(0, 2);
            Console.Write("Finished: ");

            foreach (var (participant, position) in _currentRace.Finished)
            {
                Console.Write($"{participant.Name}: #{position} - ");
            }
        }

        private static void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            DrawTrack(e.Track);
            DrawLaps();
            DrawFinished();
        }
    }
}