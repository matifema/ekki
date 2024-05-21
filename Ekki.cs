using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mygame
{
    public class Ekki : Game
    {
        private RenderTarget2D _renderTarget;
        private int _targetHeight = 144*2, _targetWidth = 160*2;
        private int _scale = 4;
        private Rectangle _actualScreenRectangle;

        public Ekki()
        {
            Globals.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            

            // setting backbuffer to desired scaled size
            Globals.graphics.PreferredBackBufferWidth = _targetWidth * _scale;
            Globals.graphics.PreferredBackBufferHeight = _targetHeight * _scale;
        }

        protected override void Initialize()
        {
            // runs once at startup

            // game screen render target
            _renderTarget = new RenderTarget2D(GraphicsDevice, _targetWidth, _targetHeight);

            // target rectagle, scaled
            _actualScreenRectangle = new Rectangle(x: 0, y: 0, width: _scale * _targetWidth, height: _scale * _targetHeight);

            Globals.Content = Content;

            // inizializzo isIngame a false poi quando carico world true
            Globals.isIngame = true;

            //TODO REMOVE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Globals.World = new World();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            // loading font
            Globals.gameFont = Content.Load<SpriteFont>("galleryFont");
            
            //TODO REMOVE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Globals.World.LoadAssets();
        }

        protected override void Update(GameTime gameTime)
        {
            // runs at every frame. monogame usa 60fps di default.

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
                ToggleFullScreen();

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                ToggleWorldEdit();

            if (Globals.isIngame)
            {
                Globals.World.WorldUpdate();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // runs once every frame, again 60 times per second.
            // should not change variables or calculation. only drawing
            // the order is important!!

            // setting the rendertarget to the 2D rendertexture 
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.White);
            
            // drawing game
            Globals.SpriteBatch.Begin();

                // TODO REMOVE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                Globals.World._worldMap.Draw();
                Globals.World._player.Draw(gameTime);

            Globals.SpriteBatch.End();

            // rendering target to backbuffer
            GraphicsDevice.SetRenderTarget(null); // switching to backbuffer

            // drawing rendertarget as a texture
            Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Globals.SpriteBatch.Draw(_renderTarget, _actualScreenRectangle, Color.White);
            Globals.SpriteBatch.End();

            Globals.renderedFrames++;
            base.Draw(gameTime);
        }

        private void ToggleWorldEdit()
        {
            
        }
        void ToggleFullScreen()
        {
            if (!Globals.graphics.IsFullScreen)
            {
                Globals.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Globals.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                Globals.graphics.PreferredBackBufferWidth = _targetWidth * _scale;
                Globals.graphics.PreferredBackBufferHeight = _targetHeight * _scale;
            }
            Globals.graphics.IsFullScreen = !Globals.graphics.IsFullScreen;
            Globals.graphics.ApplyChanges();

            _actualScreenRectangle = GetRenderTargetDestination(new Point(_targetWidth, _targetHeight), Globals.graphics.PreferredBackBufferWidth, Globals.graphics.PreferredBackBufferHeight);
        }

        Rectangle GetRenderTargetDestination(Point resolution, int preferredBackBufferWidth, int preferredBackBufferHeight)
        {
            float resolutionRatio = (float)resolution.X / resolution.Y;
            float screenRatio;
            Point bounds = new Point(preferredBackBufferWidth, preferredBackBufferHeight);
            screenRatio = (float)bounds.X / bounds.Y;
            float scale;
            Rectangle rectangle = new Rectangle();

            if (resolutionRatio < screenRatio)
                scale = (float)bounds.Y / resolution.Y;
            else if (resolutionRatio > screenRatio)
                scale = (float)bounds.X / resolution.X;
            else
            {
                // Resolution and window/screen share aspect ratio
                rectangle.Size = bounds;
                return rectangle;
            }
            rectangle.Width = (int)(resolution.X * scale);
            rectangle.Height = (int)(resolution.Y * scale);
            return CenterRectangle(new Rectangle(Point.Zero, bounds), rectangle);
        }

        static Rectangle CenterRectangle(Rectangle outerRectangle, Rectangle innerRectangle)
        {
            Point delta = outerRectangle.Center - innerRectangle.Center;
            innerRectangle.Offset(delta);
            return innerRectangle;
        }
    }
}
