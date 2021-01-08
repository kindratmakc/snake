using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    public class OldSnake
    {
        private const int Step = 32;
        private static readonly Rectangle Head = new Rectangle(0, 0, Step, Step);
        private static readonly Rectangle Body = new Rectangle(Step, 0, Step, Step);
        private static readonly Rectangle Tail = new Rectangle(Step * 2, 0, Step, Step);

        private Texture2D _texture;

        private Vector2 _velocity = new Vector2(Step, 0);
        private Vector2 _position = Vector2.Zero;
        
        private TimeSpan _timeSinceLastMove = TimeSpan.Zero;

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("snake");
        }

        public void Update(GameTime time)
        {
            HandleInput();
            Move(time);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _position, Head, Color.White);
            batch.Draw(_texture, new Vector2(_position.X, _position.Y + Step), Body, Color.White);
            batch.Draw(_texture, new Vector2(_position.X, _position.Y + Step * 2), Tail, Color.White);
        }

        private void HandleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _velocity = new Vector2(0, -1 * Step);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _velocity = new Vector2(0, 1 * Step);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _velocity = new Vector2(-1 * Step, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _velocity = new Vector2(1 * Step, 0);
            }
        }

        private void Move(GameTime time)
        {
            _timeSinceLastMove += time.ElapsedGameTime;
            if (_timeSinceLastMove <= TimeSpan.FromSeconds(1)) return;
            _position += _velocity;
            _timeSinceLastMove = TimeSpan.Zero;
        }
    }
}