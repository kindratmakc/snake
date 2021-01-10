using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2Numeric = System.Numerics.Vector2;

namespace Snake
{
    public class SnakeGame : Game
    {
        private const int GridSize = 32;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Snake2DRenderer _snakeRenderer;
        private Snake _snake;
        private TimeSpan _timeSinceLastMove = TimeSpan.Zero;
        private bool _isPaused;
        private Texture2D _texture1Px;

        public SnakeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _snake = new Snake(new List<Vector2Numeric>(new[]
            {
                new Vector2Numeric(5, 0),
                new Vector2Numeric(4, 0),
                new Vector2Numeric(3, 0),
                new Vector2Numeric(2, 0),
                new Vector2Numeric(1, 0),
                new Vector2Numeric(0, 0),
            }));

            base.Initialize();
            Console.WriteLine(_graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
            Console.WriteLine(_graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            Console.WriteLine("");
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _snakeRenderer = new Snake2DRenderer(Content.Load<Texture2D>("pig"), _spriteBatch);
            _texture1Px = new Texture2D(GraphicsDevice, 1, 1);
            _texture1Px.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime time)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            HandleInput();

            if (!_isPaused)
            {
                _timeSinceLastMove += time.ElapsedGameTime;
                if (_timeSinceLastMove >= TimeSpan.FromMilliseconds(1000))
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

            if (Environment.GetEnvironmentVariable("GRID_ENABLED") != null)
            {
                DrawGrid(_spriteBatch);
            }
            
            _spriteBatch.End();

            base.Draw(time);
        }

        private void DrawGrid(SpriteBatch batch)
        {
            var gridColor = Color.Salmon;
            var width = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            var height = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            var columns = width / GridSize;
            var rows = height / GridSize;

            for (var x = 0; x < columns; x++)
            {
                var rectangle = new Rectangle(new Point(x * GridSize, 0), new Point(1, height));
                batch.Draw(_texture1Px, rectangle, gridColor);
            }

            for (var y = 0; y < rows; y++)
            {
                var rectangle = new Rectangle(new Point(0, y * GridSize), new Point(width, 1));
                batch.Draw(_texture1Px, rectangle, gridColor);
            }
        }

        private void HandleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _snake.Turn(Direction.Up);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _snake.Turn(Direction.Down);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _snake.Turn(Direction.Left);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _snake.Turn(Direction.Right);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _isPaused = !_isPaused;
            }
        }
    }
}
