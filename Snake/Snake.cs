using System;
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

        public void Turn(Direction direction)
        {
            if (_direction.IsOppositeTo(direction)) return;
            _direction = direction;
        }

        public void Move()
        {
            _parts = _parts.Select((part, index) =>
                {
                    return index == 0
                        ? Vector2.Add(part, _direction.GetVector())
                        : new Vector2(_parts[index - 1].X, _parts[index - 1].Y);
                }
            ).ToList();
        }
        
        private void GuessDirection()
        {
            _direction = Vector2.Subtract(GetHead(), GetNextToHead()).GetDirection();
        }

        public IList<Vector2> GetState()
        {
            return _parts;
        }

        private Vector2 GetHead() => _parts.First();
        private Vector2 GetNextToHead() => _parts.ElementAt(1);
    }
}