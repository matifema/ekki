using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace mygame
{
    public class Ekki : Game
    {
        private RenderTarget2D _renderTarget;
        private KeyboardState _previousKeypress;
        private Rectangle _actualScreenRectangle;
        public List<Component> _renderedComponents = new();
        private int _targetHeight = 288, _targetWidth = 512, _scale = 4;
        private WorldEditor _Editor;

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

            // set some global variables
            Globals.Content = Content;
            Globals.isEditing = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // creating spritebatch
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);

            // loading font
            Globals.gameFont = Content.Load<SpriteFont>("galleryFont");

            // loading sprites
            Globals.LoadSprites("GrassHills");
            Globals.LoadSprites("Lake_Tileset");
            Globals.LoadSprites("Props");

            // initiating scene at spawn
            Globals.CurrentScene = new Scene
            (
                new Map("Spawn")
            );

            Globals.CurrentScene.SpawnEntity(new Player(), new Vector2(16*16,8*16));
        }

        protected override void Update(GameTime gameTime)
        {
            // runs at every frame. monogame usa 60fps di default.

            // checking for fullscreen key
            if (Keyboard.GetState().IsKeyDown(Keys.F11) && Keyboard.GetState() != _previousKeypress)
                ToggleFullScreen();

            // checking for map editor key
            if (Keyboard.GetState().IsKeyDown(Keys.F1) && Keyboard.GetState() != _previousKeypress)
                ToggleWorldEdit();

            _previousKeypress = Keyboard.GetState();

            // update loop
            foreach (Component component in _renderedComponents)
            {
                component.Update();
            }

            // scene update
            Globals.CurrentScene.Update();

            // gametime update
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
            GraphicsDevice.Clear(new Color(79, 79, 79));


            // drawing game
            Globals.SpriteBatch.Begin();

            // draw scene
            Globals.CurrentScene.Draw();

            // draw loop
            foreach (Component component in _renderedComponents)
            {
                component.Draw();
            }

            Globals.SpriteBatch.End();

            // rendering target to backbuffer
            GraphicsDevice.SetRenderTarget(null); // switching to backbuffer

            // drawing rendertarget as a texture
            Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Globals.SpriteBatch.Draw(_renderTarget, _actualScreenRectangle, Color.White);
            Globals.SpriteBatch.End();

            // keeping count of frames
            Globals.renderedFrames++;

            base.Draw(gameTime);
        }

        protected private void ToggleWorldEdit()
        {
            Globals.isEditing = !Globals.isEditing;

            // adds/removes the editor from renders
            if (Globals.isEditing)
            {
                _Editor = new(Globals.LoadedSprites);
                _renderedComponents.Add(_Editor);
            }
            else
            {
                _renderedComponents.Remove(_Editor);
            }
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
