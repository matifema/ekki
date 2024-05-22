using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace mygame
{
    public class Ekki : Game
    {
        private World _testWorld;
        private RenderTarget2D _renderTarget;
        private KeyboardState _previousKeypress;
        private Rectangle _actualScreenRectangle;
        public List<Component> _renderedComponents = new();
        private int _targetHeight = 288, _targetWidth = 512, _scale = 4;

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

            //TODO maybe remove this
            _testWorld = new();

            Globals.isEditing = false;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // creating spritebatch
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            // loading font
            Globals.gameFont = Content.Load<SpriteFont>("galleryFont");

            //TODO maybe remove this
            _testWorld.LoadAssets();
        }

        protected override void Update(GameTime gameTime)
        {
            // runs at every frame. monogame usa 60fps di default.

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F11) && Keyboard.GetState() != _previousKeypress)
                ToggleFullScreen();

            if (Keyboard.GetState().IsKeyDown(Keys.F1) && Keyboard.GetState() != _previousKeypress)
                ToggleWorldEdit();

            _previousKeypress = Keyboard.GetState();

            //TODO maybe remove this
            _testWorld.WorldUpdate();

            Globals.time = gameTime;
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

            // TODO: draw game components
            foreach (Component component in _renderedComponents)
            {
                component.Draw();
            }
            
            // TODO maybe remove this
            _testWorld._worldMap.Draw();
            _testWorld._player.Draw();
            
            if (Globals.isEditing)
            {
                _testWorld._Editor.Draw();
            }
            
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

        protected private void ToggleWorldEdit()
        {
            Globals.mousePosition = Mouse.GetState().Position;
            Globals.isEditing = !Globals.isEditing;
        }
        
        protected void ToggleFullScreen()
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

        protected Rectangle GetRenderTargetDestination(Point resolution, int preferredBackBufferWidth, int preferredBackBufferHeight)
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

        protected static Rectangle CenterRectangle(Rectangle outerRectangle, Rectangle innerRectangle)
        {
            Point delta = outerRectangle.Center - innerRectangle.Center;
            innerRectangle.Offset(delta);
            return innerRectangle;
        }
    
    }
}
