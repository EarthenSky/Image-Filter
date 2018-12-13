
''' This component inverses pixel colours in a channel'''
class Component():

    def __init__(self):
        self._components = []
        self.name = "inverse"

    def components(self):
        # Components are tuples with the data (id, title, init, min, max, type, is_headsup)
        self._components.append( (1, "R Inverse", False, 0, 0, 7, True) )
        self._components.append( (2, "G Inverse", False, 0, 0, 7, True) )
        self._components.append( (3, "B Inverse", False, 0, 0, 7, True) )

        #TODO: Add some cool multichannel inversing.

    def run(self, image_data, component_values, width, height):
        is_r_inv = component_values[0]
        is_g_inv = component_values[1]
        is_b_inv = component_values[2]

        for index in range(0, len(image_data)):
            r_val = (255 - image_data[index][0]) if is_r_inv else image_data[index][0]
            g_val = (255 - image_data[index][1]) if is_g_inv else image_data[index][1]
            b_val = (255 - image_data[index][2]) if is_b_inv else image_data[index][2]
            image_data[index] = [r_val, g_val, b_val, 255]

        return image_data  # This must return image_data.
