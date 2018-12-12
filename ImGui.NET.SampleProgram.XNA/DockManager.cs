using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;  // For Vector2.

namespace ImGuiNET.ImageFilter
{
    public class DockManager
    {
        private Vector2 _initialDockPosition;
        private Vector2 _initialDockSize;
        private bool _isFirstDraw = true;

        public DockManager() {
            _initialDockPosition = new Vector2(8, 8);
            _initialDockSize = new Vector2(64 * 5, 64 * 10);
        }

        /// <summary> This function is called for the first draw frame </summary>  
        private void DrawInit() {
            ImGui.SetNextWindowPos(_initialDockPosition);
            ImGui.SetNextWindowSize(_initialDockSize);
            _isFirstDraw = false;
        }

        /// <summary> This function Draws the dock and components </summary>  
        public void Draw(ComponentManager componentManager, ComponentLoader componentLoader, ImagePainter imagePainter)
        {
            if (_isFirstDraw == true) { DrawInit(); }

            ImGui.Begin("Component Dock", /*ImGuiWindowFlags.MenuBar |*/ ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
            ImGui.Checkbox("Show filtered image", ref imagePainter.isFilteredImage);

            componentManager.DrawComponents(componentLoader, imagePainter);  // This function is from the Component Manager.

            // The bottom ui button.
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 64);
            ImGui.Spacing(); ImGui.Spacing(); ImGui.Indent(64+16+8);
            if (ImGui.Button("Update Image", new Vector2(128, 48))) {
                imagePainter.UpdateFilteredImage(componentManager);
            }

            ImGui.PopStyleVar();
            ImGui.End();
        }

    }
}
