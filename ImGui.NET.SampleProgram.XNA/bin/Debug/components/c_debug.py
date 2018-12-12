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

''' This class is for debugging'''
class Component():

    def __init__(self):
        self._components = []
        self.name = "debug"

    def components(self):
        # Components are tuples with the data (id, title, init, min, max, type, is_headsup)
        self._components.append( (1, "R Add", 0, -255, 255, 1, True) )
        self._components.append( (2, "G Add", 0, -255, 255, 1, True) )
        self._components.append( (3, "B Add", 0, -255, 255, 1, True) )

        self._components.append( (4, "Component4", "", None, None, 3, True) )
        self._components.append( (5, "Component5", 0, None, None, 4, True) )
        self._components.append( (6, "Component1", 0.5, 0.0, 1.0, 2, True) )
        self._components.append( (7, "Component2", 5,   0,   10,  1, True) )
        self._components.append( (8, "Component3", 1.5, 0.0, 3.0, 2, True) )
        self._components.append( (9, "Component4", "", None, None, 3, True) )
        self._components.append( (10, "Component5", 0, None, None, 4, True) )

    ### component_values is a list of user edited component values.  They are mapped the same as added abve.
    def run(self, image_data, component_values, width, size):
        print( type(image_data[0]).__name__ )
        print( "bef: " + image_data[0] )

        r_add = component_values[0]
        g_add = component_values[1]
        b_add = component_values[2]

        for index in range(0, len(image_data)):
            px = image_data[index]
            image_data[index] = [px[0] + r_add, px[1] + g_add, px[2] + b_add, 255]  # Cast to useable type.

        print( "aft: " + image_data[0] )

        return image_data  # This must return image_data.
