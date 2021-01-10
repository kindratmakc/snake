using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Snake
{
    public class Snake
    {
        private IList<Vector2> _parts;
        private readonly Point _boardSize;
        private Direction _direction;
        private bool _isDead;

        public Snake(IList<Vector2> parts, Point boardSize)
        {
            _parts = parts;
            _boardSize = boardSize;
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
            if (GetNextToHead() == GetHead() + direction.GetVector()) return;

            _direction = direction;
        }

        public void Move()
        {
            _parts = _parts.Select((part, index) =>
            {
                if (_isDead)
                {
                    return part;
                }
                if (index == 0)
                {
                    var nextHeadPosition = part + _direction.GetVector();
                    var board = new Rectangle(new Point(0, 0), new Size(_boardSize));

                    if (!board.Contains(new Point((int)nextHeadPosition.X, (int)nextHeadPosition.Y)))
                    {
                        Die();
                        return part;
                    }

                    if (_parts.Contains(nextHeadPosition))
                    {
                        Die();
                        return part;
                    }

                    return nextHeadPosition;
                }
                
                return _parts[index - 1].Clone();
            }).ToList();
        }

        public IList<Vector2> GetState()
        {
            return _parts;
        }

        public void Render(ISnakeRenderer renderer)
        {
            foreach (var part in _parts)
            {
                if (_parts.IndexOf(part) == 0)
                {
                    renderer.RenderHead(part, (GetHead() - GetNextToHead()).GetDirection());
                    continue;
                }

                if (_parts.IndexOf(part) == _parts.Count - 1)
                {
                    var previous = _parts.ElementAt(_parts.IndexOf(part) - 1);
                    renderer.RenderTail(part, (previous - part).GetDirection());
                    continue;
                }

                var toPrev = (_parts.ElementAt(_parts.IndexOf(part) - 1) - part).GetDirection();
                var toNext = (_parts.ElementAt(_parts.IndexOf(part) + 1) - part).GetDirection();
                
                renderer.RenderBody(part, toPrev, toNext);
            }
        }

        private void Die()
        {
            Console.WriteLine("Died");
            _isDead = true;
        }

        private void GuessDirection()
        {
            _direction = (GetHead() - GetNextToHead()).GetDirection();
        }

        private Vector2 GetHead() => _parts.First();

        private Vector2 GetNextToHead() => _parts.ElementAt(1);

        public bool IsDead()
        {
            return _isDead;
        }
    }

    public static class VectorExtension
    {
        public static Vector2 Clone(this Vector2 vector)
        {
            return new(vector.X, vector.Y);
        }
    }
    
    public interface ISnakeRenderer
    {
        public void RenderHead(Vector2 coordinates, Direction direction);
        public void RenderBody(Vector2 coordinates, Direction toPrev, Direction toNext);
        public void RenderTail(Vector2 coordinates, Direction direction);
    }
}