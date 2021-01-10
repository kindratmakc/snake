﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Snake
{
    public class Snake
    {
        private IList<Vector2> _parts;
        private readonly Size _boardSize;
        private Direction _direction;
        private bool _isDead;

        public Snake(IList<Vector2> parts, Size boardSize)
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
            var newParts = _parts.Select((part, index) => index == 0
                    ? part + _direction.GetVector()
                    : _parts[index - 1].Clone()
                ).ToList();
            if (HasCollisions(newParts))
            {
                Die();
                return;
            }

            _parts = newParts;
        }

        private bool HasCollisions(IReadOnlyCollection<Vector2> newParts)
        {
            var newHead = newParts.First();
            var bodyPartsCollidedWithHead = newParts.Skip(1).Where(bodyPart => bodyPart == newHead);
            var board = new Rectangle(new Point(0, 0), _boardSize);

            return bodyPartsCollidedWithHead.Any() || !board.Contains(new Point((int) newHead.X, (int) newHead.Y));
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