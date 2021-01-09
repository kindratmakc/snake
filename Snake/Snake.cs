using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Snake
{
    public class Snake
    {
        private IList<Vector2> _parts;
        private Direction _direction;

        public Snake(IList<Vector2> parts)
        {
            _parts = parts;
            GuessDirection();
        }

        public void Eat()
        {
            var tail = _parts.Last().Clone();
            Move();
            _parts.Add(tail);
        }

        public void Turn(Direction direction)
        {
            if (_direction.IsOppositeTo(direction)) return;
            _direction = direction;
        }

        public void Move()
        {
            _parts = _parts.Select((part, index) => index == 0 ? part + _direction.GetVector() : _parts[index - 1].Clone()).ToList();
        }

        private void GuessDirection()
        {
            _direction = (GetHead() - GetNextToHead()).GetDirection();
        }

        public IList<Vector2> GetState()
        {
            return _parts;
        }

        private Vector2 GetHead() => _parts.First();

        private Vector2 GetNextToHead() => _parts.ElementAt(1);
    }

    public static class VectorExtension
    {
        public static Vector2 Clone(this Vector2 vector)
        {
            return new(vector.X, vector.Y);
        }
    }
}