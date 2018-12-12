using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;  // For Vector2.  

// iron python stuff.
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using IronPython.Runtime.Types;
using IronPython.Modules.Bz2;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImGuiNET.ImageFilter
{
    public class ComponentManager
    {
        private List<Component> _componentList = new List<Component>();
        private string _inputPath = "";

        private bool _pathIsError = false;
        private int pathErrorCounter = 0;  // Timer.

        private int _componentHeight = 96;

        // The sub menu gives you options about components.
        public void DrawSubMenu(ComponentLoader componentLoader, ImagePainter imagePainter)
        {
            if (ImGui.BeginMenuBar()) {
                if (ImGui.BeginMenu("Component Bar")) {
                    if (ImGui.BeginMenu("Open Image")) {

                        ImGui.Text("image filepath: ");
                        ImGui.InputText("##hidelabel", ref _inputPath, 256);  //TODO: make number larger?

                        if (ImGui.Button("Load Image")) {
                            if (imagePainter.LoadImage(_inputPath) == 1) {
                                _pathIsError = true;
                                pathErrorCounter = 50;
                            }
                        }

                        if (pathErrorCounter <= 0) { _pathIsError = false; }

                        if (_pathIsError == true) { ImGui.Text("Error loading file at path."); }  // Error message.

                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Save Image"))
                    {
                        ImGui.Text("output filepath: ");
                        ImGui.InputText("##hidelabel", ref imagePainter.filename, 256);  //TODO: make number larger?

                        if (ImGui.Button("Save Image")) {
                            ImageWriter.WriteImage(imagePainter.filename, imagePainter.GetFilteredImage(this));  // TODO: CHECK THIS.
                        }

                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Add Component")) {
                        // All component names are printed here.
                        foreach (string name in componentLoader.GetComponentNames()) {
                            if (ImGui.MenuItem(name)) {
                                componentLoader.AddComponent(name, _componentList);
                                // Get component information from component loader and add component to the list.
                            }
                        }

                        ImGui.EndMenu();
                    }

                    if (ImGui.MenuItem("Reload Components")) {
                        Console.WriteLine("Reloading components.");
                        componentLoader.ReadComponentList();

                        //ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Set Component Size")) {
                        if (ImGui.MenuItem("Small")) { _componentHeight = 64; }
                        if (ImGui.MenuItem("Medium")) { _componentHeight = 96; }
                        if (ImGui.MenuItem("Large")) { _componentHeight = 128; }
                        ImGui.EndMenu();
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }

        public void Update() {
            if (pathErrorCounter > 0)
                pathErrorCounter--;

            // Check for a garbage component. (only ever one per loop)
            foreach (Component c in _componentList) {
                if (c.GetIsDead() == true) {
                    _componentList.Remove(c);
                    break;  // Exit loop so no mess-up.
                }
            }
        }

        /// <summary> This function Draws the component boxes </summary>  
        public void DrawComponents(ComponentLoader componentLoader, ImagePainter imagePainter)
        {
            ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 1.0f);
            ImGui.BeginChild("Component Manager", new System.Numerics.Vector2(64*5-16, 64*8), true, ImGuiWindowFlags.AlwaysVerticalScrollbar | ImGuiWindowFlags.MenuBar);
            ImGui.PopStyleVar();

            DrawSubMenu(componentLoader, imagePainter);

            // Draw the components.
            foreach (Component c in _componentList) {
                c.Draw(_componentHeight);
            }

            ImGui.EndChild(); 

            // ImGui.ShowDemoWindow();
        }

        /// <summary> This function draws remove a component from the list based on it's id. </summary>  
        public void RemoveComponent(Component comp) {
            _componentList.Remove(comp);
        }

        /// <summary> This function calls the code functions in all the components. </summary>  
        public void IterateComponents(ref Color[] colorData, Microsoft.Xna.Framework.Vector2 imgSize) {
            // Convert color to generic data type first.
            IronPython.Runtime.List genericData = new IronPython.Runtime.List();  // int of size 4.
            foreach (Color c in colorData) {
                genericData.append( new IronPython.Runtime.List { c.R, c.G, c.B, c.A } );
            }         

            Console.WriteLine("to-array done");

            ObjectOperations op = ComponentLoader.s_ironPyEngine.Operations;

            // Pass generic datatype to python scripts.
            foreach (Component c in _componentList) {
                if (c.disabled == false) {  // Also passes image size.
                    //Console.WriteLine(op.Invoke(c.Run, genericData, c.GetComponentValueList(), imgSize.X, imgSize.Y)[0][2]);
                    genericData = op.Invoke( c.Run, genericData, c.GetComponentValueList(), imgSize.X, imgSize.Y);  
                }
            }

            Console.WriteLine("components done");

            // Convert generic data type to Color[]
            int i = 0;
            foreach (dynamic ca in genericData) {
                colorData[i] = new Color((int)ca[0], (int)ca[1], (int)ca[2], (int)ca[3]);
                i++;
            }

            Console.WriteLine("to-colour done");
        }
    }
}
