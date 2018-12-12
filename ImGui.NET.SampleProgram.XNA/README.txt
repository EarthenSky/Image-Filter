###How to make a new project using monogame and imgui.net:
- Copy xna test files to a new directory and open the solution file.
- Rename settings. (Use ImGui.NET for main project namespace)
- Reroute paths.

### Setup IronPython to read Python classes from file.
- Use nuget to Install-Package IronPython.
- Add reference to Microsoft.CSharp (for using the dynamic type)

### Use information.
- All component files must start with "m_" and end with .py.  The search pattern used to identify components in the components folder is "m_*.py"
-