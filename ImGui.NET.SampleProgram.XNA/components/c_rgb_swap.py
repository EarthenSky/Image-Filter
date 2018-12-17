
''' Swaps RGB values for all pixels in the image.'''
class Component():

    def __init__(self):
        self._components = []
        self.name = "rgb swap"

    def components(self):
        # Components are tuples with the data (id, title, init, min, max, type, is_headsup)
        self._components.append( (1, "Swap Type", 0, 0, 5, 1, True) )

    def run(self, image_data, component_values, width, height):
        swap_type = component_values[0]

        colour_map = []
        if swap_type == 0:
            colour_map = [0, 1, 2]
        elif swap_type == 1:
            colour_map = [0, 2, 1]
        elif swap_type == 2:
            colour_map = [1, 0, 2]
        elif swap_type == 3:
            colour_map = [1, 2, 0]
        elif swap_type == 4:
            colour_map = [2, 0, 1]
        elif swap_type == 5:
            colour_map = [2, 1, 0]

        for index in range(0, len(image_data)):
            image_data[index] = [image_data[index][colour_map[0]], image_data[index][colour_map[1]], image_data[index][colour_map[2]], 255]

        return image_data  # This must return image_data.
