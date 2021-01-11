using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeRules;
using Vector2Numeric = System.Numerics.Vector2;

namespace SnakeGame
{
    public class Snake2DRenderer : ISnakeRenderer
    {
        private const int Step = 32;

        private readonly Texture2D _texture;
        private readonly SpriteBatch _batch;

        public Snake2DRenderer(Texture2D texture, SpriteBatch batch)
        {
            _texture = texture;
            _batch = batch;
        }

        public void RenderHead(Vector2Numeric coordinates, Direction direction)
        {
            Render(coordinates.ToXna() * Step, GetHead(direction));
        }

        public void RenderBody(Vector2Numeric coordinates, Direction toPrev, Direction toNext)
        {
            var rectangle = toPrev.IsOppositeTo(toNext)
                ? GetStraightBody(toPrev)
                : GetCurvedBody(toPrev, toNext);
            
            Render(coordinates.ToXna() * Step, rectangle);
        }

        public void RenderTail(Vector2Numeric coordinates, Direction direction)
        {
            Render(coordinates.ToXna() * Step, GetTail(direction));
        }

        private void Render(Vector2 coordinates, Rectangle sourceRectangle)
        {
            _batch.Draw(_texture, coordinates, sourceRectangle, Color.White);
        }

        private Rectangle GetHead(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Rectangle(0, 0, Step, Step),
                Direction.Down => new Rectangle(Step * 2, 0, Step, Step),
                Direction.Right => new Rectangle(Step * 3, 0, Step, Step),
                Direction.Left => new Rectangle(Step, 0, Step, Step),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
            };
        }

        private Rectangle GetCurvedBody(Direction prev, Direction next)
        {
            const int row = Step * 2;

            return (prev, next) switch {
                (Direction.Up, Direction.Right) => new Rectangle(0, row, Step, Step),
                (Direction.Right, Direction.Up) => new Rectangle(0, row, Step, Step),
                (Direction.Up, Direction.Left) => new Rectangle(Step, row, Step, Step),
                (Direction.Left, Direction.Up) => new Rectangle(Step, row, Step, Step),
                (Direction.Down, Direction.Right) => new Rectangle(Step * 3, row, Step, Step),
                (Direction.Right, Direction.Down) => new Rectangle(Step * 3, row, Step, Step),
                (Direction.Down, Direction.Left) => new Rectangle(Step * 2, row, Step, Step),
                (Direction.Left, Direction.Down) => new Rectangle(Step * 2, row, Step, Step),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private Rectangle GetStraightBody(Direction direction)
        {
            const int row = Step;
            
            return direction switch
            {
                Direction.Up => new Rectangle(0, row, Step, Step),
                Direction.Down => new Rectangle(0, row, Step, Step),
                Direction.Right => new Rectangle(Step, row, Step, Step),
                Direction.Left => new Rectangle(Step, row, Step, Step),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
            };
        }

        private Rectangle GetTail(Direction direction)
        {
            const int row = Step * 3;

            return direction switch
            {
                Direction.Up => new Rectangle(0, row, Step, Step),
                Direction.Down => new Rectangle(Step * 2, row, Step, Step),
                Direction.Right => new Rectangle(Step * 3, row, Step, Step),
                Direction.Left => new Rectangle(Step, row, Step, Step),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
            };
        }
    }

    public static class Vector2NumericExtension
    {
        public static Vector2 ToXna(this Vector2Numeric vector2Numeric)
        {
            return new(vector2Numeric.X, vector2Numeric.Y);
        }
    }
}