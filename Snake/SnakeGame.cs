using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;

namespace Snake
{
    public class SnakeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Snake2DRenderer _snakeRenderer;
        private Snake _snake;
        private TimeSpan _timeSinceLastMove = TimeSpan.Zero;
        private bool _isPaused = false;

        public SnakeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _snake = new Snake(new List<Vector2>(new[]
            {
                new Vector2(5, 0),
                new Vector2(4, 0),
                new Vector2(3, 0),
                new Vector2(2, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
            }));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _snakeRenderer = new Snake2DRenderer(Content.Load<Texture2D>("pig"), _spriteBatch);
        }

        protected override void Update(GameTime time)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            HandleInput();

            if (!_isPaused)
            {
                _timeSinceLastMove += time.ElapsedGameTime;
                if (_timeSinceLastMove >= TimeSpan.FromSeconds(1))
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
            _spriteBatch.End();

            base.Draw(time);
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
