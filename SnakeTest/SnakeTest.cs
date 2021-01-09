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
        public void BecomesLongerWhenEats()
        {
            var snake = CreateSnake(new[]
            {
                new[] {" ", " ", " "},
                new[] {"2", "1", " "},
                new[] {" ", " ", " "},
            });

            snake.Eat();
        
            AssertState(new[]
            {
                new[] {" ", " ", " "},
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
            }, snake.GetState());
        }
        
        [Fact]
        public void MovesUpAndForward()
        {
            var snake = CreateSnake(new[]
            {
                new[] {" ", " ", " "},
                new[] {" ", " ", " "},
                new[] {"3", "2", "1"},
            });
        
            snake.Turn(Direction.Up);
            snake.Move();
            snake.Move();
        
            AssertState(new[]
            {
                new[] {" ", " ", "1"},
                new[] {" ", " ", "2"},
                new[] {" ", " ", "3"},
            }, snake.GetState());
        }

        [Fact]
        public void IgnoresTurnToNextToHeadDirection()
        {
            var snake = CreateSnake(new[]
            {
                new[] {" ", " ", " "},
                new[] {" ", " ", " "},
                new[] {"3", "2", "1"},
            });
        
            snake.Turn(Direction.Up);
            snake.Turn(Direction.Left);
            snake.Move();
        
            AssertState(new[]
            {
                new[] {" ", " ", " "},
                new[] {" ", " ", "1"},
                new[] {" ", "3", "2"},
            }, snake.GetState());
        }

        [Theory]
        [MemberData(nameof(SingleMovesData))]
        public void SingleMoves(string _, string[][] state, string[][] expected)
        {
            var snake = CreateSnake(state);

            snake.Move();
            
            AssertState(expected, snake.GetState());
        }

        [Theory]
        [MemberData(nameof(TurnsData))]
        public void Turns(Direction direction, string[][] state, string[][] expected)
        {
            var snake = CreateSnake(state);

            snake.Turn(direction);
            snake.Move();
            
            AssertState(expected, snake.GetState());
        }

        public static IEnumerable<object[]> SingleMovesData =>
            new List<object[]>
            {
                new object[]
                {
                    "forward vertically down",
                    new[]
                    {
                        new[] {"3", " ", " "},
                        new[] {"2", " ", " "},
                        new[] {"1", " ", " "},
                        new[] {" ", " ", " "},
                    },
                    new[]
                    {
                        new[] {" ", " ", " "},
                        new[] {"3", " ", " "},
                        new[] {"2", " ", " "},
                        new[] {"1", " ", " "},
                    },
                },
                new object[]
                {
                    "forward vertically up",
                    new[]
                    {
                        new[] {" ", " ", " "},
                        new[] {"1", " ", " "},
                        new[] {"2", " ", " "},
                        new[] {"3", " ", " "},
                    },
                    new[]
                    {
                        new[] {"1", " ", " "},
                        new[] {"2", " ", " "},
                        new[] {"3", " ", " "},
                        new[] {" ", " ", " "},
                    },
                },
                new object[]
                {
                    "forward horizontally left",
                    new[]
                    {
                        new[] {" ", " ", " ", " "},
                        new[] {" ", "1", "2", "3"},
                        new[] {" ", " ", " ", " "},
                    },
                    new[]
                    {
                        new[] {" ", " ", " ", " "},
                        new[] {"1", "2", "3", " "},
                        new[] {" ", " ", " ", " "},
                    },
                },
                new object[]
                {
                    "forward horizontally right",
                    new[]
                    {
                        new[] {" ", " ", " ", " "},
                        new[] {"3", "2", "1", " "},
                        new[] {" ", " ", " ", " "},
                    },
                    new[]
                    {
                        new[] {" ", " ", " ", " "},
                        new[] {" ", "3", "2", "1"},
                        new[] {" ", " ", " ", " "},
                    },
                },
            };
        
        public static IEnumerable<object[]> TurnsData =>
            new List<object[]>
            {
                new object[]
                {
                    Direction.Left,
                    new[]
                    {
                        new[] {" ", "3", " "},
                        new[] {" ", "2", " "},
                        new[] {" ", "1", " "},
                    },
                    new[]
                    {
                        new[] {" ", " ", " "},
                        new[] {" ", "3", " "},
                        new[] {"1", "2", " "},
                    },
                },
                new object[]
                {
                    Direction.Right,
                    new[]
                    {
                        new[] {"3", " ", " "},
                        new[] {"2", " ", " "},
                        new[] {"1", " ", " "},
                    },
                    new[]
                    {
                        new[] {" ", " ", " "},
                        new[] {"3", " ", " "},
                        new[] {"2", "1", " "},
                    },
                },
                new object[]
                {
                    Direction.Down,
                    new[]
                    {
                        new[] {"3", "2", "1"},
                        new[] {" ", " ", " "},
                        new[] {" ", " ", " "},
                    },
                    new[]
                    {
                        new[] {" ", "3", "2"},
                        new[] {" ", " ", "1"},
                        new[] {" ", " ", " "},
                    },
                },
                new object[]
                {
                    Direction.Up,
                    new[]
                    {
                        new[] {" ", " ", " "},
                        new[] {"3", "2", "1"},
                        new[] {" ", " ", " "},
                    },
                    new[]
                    {
                        new[] {" ", " ", "1"},
                        new[] {" ", "3", "2"},
                        new[] {" ", " ", " "},
                    },
                },
            };
        
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