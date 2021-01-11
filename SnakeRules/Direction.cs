using System;
using System.Numerics;

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

        public static Vector2 GetVector(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Vector2(0, -1),
                Direction.Down => new Vector2(0, 1),
                Direction.Right => new Vector2(1, 0),
                Direction.Left => new Vector2(-1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
            };
        }
        
        public static Direction GetDirection(this Vector2 vector)
        {
            return vector switch
            {
                {X: 0, Y: -1} => Direction.Up,
                {X: 0, Y: 1} => Direction.Down,
                {X: 1, Y: 0} => Direction.Right,
                {X: -1, Y: 0} => Direction.Left,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}