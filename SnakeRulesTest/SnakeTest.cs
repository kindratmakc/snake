using System.Collections.Generic;
using System.Linq;
using SnakeRules;
using Xunit;

namespace SnakeRulesTest
{
    public class SnakeTest
    {
        [Fact]
        public void CantMoveIfDead()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"F", " ", " "},
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
            });

            snake.Move();
            snake.Turn(Direction.Down);
            snake.Move();

            AssertState(new[]
            {
                new[] {"F", " ", " "},
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
            }, snake);
        }

        [Fact]
        public void CanChaseOwnTail()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"F", "1", "2"},
                new[] {" ", "4", "3"},
                new[] {" ", " ", " "},
            });

            snake.Turn(Direction.Down);
            snake.Move();

            AssertState(new[]
            {
                new[] {"F", "2", "3"},
                new[] {" ", "1", "4"},
                new[] {" ", " ", " "},
            }, snake);
        }

        [Fact]
        public void DiesWhenCollidesWithTrail()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"F", "1", "2"},
                new[] {"5", "4", "3"},
                new[] {" ", " ", " "},
            });

            snake.Turn(Direction.Down);
            snake.Move();

            AssertState(new[]
            {
                new[] {"F", "1", "2"},
                new[] {"5", "4", "3"},
                new[] {" ", " ", " "},
            }, snake);
            Assert.True(snake.IsDead());
        }

        [Fact]
        public void DiesWhenCollidesWithBorders()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"F", " ", " "},
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
            });

            snake.Move();

            AssertState(new[]
            {
                new[] {"F", " ", " "},
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
            }, snake);
            Assert.True(snake.IsDead());
        }

        [Fact]
        public void BecomesLongerWhenEats()
        {
            var snake = CreateSnake(new[]
            {
                new[] {" ", " ", " "},
                new[] {"2", "1", "F"},
                new[] {" ", " ", " "},
            });

            snake.Move();

            AssertState(new[]
            {
                new[] {" ", " ", " "},
                new[] {"3", "2", "1"},
                new[] {" ", " ", " "},
            }, snake);
        }

        [Fact]
        public void FoodIsDisappearedWhenEaten()
        {
            var snake = CreateSnake(new[]
            {
                new[] {" ", " ", " ", " ", " ", " "},
                new[] {"2", "1", "F", " ", " ", " "},
                new[] {" ", " ", " ", " ", " ", " "},
            });

            snake.Move();
            snake.Move();
            snake.Move();
            snake.Move();

            AssertState(new[]
            {
                new[] {" ", " ", " ", " ", " ", " "},
                new[] {" ", " ", " ", "3", "2", "1"},
                new[] {" ", " ", " ", " ", " ", " "},
            }, snake);
        }

        [Fact]
        public void MovesUpAndForward()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"F", " ", " "},
                new[] {" ", " ", " "},
                new[] {"3", "2", "1"},
            });

            snake.Turn(Direction.Up);
            snake.Move();
            snake.Move();

            AssertState(new[]
            {
                new[] {"F", " ", "1"},
                new[] {" ", " ", "2"},
                new[] {" ", " ", "3"},
            }, snake);
        }

        [Fact]
        public void IgnoresTurnToNextToHeadDirection()
        {
            var snake = CreateSnake(new[]
            {
                new[] {"F", " ", " "},
                new[] {" ", " ", " "},
                new[] {"3", "2", "1"},
            });

            snake.Turn(Direction.Up);
            snake.Turn(Direction.Left);
            snake.Move();

            AssertState(new[]
            {
                new[] {"F", " ", " "},
                new[] {" ", " ", "1"},
                new[] {" ", "3", "2"},
            }, snake);
        }

        [Theory]
        [MemberData(nameof(SingleMovesData))]
        public void SingleMoves(string _, string[][] state, string[][] expected)
        {
            var snake = CreateSnake(state);

            snake.Move();

            AssertState(expected, snake);
        }

        [Theory]
        [MemberData(nameof(TurnsData))]
        public void Turns(Direction direction, string[][] state, string[][] expected)
        {
            var snake = CreateSnake(state);

            snake.Turn(direction);
            snake.Move();

            AssertState(expected, snake);
        }

        public static IEnumerable<object[]> SingleMovesData =>
            new List<object[]>
            {
                new object[]
                {
                    "forward vertically down",
                    new[]
                    {
                        new[] {"F", "3", " "},
                        new[] {" ", "2", " "},
                        new[] {" ", "1", " "},
                        new[] {" ", " ", " "},
                    },
                    new[]
                    {
                        new[] {"F", " ", " "},
                        new[] {" ", "3", " "},
                        new[] {" ", "2", " "},
                        new[] {" ", "1", " "},
                    },
                },
                new object[]
                {
                    "forward vertically up",
                    new[]
                    {
                        new[] {"F", " ", " "},
                        new[] {" ", "1", " "},
                        new[] {" ", "2", " "},
                        new[] {" ", "3", " "},
                    },
                    new[]
                    {
                        new[] {"F", "1", " "},
                        new[] {" ", "2", " "},
                        new[] {" ", "3", " "},
                        new[] {" ", " ", " "},
                    },
                },
                new object[]
                {
                    "forward horizontally left",
                    new[]
                    {
                        new[] {"F", " ", " ", " "},
                        new[] {" ", "1", "2", "3"},
                        new[] {" ", " ", " ", " "},
                    },
                    new[]
                    {
                        new[] {"F", " ", " ", " "},
                        new[] {"1", "2", "3", " "},
                        new[] {" ", " ", " ", " "},
                    },
                },
                new object[]
                {
                    "forward horizontally right",
                    new[]
                    {
                        new[] {"F", " ", " ", " "},
                        new[] {"3", "2", "1", " "},
                        new[] {" ", " ", " ", " "},
                    },
                    new[]
                    {
                        new[] {"F", " ", " ", " "},
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
                        new[] {"F", "3", " "},
                        new[] {" ", "2", " "},
                        new[] {" ", "1", " "},
                    },
                    new[]
                    {
                        new[] {"F", " ", " "},
                        new[] {" ", "3", " "},
                        new[] {"1", "2", " "},
                    },
                },
                new object[]
                {
                    Direction.Right,
                    new[]
                    {
                        new[] {"F", "3", " "},
                        new[] {" ", "2", " "},
                        new[] {" ", "1", " "},
                    },
                    new[]
                    {
                        new[] {"F", " ", " "},
                        new[] {" ", "3", " "},
                        new[] {" ", "2", "1"},
                    },
                },
                new object[]
                {
                    Direction.Down,
                    new[]
                    {
                        new[] {"F", " ", " "},
                        new[] {"3", "2", "1"},
                        new[] {" ", " ", " "},
                    },
                    new[]
                    {
                        new[] {"F", " ", " "},
                        new[] {" ", "3", "2"},
                        new[] {" ", " ", "1"},
                    },
                },
                new object[]
                {
                    Direction.Up,
                    new[]
                    {
                        new[] {"F", " ", " "},
                        new[] {"3", "2", "1"},
                        new[] {" ", " ", " "},
                    },
                    new[]
                    {
                        new[] {"F", " ", "1"},
                        new[] {" ", "3", "2"},
                        new[] {" ", " ", " "},
                    },
                },
            };

        private static Snake CreateSnake(string[][] state)
        {
            var parts = state.SelectMany((subArr, y) => subArr.Select((value, x) => new {x, y, value}))
                .Where(item => item.value != " " && item.value != "F")
                .OrderBy(item => item.value)
                .Select(item => new Point(item.x, item.y))
                .ToList();
            var food = state.SelectMany((subArr, y) => subArr.Select((value, x) => new {x, y, value}))
                .Where(item => item.value == "F")
                .Select(item => new Point(item.x, item.y))
                .FirstOrDefault();
            var columns = state.Select(subArr => subArr.Length).Max();

            return new Snake(parts, new Size(columns, state.Length), food);
        }

        private static void AssertState(string[][] expected, Snake snake)
        {
            var width = expected.Length;
            var height = expected.Select(subArr => subArr.Length).Max();
            var renderer = new MatrixRenderer(width, height);
            snake.Render(renderer, renderer);
            Assert.Equal(expected, renderer.GetMatrix());
        }

        private class MatrixRenderer : ISnakeRenderer, IFoodRenderer
        {
            private readonly string[][] _matrix;
            private int _renderSnakeCalls = 1;

            public MatrixRenderer(int width, int height)
            {
                _matrix = new string[width][];
                for (var i = 0; i < width; i++)
                {
                    _matrix[i] = Enumerable.Repeat(" ", height).ToArray();
                }
            }

            public void RenderHead(Point coordinates, Direction direction)
            {
                RenderNextBodyPart(coordinates);
            }

            public void RenderBody(Point coordinates, Direction toPrev, Direction toNext)
            {
                RenderNextBodyPart(coordinates);
            }

            public void RenderTail(Point coordinates, Direction direction)
            {
                RenderNextBodyPart(coordinates);
            }

            public void RenderFood(Point coordinates)
            {
                _matrix[coordinates.Y][coordinates.X] = "F";
            }

            private void RenderNextBodyPart(Point coordinates)
            {
                _matrix[coordinates.Y][coordinates.X] = (_renderSnakeCalls++).ToString();
            }

            public string[][] GetMatrix()
            {
                return _matrix;
            }
        }
    }
}