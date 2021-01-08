using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Snake;
using Xunit;

namespace SnakeTest
{
    public class SnakeTest
    {
        [Fact]
        public void MovesUp()
        {
            var snake = CreateSnake(new[]
            {
                new[] {" ", " ", " "},
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
            });

            snake.Move(Direction.Up);

            AssertState(new[]
            {
                new[] {" ", " ", "1"},
                new[] {" ", "3", "2"},
                new[] {" ", " ", " "},
            }, snake.GetState());
        }

        // [Fact]

        // public void MovesUpAndForward()
        // {
        //     var snake = new Snake.Snake(new List<Vector2>
        //     {
        //         new(2, 2), // - - -
        //         new(1, 2), // - - -
        //         new(0, 2), // o o >
        //     });
        //
        //     snake.Move(Direction.Up);
        //     snake.Move(Direction.Forward);
        //
        //     AssertState(new[,]
        //     {
        //         {"", "", ">"},
        //         {"", "", "o"},
        //         {"", "", "o"},
        //     }, snake.GetState());
        // }

        [Fact]
        public void MovesDown()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
                new[] {" ", " ", " "},
            });

            snake.Move(Direction.Down);
            
            AssertState(new[]
            {
                new[] {" ", "3", "2"},
                new[] {" ", " ", "1"},
                new[] {" ", " ", " "},
            }, snake.GetState());
        }

        [Fact]
        public void MovesRight()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"3", " ", " "},
                new[] {"2", " ", " "},
                new[] {"1", " ", " "},
            });

            snake.Move(Direction.Right);
            
            AssertState(new[]
            {
                new[] {" ", " ", " "},
                new[] {"3", " ", " "},
                new[] {"2", "1", " "},
            }, snake.GetState());
        }

        [Fact]
        public void MovesLeft()
        {
            var snake = CreateSnake(new[]
            {
                new[] {" ", "3", " "},
                new[] {" ", "2", " "},
                new[] {" ", "1", " "},
            });

            snake.Move(Direction.Left);
            
            AssertState(new[]
            {
                new[] {" ", " ", " "},
                new[] {" ", "3", " "},
                new[] {"1", "2", " "},
            }, snake.GetState());
        }

        [Fact]
        public void MovesForwardHorizontally()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"3", "2", "1", " "},
                new[] {" ", " ", " ", " "},
                new[] {" ", " ", " ", " "},
            });

            snake.Move(Direction.Forward);
            
            AssertState(new[]
            {
                new[] {" ", "3", "2", "1"},
                new[] {" ", " ", " ", " "},
                new[] {" ", " ", " ", " "},
            }, snake.GetState());
        }

        [Fact]
        public void MovesForwardVertically()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"3", " ", " "},
                new[] {"2", " ", " "},
                new[] {"1", " ", " "},
                new[] {" ", " ", " "},
            });

            snake.Move(Direction.Forward);
            
            AssertState(new[]
            {
                new[] {" ", " ", " "},
                new[] {"3", " ", " "},
                new[] {"2", " ", " "},
                new[] {"1", " ", " "},
            }, snake.GetState());
        }

        private static Snake.Snake CreateSnake(string[][] state)
        {
            var parts = state.SelectMany((subArr, y) => subArr.Select((value, x) => new {x, y, value}))
                .Where(item => item.value != " ")
                .OrderBy(item => item.value)
                .Select(item => new Vector2(item.x, item.y))
                .ToList();

            return new(parts);
        }

        private static void AssertState(string[][] expected, IList<Vector2> actual)
        {
            var actualMatrix = new string[expected.Length][];
            for (int i = 0; i < expected.Length; i++)
            {
                actualMatrix[i] = Enumerable.Repeat(" ", expected[i].Length).ToArray();
            }


            foreach (var part in actual)
            {
                actualMatrix[(int) part.Y][(int) part.X] = (actual.IndexOf(part) + 1).ToString();
            }

            Assert.Equal(expected, actualMatrix);
        }
    }
}