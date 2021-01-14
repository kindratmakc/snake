using System;

namespace SnakeRules
{
    public readonly struct Point
    {
        public static readonly Point Zero = new(0, 0);

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public Direction GetDirection()
        {
            return this switch
            {
                {X: 0, Y: -1} => Direction.Up,
                {X: 0, Y: 1} => Direction.Down,
                {X: 1, Y: 0} => Direction.Right,
                {X: -1, Y: 0} => Direction.Left,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;
    }
}