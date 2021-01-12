using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaGame = Microsoft.Xna.Framework.Game;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeRules;
using Point = SnakeRules.Point;
using XnaPoint = Microsoft.Xna.Framework.Point;
using Vector2Numeric = System.Numerics.Vector2;

namespace SnakeGame
{
    public class Game : XnaGame
    {
        private const int GridSize = 32;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Snake2DRenderer _snakeRenderer;
        private Snake _snake;
        private TimeSpan _timeSinceLastMove = TimeSpan.Zero;
        private bool _isPaused;
        private Texture2D _texture1Px;
        private int _width;
        private int _height;
        private int _columns;
        private int _rows;
        private KeyboardState _previousState;
        private SpriteFont _gameOverFont;
        private bool _isSnakeDead;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _previousState = Keyboard.GetState();

            _width = _graphics.GraphicsDevice.Viewport.Width;
            _height = _graphics.GraphicsDevice.Viewport.Height;
            _columns = _width / GridSize;
            _rows = _height / GridSize;

            Console.WriteLine("Width: " + _width);
            Console.WriteLine("Height: " + _height);
            Console.WriteLine("Columns(x): " + _columns);
            Console.WriteLine("Row(y): " + _rows);
            Console.WriteLine("");
            
            _snake = CreateSnake();
        }

        private Snake CreateSnake()
        {
            Console.WriteLine("Snake is created.");
            var snake = new Snake(new List<Point>(new[]
                {
                    new Point(5, 0),
                    new Point(4, 0),
                    new Point(3, 0),
                    new Point(2, 0),
                    new Point(1, 0),
                    new Point(0, 0),
                }),
                new Size(_columns, _rows), new List<Point>());
            snake.Died += () => Console.WriteLine("Snake is dead.");
            snake.Died += () => {_isSnakeDead = true;};

            return snake;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _snakeRenderer = new Snake2DRenderer(Content.Load<Texture2D>("pig"), _spriteBatch);
            _texture1Px = new Texture2D(GraphicsDevice, 1, 1);
            _texture1Px.SetData(new[] {Color.White});
            _gameOverFont = Content.Load<SpriteFont>("game_over");
        }

        protected override void Update(GameTime time)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandleInput();

            if (!_isPaused)
            {
                _timeSinceLastMove += time.ElapsedGameTime;
                if (_timeSinceLastMove >= TimeSpan.FromMilliseconds(250))
                {
                    _snake.Move();
                    _timeSinceLastMove = TimeSpan.Zero;
                }
            }

            base.Update(time);
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _snake.Render(_snakeRenderer);
            DrawGrid();
            DrawGameOver();
            _spriteBatch.End();

            base.Draw(time);
        }

        private void DrawGameOver()
        {
            if (!_isSnakeDead) return;

            const string text = "Game over";
            var size = _gameOverFont.MeasureString(text);
            var center = new Vector2((float) _width / 2, (float) _height / 2);
            _spriteBatch.DrawString(_gameOverFont, text, center - size / 2, Color.Red);
        }

        private void DrawGrid()
        {
            if (Environment.GetEnvironmentVariable("GRID_ENABLED") == null)
            {
                return;
            }
            
            var gridColor = Color.Salmon;

            for (var x = 0; x < _columns; x++)
            {
                var rectangle = new Rectangle(new XnaPoint(x * GridSize, 0), new XnaPoint(1, _height));
                _spriteBatch.Draw(_texture1Px, rectangle, gridColor);
            }

            for (var y = 0; y < _rows; y++)
            {
                var rectangle = new Rectangle(new XnaPoint(0, y * GridSize), new XnaPoint(_width, 1));
                _spriteBatch.Draw(_texture1Px, rectangle, gridColor);
            }
        }

        private void HandleInput()
        {
            var currentState = Keyboard.GetState();
            if (!_isPaused)
            {
                if (currentState.IsKeyDown(Keys.Up))
                {
                    _snake.Turn(Direction.Up);
                }

                if (currentState.IsKeyDown(Keys.Down))
                {
                    _snake.Turn(Direction.Down);
                }

                if (currentState.IsKeyDown(Keys.Left))
                {
                    _snake.Turn(Direction.Left);
                }

                if (currentState.IsKeyDown(Keys.Right))
                {
                    _snake.Turn(Direction.Right);
                }
            }

            if (currentState.IsKeyDown(Keys.Space) && !_previousState.IsKeyDown(Keys.Space))
            {
                _isPaused = !_isPaused;
            }

            if (currentState.IsKeyDown(Keys.R) && !_previousState.IsKeyDown(Keys.R))
            {
                Restart();
            }

            _previousState = currentState;
        }

        private void Restart()
        {
            _isSnakeDead = false;
            _snake = CreateSnake();
        }
    }
}