using System.Collections.Generic;
using System.Linq;

namespace SnakeRules
{
    public class Snake
    {
        private IList<Point> _parts;
        private Point? _food;
        private readonly Size _boardSize;
        private Direction _direction;
        private bool _isDead;

        public delegate void DeathHandler();

        public event DeathHandler Died;

        public Snake(IList<Point> parts, Size boardSize, Point food = new())
        {
            _parts = parts;
            _food = food;
            _boardSize = new Size(boardSize.Width, boardSize.Height);
            GuessDirection();
        }

        public void Turn(Direction direction)
        {
            if (GetNextToHead() == GetHead() + direction.GetPoint()) return;

            _direction = direction;
        }

        public void Move()
        {
            if (_isDead) return;

            var newParts = _parts.Select((part, index) => index == 0
                ? part + _direction.GetPoint()
                : _parts[index - 1]
            ).ToList();
            var newHead = newParts.First();
            if (_food == newHead)
            {
                newParts.Add(GetTail());
                _food = null;
            }

            if (HasCollisions(newParts))
            {
                Die();
                return;
            }

            _parts = newParts;
        }

        private bool HasCollisions(IReadOnlyCollection<Point> newParts)
        {
            var newHead = newParts.First();
            var bodyPartsCollidedWithHead = newParts.Skip(1).Where(bodyPart => bodyPart == newHead);
            var board = new Rectangle(_boardSize);

            return bodyPartsCollidedWithHead.Any() || !board.Contains(new Point(newHead.X, newHead.Y));
        }

        public void Render(ISnakeRenderer snakeRenderer, IFoodRenderer foodRenderer)
        {
            foreach (var part in _parts)
            {
                if (_parts.IndexOf(part) == 0)
                {
                    snakeRenderer.RenderHead(part, (GetHead() - GetNextToHead()).GetDirection());
                    continue;
                }

                if (_parts.IndexOf(part) == _parts.Count - 1)
                {
                    var previous = _parts.ElementAt(_parts.IndexOf(part) - 1);
                    snakeRenderer.RenderTail(part, (previous - part).GetDirection());
                    continue;
                }

                var toPrev = (_parts.ElementAt(_parts.IndexOf(part) - 1) - part).GetDirection();
                var toNext = (_parts.ElementAt(_parts.IndexOf(part) + 1) - part).GetDirection();

                snakeRenderer.RenderBody(part, toPrev, toNext);
            }

            if (_food is { } food)
            {
                foodRenderer.RenderFood(food);
            }
        }

        private void Die()
        {
            Died?.Invoke();
            _isDead = true;
        }

        private void GuessDirection()
        {
            _direction = (GetHead() - GetNextToHead()).GetDirection();
        }

        private Point GetHead() => _parts.First();
        private Point GetNextToHead() => _parts.ElementAt(1);
        private Point GetTail() => _parts.Last();

        public bool IsDead()
        {
            return _isDead;
        }
    }

    public interface ISnakeRenderer
    {
        public void RenderHead(Point coordinates, Direction direction);
        public void RenderBody(Point coordinates, Direction toPrev, Direction toNext);
        public void RenderTail(Point coordinates, Direction direction);
    }

    public interface IFoodRenderer
    {
        public void RenderFood(Point coordinates);
    }
}