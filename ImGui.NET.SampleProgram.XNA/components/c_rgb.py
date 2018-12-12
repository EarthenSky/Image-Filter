# ----------------------------------------------------------------
# The function "run(image_data)" will be called when the component is being used.
#
# The function "components()" will be called just before the "run" function.  You can add
# components in the constructor if you want.
#
# When creating a new component you must return the changed pixel values.
# Leave the class named Component.
# Must have a name member variable.
#
# All function parameters are deliberate and must not be changed.
# ----------------------------------------------------------------

''' Type Values:
        public const int SLIDER_INT     = 1;
        public const int SLIDER_FLOAT   = 2;
        public const int INPUT_TEXT     = 3;
        public const int INPUT_INT      = 4;
        public const int INPUT_FLOAT    = 5;
        public const int TEXT           = 6;
        public const int BOOLEAN        = 7;
'''

''' This component adds to the rgb pixel values.'''
class Component():

    def __init__(self):
        self._components = []
        self.name = "rgb modifier"

    def components(self):
        # Components are tuples with the data (id, title, init, min, max, type, is_headsup)
        self._components.append( (1, "Multiplication is done before addition.", 0, 0, 0, 6, False) )

        self._components.append( (2, "R Add", 0, -256, 256, 1, True) )
        self._components.append( (3, "G Add", 0, -256, 256, 1, True) )
        self._components.append( (4, "B Add", 0, -256, 256, 1, True) )

        self._components.append( (5, "R Mod", 1.0, 0.0, 8.0, 2, True) )
        self._components.append( (6, "G Mod", 1.0, 0.0, 8.0, 2, True) )
        self._components.append( (7, "B Mod", 1.0, 0.0, 8.0, 2, True) )

    ''' component_values is a list of user edited component values.  They are mapped the same as added in the above class. '''
    def run(self, image_data, component_values, width, size):
        r_add = component_values[1]
        g_add = component_values[2]
        b_add = component_values[3]

        r_mod = component_values[4]
        g_mod = component_values[5]
        b_mod = component_values[6]

        for index in range(0, len(image_data)):
            image_data[index] = [image_data[index][0]*r_mod + r_add, image_data[index][1]*g_mod + g_add, image_data[index][2]*b_mod + b_add, 255]

        return image_data  # This must return image_data.
