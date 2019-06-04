using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Views;
using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Demo {
    enum WindowMode {Windowed, Fullscreen}

    public class DemoGame : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ViewManager viewManager;

        public DemoGame() {
            graphics = new GraphicsDeviceManager(this);
            viewManager = new ViewManager(Content, graphics);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            graphics.HardwareModeSwitch = false; // never forget -.-
            SetWindowMode(WindowMode.Fullscreen);
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var mainPane = Content.Load<StackPane>("layout/stp_home");
            viewManager.Add(new DemoView(mainPane));
        }

        protected override void Update(GameTime gameTime) {
            if(viewManager.Views.Count == 0)
                Exit();
            
            viewManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            viewManager.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        private void SetWindowMode(WindowMode mode) {
            if(mode == WindowMode.Fullscreen
            && !graphics.IsFullScreen) {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.ApplyChanges();
            } else
            if(mode == WindowMode.Windowed
            && graphics.IsFullScreen) {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 600;
                graphics.ApplyChanges();
            }
        }
    }
}
