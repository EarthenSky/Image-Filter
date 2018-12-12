using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For filesystem commands.
using System.IO;

// For textures.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImGuiNET.ImageFilter
{
    // There only needs to be one instance of this class.
    public static class ImageWriter
    {
        // Include extension in filename (only .png?)
        public static void WriteImage(string filename, Texture2D texture) {
            //TODO: check for good path.  // DOESN'T WORK.
            Stream stream = File.Create(filename);
            if (stream != null) {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            } else {
                Console.WriteLine("Failed to save image.");
            }

            stream.Dispose();

        }
    }
}
