using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

// for maximize window.
//using System.Runtime.InteropServices;

namespace ImGuiNET.ImageFilter
{
    /// <summary>
    /// Simple FNA + ImGui example
    /// </summary>
    public class App : Game
    {
        private GraphicsDeviceManager _graphics;
        private ImGuiRenderer _imGuiRenderer;

        private DockManager _dockManager;
        private ComponentManager _componentManager;
        private ComponentLoader _componentLoader;
        private ImagePainter _imagePainter;

        // Pre-initialize componWents.
        public App()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;

            _graphics.PreferMultiSampling = true;
            //_graphics.IsFullScreen = true;

            this.Window.Title = "Image Filter Program";
            this.Window.AllowUserResizing = true;

            IsMouseVisible = true;
        }

        // Initialize components.
        protected override void Initialize()
        {
            _imGuiRenderer = new ImGuiRenderer(this);
            _imGuiRenderer.RebuildFontAtlas();

            // Instantiate Subsystems.
            _componentLoader = new ComponentLoader();
            _dockManager = new DockManager();
            _componentManager = new ComponentManager();
            _imagePainter = new ImagePainter(ref _graphics);  // Send reference to _graphics manager.

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _imagePainter.LoadContent();
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _imagePainter.DrawImage();
            
            _imGuiRenderer.BeforeLayout(gameTime);  // Call BeforeLayout first to set things up
            _dockManager.Draw(_componentManager, _componentLoader, _imagePainter);  // Draw ui.
            _imGuiRenderer.AfterLayout();  // Call AfterLayout to finish up and draw all the things.

            base.Draw(gameTime);

            // Update calls after here:
            _componentManager.Update();
        }
    }
}