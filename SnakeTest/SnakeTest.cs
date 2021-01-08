using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace SnakeTest
{
    public class SnakeTest
    {
        [Fact]
        public void MovesUp()
        {
            var snake = new Snake(new List<Vector2>
            {
                new(2, 1), // - - -
                new(1, 1), // o o >
                new(0, 1), // - - -
            });

            snake.Move(Direction.Up);

            Assert.Equal(new List<Vector2>
            {
                new(2, 0), // - - >
                new(2, 1), // - o o
                new(1, 1), // - - -
            }, snake.GetState());
        }

        [Fact]
        public void MovesDown()
        {
            var snake = new Snake(new List<Vector2>
            {
                new(2, 0), // o o >
                new(1, 0), // - - -
                new(0, 0), // - - -
            });

            snake.Move(Direction.Down);

            Assert.Equal(new List<Vector2>
            {
                new(2, 1), // - o o
                new(2, 0), // - - >
                new(1, 0), // - - -
            }, snake.GetState());
        }

        [Fact]
        public void MovesRight()
        {
            var snake = new Snake(new List<Vector2>
            {
                new(0, 2), // o - -
                new(0, 1), // o - -
                new(0, 0), // > - -
            });

            snake.Move(Direction.Right);

            Assert.Equal(new List<Vector2>
            {
                new(1, 2), // - - -
                new(0, 2), // o - -
                new(0, 1), // o > -
            }, snake.GetState());
        }

        [Fact]
        public void MovesForwardHorizontally()
        {
            var snake = new Snake(new List<Vector2>
            {
                new(2, 0),
                new(1, 0),
                new(0, 0),
            });

            snake.Move(Direction.Forward);

            Assert.Equal(new List<Vector2>
            {
                new(3, 0),
                new(2, 0),
                new(1, 0),
            }, snake.GetState());
        }

        [Fact]
        public void MovesForwardVertically()
        {
            var snake = new Snake(new List<Vector2>
            {
                new(0, 2),
                new(0, 1),
                new(0, 0),
            });

            snake.Move(Direction.Forward);

            Assert.Equal(new List<Vector2>
            {
                new(0, 3),
                new(0, 2),
                new(0, 1),
            }, snake.GetState());
        }
    }
}