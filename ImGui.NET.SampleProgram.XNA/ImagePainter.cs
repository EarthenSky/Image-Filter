using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImGuiNET.ImageFilter
{
    public class ImagePainter
    {
        public string filename = "No file opened";  // TODO: draw this above the image.

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;  // For drawing.

        private Texture2D _mainImage;  // This is the image that is being edited.
        private Texture2D _filteredImage; // This is the image with a mask on it.

        public bool isFilteredImage = false;

        // Needs a reference to the graphics device for image loading / drawing.
        public ImagePainter(ref GraphicsDeviceManager graphicsDevice) {
            _graphics = graphicsDevice;
        }

        // For file outputing.
        public Texture2D GetFilteredImage(ComponentManager componentManager) {
            UpdateFilteredImage(componentManager);  // Update filtered image first.
            return _filteredImage;
        }

        // Runs component code.  
        // TODO: only show a 256x256 image and apply the change to it when pressing update.  When saving do entire image.
        public void UpdateFilteredImage(ComponentManager componentManager) {
            if (_mainImage == null) { Console.WriteLine("Error: Please open an image before updating it."); return; }  // Check for existance of image to do modification on.

            // Reset filtered image.
            if (_filteredImage != null) { _filteredImage.Dispose(); }  // Do I need this?
            _filteredImage = new Texture2D(_graphics.GraphicsDevice, _mainImage.Width, _mainImage.Height);

            // Get initial color data from the main image. (the Original)
            Color[] colorData = new Color[_mainImage.Width * _mainImage.Height];
            _mainImage.GetData<Color>(colorData);

            componentManager.IterateComponents(ref colorData, new Vector2(_mainImage.Width, _mainImage.Height));

            _filteredImage.SetData<Color>(colorData);

            colorData = null;  // This tells the GC to deallocate this object.
        }

        // Inits spritebatch.
        public void LoadContent() {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
        }

        // Returns 1 for failed and 0 for success.
        public int LoadImage(string filepath)
        {
            try {
                // "using" should close the filestream.
                using (FileStream fileStream = new FileStream(filepath, FileMode.Open))
                {
                    _mainImage = Texture2D.FromStream(_graphics.GraphicsDevice, fileStream);
                }
                return 0;
            }
            catch { return 1; } 
        }

        public void DrawImage()
        {
            _spriteBatch.Begin();

            //TODO: positioning and control.
            if (!isFilteredImage) {
                if (_mainImage != null) {
                    _spriteBatch.Draw(_mainImage, new Rectangle(8, 8, _mainImage.Width, _mainImage.Height), Color.White);
                } 
            } else {
                if (_filteredImage != null) {
                    _spriteBatch.Draw(_filteredImage, new Rectangle(8, 8, _filteredImage.Width, _filteredImage.Height), Color.White);
                }
            }

            _spriteBatch.End();
        }
    }
}
