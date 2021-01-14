namespace SnakeRules
{
    internal readonly struct Rectangle
    {
        private readonly int _width;
        private readonly int _height;

        public Rectangle(Size size)
        {
            _width = size.Width;
            _height = size.Height;
        }

        public bool Contains(Point point)
        {
            return point.X >= 0 && point.X < _width && point.Y >= 0 && point.Y < _height;
        }
    }
}