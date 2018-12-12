using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// iron python stuff.
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

// directory classes
using System.IO;

using Microsoft.Xna.Framework;

namespace ImGuiNET.ImageFilter
{
    // id values starts at 100.  // Creates an unrepeating id for components.
    public static class ID_GEN {
        public const uint INIT_ID = 100;
        private static uint _idValue = INIT_ID;

        public static uint GetNext() {
            return _idValue++;
        }
    }

    // This only has utilities - see Option.
    public static class OPTION {
        public const int SLIDER_INT     = 1;
        public const int SLIDER_FLOAT   = 2;
        public const int INPUT_TEXT     = 3;
        public const int INPUT_INT      = 4;
        public const int INPUT_FLOAT    = 5;
        public const int TEXT           = 6;
        public const int BOOLEAN        = 7;

        public static Type GetType(int id) {
            if (id == SLIDER_INT || id == INPUT_INT || id == TEXT ) { return typeof(int); }  // Text is here because it doesn't have any value.  Please just set values to 0.
            else if (id == SLIDER_FLOAT || id == INPUT_FLOAT) { return typeof(float); }
            else if (id == INPUT_TEXT) { return typeof(string); }
            else if (id == BOOLEAN) { return typeof(bool); }
            else { return null;  }
        }

        public static void DrawOption(Option option) {
            if (option.m_optionType == OPTION.SLIDER_INT) {
                ImGui.SliderInt(option.m_title+"###"+option.m_id, ref option.m_value_i, (int)option.m_minValue, (int)option.m_maxValue);
            } else if (option.m_optionType == OPTION.SLIDER_FLOAT) {
                ImGui.SliderFloat(option.m_title+"###"+option.m_id, ref option.m_value_f, Convert.ToSingle(option.m_minValue), Convert.ToSingle(option.m_maxValue));
            } else if (option.m_optionType == OPTION.INPUT_INT) {
                ImGui.InputInt(option.m_title+"###"+option.m_id, ref option.m_value_i);
            } else if (option.m_optionType == OPTION.INPUT_FLOAT) {
                ImGui.InputFloat(option.m_title+"###"+option.m_id, ref option.m_value_f);
            } else if (option.m_optionType == OPTION.INPUT_TEXT) {
                ImGui.InputText(option.m_title+"###"+option.m_id, ref option.m_value_s, 512);
            } else if (option.m_optionType == OPTION.TEXT) {
                ImGui.Text(option.m_title);
            } else if (option.m_optionType == OPTION.BOOLEAN) {
                ImGui.Checkbox(option.m_title+"###"+option.m_id, ref option.m_value_b);
            }
        }
    }

    public static class Colour {
        public const int RED = 1;
        public const int GREEN = 2;
        public const int BLUE = 3;
        //public const int YELLOW = 4;
        //public const int ORANGE = 5;
        public const int PURPLE = 6;
        //public const int LIGHT_BLUE = 7;
        public const int GREY = 8;
        public const int LIGHT_GREY = 9;

        public static void PushColour(int colour) {
            if (colour == RED) {
                ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new System.Numerics.Vector4(1.0f, 0.35f, 0.35f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.65f, 0.15f, 0.15f, 0.7f));
            } else if(colour == GREEN) {
                ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new System.Numerics.Vector4(0.3f, 0.8f, 0.3f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.1f, 0.45f, 0.1f, 0.7f));
            } else if (colour == BLUE) {
                ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new System.Numerics.Vector4(0.25f, 0.45f, 1.0f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.05f, 0.15f, 0.65f, 0.7f));
            } else if (colour == PURPLE) {
                ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new System.Numerics.Vector4(0.6f, 0.55f, 0.95f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.3f, 0.25f, 0.60f, 0.7f));
            } else if (colour == GREY) {
                // Do nothing for grey.
            } else if (colour == LIGHT_GREY) {
                ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new System.Numerics.Vector4(0.75f, 0.75f, 0.75f, 0.4f));
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.95f, 0.95f, 0.95f, 0.25f));
            }
        }

        public static void PopColour(int colour) {
            if (colour != GREY)  // Don't pop for grey.
                ImGui.PopStyleColor(2);
        }

        // Draws the colour menu.
        public static void SetMenu( ref int colour ) {
            if (ImGui.MenuItem("RED")) {
                colour = RED;
            }
            if (ImGui.MenuItem("GREEN")) {
                colour = GREEN;
            }
            if (ImGui.MenuItem("BLUE")) {
                colour = BLUE;
            }
            if (ImGui.MenuItem("PURPLE")) {
                colour = PURPLE;
            }
            if (ImGui.MenuItem("GREY")) {
                colour = GREY;
            }
        }
    }

    /// <Summary> This type holds all the information needed to draw a component and save it's properties. </Summary>
    public class Component
    {
        // TODO: add an info section.
        private const int _WIDTH = 64 * 5 - (16 * 3);

        public bool disabled;
        private bool _optionsOpen = false;
        private bool _markedDead = false;

        public uint m_id;
        public string m_componentName;

        private List<Option> _optionsList;
        private int _colour;

        public dynamic Run;  // This holds the function.

        public Component(dynamic Run, string componentName, int colour=Colour.GREY) {
            _optionsList = new List<Option>();
            m_id = ID_GEN.GetNext();
            m_componentName = componentName + (m_id - ID_GEN.INIT_ID);

            _colour = colour;
            _markedDead = false;

            this.Run = Run;
        }

        private void DrawMenu() {
            if (ImGui.BeginMenuBar()) {
                if ( ImGui.BeginMenu(m_componentName) ) {
                    if (ImGui.BeginMenu("Set Colour")) {
                        Colour.SetMenu(ref _colour);
                        ImGui.EndMenu();
                    }

                    if (ImGui.MenuItem("Remove Component")) {
                        _markedDead = true;
                    }

                    ImGui.EndMenu();
                }

                ImGui.Text(_optionsOpen ? "- open" : "");

                ImGui.SameLine(_WIDTH - 118);
                if (disabled) { ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(223f/255f, 68f/255f, 59f/255f, 1f)); }
                else { ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(1f, 1f, 1f, 0.95f)); }
                ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(1f, 1f, 1f, 0f));
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, new System.Numerics.Vector4(1f, 1f, 1f, 0.1f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new System.Numerics.Vector4(1f, 1f, 1f, 0.1f));
                    if (ImGui.Button("Disable")) { disabled = !disabled; }  // Toggle value.
                ImGui.PopStyleColor(4);

                ImGui.SameLine(_WIDTH - 52);
                ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(248f / 255f, 190f / 255f, 60f / 255f, 0.85f));
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, new System.Numerics.Vector4(240f / 255f, 200f / 255f, 75f / 255f, 0.95f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new System.Numerics.Vector4(234f / 255f, 163f / 255f, 36f / 255f, 0.85f));
                ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 4.0f);
                    if (ImGui.Button("Edit")) { _optionsOpen = !_optionsOpen; }  // Toggle value.
                ImGui.PopStyleVar();
                ImGui.PopStyleColor(3);

                ImGui.EndMenuBar();
            }
        }
      
        // Draw the options window (free moving.)
        private void DrawOptionsWindow() {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(64 * 6, 64 * 4));
            ImGui.Begin(m_componentName + (disabled ? " - disabled" : "") + "###" + m_id, ref _optionsOpen, ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoResize);

            for( int i=0; i< _optionsList.Count; i++) {
                OPTION.DrawOption(_optionsList[i]);
            }

            ImGui.End();
        }

        public void Draw(int height) {
            if (disabled) { Colour.PushColour(Colour.LIGHT_GREY); }
            else { Colour.PushColour(_colour); }

            // Draw the main child.
            ImGui.BeginChild(m_id, new System.Numerics.Vector2(_WIDTH, height), true, ImGuiWindowFlags.MenuBar);

            if (disabled) { Colour.PopColour(Colour.LIGHT_GREY); }
            else { Colour.PopColour(_colour); }

            DrawMenu();

            foreach (Option option in _optionsList) {
                if (option.m_onHeadsUp == true) {
                    ImGui.Text(option.m_title + ": " + option.GetValue());
                }
            }

            ImGui.EndChild();

            if (_optionsOpen) { DrawOptionsWindow();  }
        }

        public bool GetIsDead() {
            return _markedDead;
        }

        public void AddOption(Option option) {
            _optionsList.Add(option);
        }

        // This function returns a list of the obejct value from each component.  
        /// <summary> "python's not going to do type checking anyways" </summary>
        public List<object> GetComponentValueList() {
            List<object> outList = new List<object>();

            foreach (Option o in _optionsList) {
                outList.Add(o.GetValue());
            }

            return outList;
        }
    }

    // This type holds the information that makes up each option.  Options are internally accessed by id.
    // Remember to update both the typed and object variables.
    public class Option
    {
        public int m_id;
        public string m_title;
        public Type m_type;

        public object m_minValue;
        public object m_maxValue;

        // These are type specific values.
        public int m_value_i;
        public float m_value_f;
        public string m_value_s;
        public bool m_value_b;

        public int m_optionType;
        public bool m_onHeadsUp;  // Is shown on the heads-up display.

        public Option(int id, string title, Type type, object initValue, object minValue = null, object maxValue = null, int optionType = OPTION.SLIDER_INT, bool onHeadsUp = false) {
            m_id = id;
            m_title = title;
            m_type = type;

            m_minValue = minValue;
            m_maxValue = maxValue;

            m_optionType = optionType;
            m_onHeadsUp = onHeadsUp;

            if (m_type == typeof(int)) {
                m_value_i = (int)initValue;
            } else if (m_type == typeof(float)) {
                m_value_f = Convert.ToSingle(initValue);
            } else if (m_type == typeof(string)) {
                m_value_s = (string)initValue;
            } else if (m_type == typeof(bool)) {
                m_value_b = (bool)initValue;
            }
        }

        public object GetValue() {
            if (m_type == typeof(int)) {
                return m_value_i;
            } else if (m_type == typeof(float)) {
                return m_value_f;
            } else if (m_type == typeof(string)) {
                return m_value_s;
            } else if (m_type == typeof(bool)) {
                return m_value_b;
            } else {
                return null;
            }
        }
    }

    public class ComponentLoader
    {
        // This list holds the names of all the loaded components.
        private List<string> _componentNameList;

        public static ScriptEngine s_ironPyEngine;

        // When instatiated loads components.  The components list can be reloaded.
        public ComponentLoader()
        {
            ComponentLoader.s_ironPyEngine = Python.CreateEngine();  // Init iron py engine.
            ComponentLoader.s_ironPyEngine.InitializeLifetimeService();  // Does this do anything?

            ReadComponentList();
        }

        /// <summary> This function reads the components directory and saves all valid component filenames. </summary>
        public void ReadComponentList()
        {
            _componentNameList = new List<string>();  // Reset list.

            DirectoryInfo CurrentDir = new DirectoryInfo( @"components/");  //Directory.GetCurrentDirectory() +
            FileInfo[] Files = CurrentDir.GetFiles("c_*.py"); // C:\code\imageFilter-2\ImGui.NET.SampleProgram.XNA\components

            foreach (FileInfo file in Files) {
                _componentNameList.Add(file.Name);
                Console.WriteLine("\tLoaded " + file.Name);
            }
        }

        /// <summary> This is a list of the filename strings of the components. </summary>
        public List<string> GetComponentNames()
        {
            return _componentNameList;
        }

        public void AddComponent(string filename, List<Component> componentList)
        {
            // Load the python file.  // This is the lag?
            ScriptSource source = ComponentLoader.s_ironPyEngine.CreateScriptSourceFromFile(@"components/"+filename);  // TODO: do this step for all components at init?
            ScriptScope scope = ComponentLoader.s_ironPyEngine.CreateScope();

            ObjectOperations op = ComponentLoader.s_ironPyEngine.Operations;

            // Create class object.
            source.Execute(scope);

            // Get data from file.
            object classObject = scope.GetVariable("Component"); // Load the class object.
            object classInstance = op.Invoke(classObject);  // Create the class instance.

            // Get the varaibles.
            dynamic runMethod = op.GetMember(classInstance, "run");
            dynamic componentsList = op.GetMember(classInstance, "_components");
            dynamic initComponentsMethod = op.GetMember(classInstance, "components");
            dynamic nameString = op.GetMember(classInstance, "name");

            Component newComponent = new Component(runMethod, nameString);

            op.Invoke(initComponentsMethod);
            foreach (dynamic option in componentsList) {  // Read options from the component file.
                newComponent.AddOption( new Option((int)option[0], (string)option[1], OPTION.GetType((int)option[5]), option[2], option[3], option[4], (int)option[5], (bool)option[6]) );
            }

            componentList.Add(newComponent);
        }
    }
}
