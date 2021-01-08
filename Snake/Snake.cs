using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Snake
{
    public class Snake
    {
        private IList<Vector2> _parts;

        public Snake(IList<Vector2> parts)
        {
            _parts = parts;
        }


        public IList<Vector2> GetState()
        {
            return _parts;
        }

        public void Move(Direction direction)
        {
            var first = _parts.First();
            var second = _parts.ElementAt(1);
            var moveX = first.Y.Equals(second.Y);

            _parts = _parts.Select((part, index) =>
                {
                    if (index == 0)
                    {
                        switch (direction)
                        {
                            case Direction.Up:
                                return new Vector2(part.X, part.Y - 1);
                            case Direction.Down:
                                return new Vector2(part.X, part.Y + 1);
                            case Direction.Right:
                                return new Vector2(part.X + 1, part.Y);
                            case Direction.Left:
                                return new Vector2(part.X - 1, part.Y);
                            case Direction.Forward:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                        }
                    }

                    return moveX
                        ? new Vector2(part.X + 1, part.Y)
                        : new Vector2(part.X, part.Y + 1);
                }
            ).ToList();
        }
    }
}