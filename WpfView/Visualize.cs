using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
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

        private const string BaseDirectory = @"C:\Users\Rick\Documents\git\windesheim-racebaan\WpfView";

        // Tracks
        private const string StraightHorizontal = @"\Resources\Tracks\straight-horizontal.png";
        private const string StraightVertical = @"\Resources\Tracks\straight-vertical.png";

        private const string FinishHorizontal = @"\Resources\Tracks\finish-horizontal.png";
        private const string FinishVertical = @"\Resources\Tracks\finish-vertical.png";

        private const string StartGrid = @"\Resources\Tracks\start-grid.png";

        private const string CornerRight = @"\Resources\Tracks\corner-right.png";
        private const string CornerLeft = @"\Resources\Tracks\corner-left.png";

        // Cars
        private const string BlueCar = @"\Resources\Cars\car-blue.png";
        private const string GreenCar = @"\Resources\Cars\car-green.png";
        private const string GreyCar = @"\Resources\Cars\car-grey.png";
        private const string RedCar = @"\Resources\Cars\car-red.png";
        private const string YellowCar = @"\Resources\Cars\car-yellow.png";
        private const string DestroyedCar = @"\Resources\Cars\car-destroyed.png";

        #endregion

        public static void Initialize(Race race)
        {
            _race = race;
        }

        public static BitmapSource DrawTrack(Track track)
        {
            (int width, int height) = DetermineTrackSize(track);

            Bitmap bitmap = ImageCache.CreateBitmap(width + 750, height + 750);
            Graphics graphics = Graphics.FromImage(bitmap);

            int x = 750, y = 150;
            Direction currentDirection = Direction.East;

            foreach (var section in track.Sections)
            {
                DrawSection(x, y, currentDirection, graphics, section);

                currentDirection = DetermineDirection(section.SectionType, currentDirection);

                DetermineNewCoordinates(ref x, ref y, currentDirection);
            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(bitmap);
        }

        private static void DrawSection(int x, int y, Direction direction, Graphics graphics, Section section)
        {
            Bitmap sectionBitmap =
                ImageCache.GetImageFromCache(BaseDirectory + FileFromSectionType(section.SectionType, direction));

            if (section.SectionType == SectionTypes.LeftCorner || section.SectionType == SectionTypes.RightCorner)
            {
                RotateSection(sectionBitmap, direction);
            }

            graphics.DrawImage(sectionBitmap, x, y, SectionDimensions, SectionDimensions);
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
            };
        }

        private static void RotateSection(Bitmap img, Direction direction)
        {
            switch ((int)direction)
            {
                case 0:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 2:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
            }
        }

        private static (int width, int height) DetermineTrackSize(Track track)
        {
            int startX = 10, startY = 10;
            int currentX = startX, currentY = startY;

            List<int> positionsX = new List<int>();
            List<int> positionsY = new List<int>();

            Direction currentDirection = Direction.East; // we always start east

            foreach (var section in track.Sections)
            {
                positionsX.Add(currentX);
                positionsY.Add(currentY);

                if (section.SectionType == SectionTypes.RightCorner || section.SectionType == SectionTypes.LeftCorner)
                {
                    currentDirection = DetermineDirection(section.SectionType, currentDirection);
                }

                DetermineNewCoordinates(ref currentX, ref currentY, currentDirection);
            }

            int minX = positionsX.Min();
            int minY = positionsY.Min();

            int maxX = positionsX.Max() + 1;
            int maxY = positionsY.Max() + 1;

            int width = maxX - minX;
            int height = maxY - minY;

            return (width, height);
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