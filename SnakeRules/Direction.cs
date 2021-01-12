using System;

namespace SnakeRules
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left,
    }

    public static class DirectionExtension
    {
        public static bool IsOppositeTo(this Direction direction, Direction other)
        {
            return direction switch
            {
                Direction.Up => other == Direction.Down,
                Direction.Down => other == Direction.Up,
                Direction.Right => other == Direction.Left,
                Direction.Left => other == Direction.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Point GetPoint(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Point(0, -1),
                Direction.Down => new Point(0, 1),
                Direction.Right => new Point(1, 0),
                Direction.Left => new Point(-1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
            };
        }
    }
}