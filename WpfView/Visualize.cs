using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace WpfView
{
    public static class Visualize
    {
        enum Direction
        {
            North,
            East,
            South,
            West
        }

        private static Race _race;

        #region imageSizes

        private const int SectionDimensions = 300;
        private const int ParticipantDimensions = 64;

        #endregion imageSizes

        #region imagePaths
        
        // Tracks
        private const string StraightHorizontal = @".\Resources\Tracks\straight-horizontal.png";
        private const string StraightVertical = @".\Resources\Tracks\straight-vertical.png";

        private const string FinishHorizontal = @".\Resources\Tracks\finish-horizontal.png";
        private const string FinishVertical = @".\Resources\Tracks\finish-vertical.png";

        private const string StartGrid = @".\Resources\Tracks\start-grid.png";

        private const string CornerRight = @".\Resources\Tracks\corner-right.png";
        private const string CornerLeft = @".\Resources\Tracks\corner-left.png";

        // Cars
        private const string BlueCar = @".\Resources\Cars\car-blue.png";
        private const string GreenCar = @".\Resources\Cars\car-green.png";
        private const string GreyCar = @".\Resources\Cars\car-grey.png";
        private const string RedCar = @".\Resources\Cars\car-red.png";
        private const string YellowCar = @".\Resources\Cars\car-yellow.png";
        private const string DestroyedCar = @".\Resources\Cars\car-destroyed.png";

        #endregion

        public static void Initialize(Race race)
        {
            _race = race;
        }

        public static BitmapSource DrawTrack(Track track)
        {
            (int width, int height) = DetermineTrackSize(track);

            Bitmap bitmap = ImageCache.CreateBitmap(width + 600, height + 66);
            Graphics graphics = Graphics.FromImage(bitmap);

            int x = 666, y = 66;
            Direction currentDirection = Direction.East;

            foreach (var section in track.Sections)
            {
                DrawSection(x, y, currentDirection, graphics, section);

                currentDirection = DetermineDirection(section.SectionType, currentDirection);

                DrawSectionParticipants(x, y, currentDirection, graphics, section);

                DetermineNewCoordinates(ref x, ref y, currentDirection);
            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(bitmap);
        }

        private static void DrawSection(int x, int y, Direction direction, Graphics graphics, Section section)
        {
            Bitmap bitmap =
                ImageCache.GetImageFromCache(FileFromSectionType(section.SectionType, direction));

            if (section.SectionType == SectionTypes.LeftCorner || section.SectionType == SectionTypes.RightCorner)
            {
                RotateSection(bitmap, direction);
            }

            graphics.DrawImage(bitmap, x, y, SectionDimensions, SectionDimensions);
        }

        private static void DrawSectionParticipants(int x, int y, Direction direction, Graphics graphics,
            Section section)
        {
            IParticipant leftParticipant = _race.GetSectionData(section).Left;
            IParticipant rightParticipant = _race.GetSectionData(section).Right;

            if (leftParticipant != null)
            {
                (int offsetX, int offsetY) = GetParticipantOffset(0, direction);
                DrawParticipant(leftParticipant, graphics, direction, x + offsetX, y + offsetY);
            }

            if (rightParticipant != null)
            {
                (int offsetX, int offsetY) = GetParticipantOffset(1, direction);
                DrawParticipant(rightParticipant, graphics, direction, x + offsetX, y + offsetY);
            }
        }

        private static void DrawParticipant(IParticipant participant, Graphics graphics, Direction direction, int xPos,
            int yPos)
        {
            Bitmap bitmap = ImageCache.GetImageFromCache(
                FileFromTeamColor(
                    participant.TeamColor,
                    participant?.Equipment?.IsBroken ?? false
                ));

            RotateSection(bitmap, direction);

            graphics.DrawImage(bitmap, xPos, yPos, ParticipantDimensions, ParticipantDimensions);
        }

        /**
         * Get offset in pixels
         *
         * Accepts 0 (left) or 1 (right) for side
         */
        private static (int x, int y) GetParticipantOffset(int side, Direction currentDirection)
        {
            return side == 0
                ? currentDirection switch
                {
                    Direction.North => (90, 0),
                    Direction.East => (90, 45),
                    Direction.South => (190, 90),
                    Direction.West => (90, 45),
                    _ => (0, 0)
                }
                : currentDirection switch
                {
                    Direction.North => (190, 0),
                    Direction.East => (90, 190),
                    Direction.South => (45, 190),
                    Direction.West => (90, 190),
                    _ => (0, 0)
                };
        }

        private static string FileFromSectionType(SectionTypes sectionType, Direction direction)
        {
            return sectionType switch
            {
                SectionTypes.Straight => direction switch
                {
                    Direction.North => StraightVertical,
                    Direction.East => StraightHorizontal,
                    Direction.South => StraightVertical,
                    Direction.West => StraightHorizontal,
                    _ => throw new Exception("Unsupported straight direction")
                },
                SectionTypes.Finish => direction switch
                {
                    Direction.North => FinishVertical,
                    Direction.East => FinishHorizontal,
                    Direction.South => FinishVertical,
                    Direction.West => FinishHorizontal,
                    _ => throw new Exception("Unsupported finish direction")
                },
                SectionTypes.RightCorner => CornerRight,
                SectionTypes.LeftCorner => CornerLeft,
                SectionTypes.StartGrid => StartGrid,
                _ => throw new Exception("Unsupported section type")
            };
        }

        private static string FileFromTeamColor(TeamColors color, bool isBroken)
        {
            return isBroken
                ? DestroyedCar
                : color switch
                {
                    TeamColors.Blue => BlueCar,
                    TeamColors.Green => GreenCar,
                    TeamColors.Red => RedCar,
                    TeamColors.Yellow => YellowCar,
                    TeamColors.Grey => GreyCar,
                    _ => throw new Exception("Unsupported team color")
                };
        }

        private static void RotateSection(Bitmap img, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Direction.South:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.West:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
            }
        }

        private static (int width, int height) DetermineTrackSize(Track track)
        {
            List<int> positionsX = new List<int>();
            List<int> positionsY = new List<int>();

            int x = 600;
            int y = 200;

            Direction currentDirection = Direction.East;

            foreach (var section in track.Sections)
            {
                positionsX.Add(x);
                positionsY.Add(y);

                currentDirection = DetermineDirection(section.SectionType, currentDirection);

                DetermineNewCoordinates(ref x, ref y, currentDirection);
            }

            int width = positionsX.Max();
            int height = positionsY.Max();

            Trace.WriteLine($"{width + x}, {height + y}");

            return (width + x, height + y);
        }

        private static void DetermineNewCoordinates(ref int x, ref int y, Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    y -= SectionDimensions;
                    break;
                case Direction.East:
                    x += SectionDimensions;
                    break;
                case Direction.South:
                    y += SectionDimensions;
                    break;
                case Direction.West:
                    x -= SectionDimensions;
                    break;
                default:
                    throw new Exception("Unsupported direction");
            }
        }

        private static Direction DetermineDirection(SectionTypes sectionType, Direction currentDirection)
        {
            Direction newDirection = currentDirection;

            switch (sectionType)
            {
                case SectionTypes.RightCorner:
                    newDirection = currentDirection switch
                    {
                        Direction.East => Direction.South,
                        Direction.South => Direction.West,
                        Direction.West => Direction.North,
                        Direction.North => Direction.East,
                        _ => throw new Exception("Unsupported prev direction")
                    };
                    break;
                case SectionTypes.LeftCorner:
                    newDirection = currentDirection switch
                    {
                        Direction.East => Direction.North,
                        Direction.North => Direction.West,
                        Direction.West => Direction.South,
                        Direction.South => Direction.East,
                        _ => throw new Exception("Unsupported prev direction")
                    };
                    break;
            }

            return newDirection;
        }
    }
}