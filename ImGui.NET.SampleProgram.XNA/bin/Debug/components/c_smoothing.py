
''' This component smooths the image by avaraging nearby pixels to adjacent pixels.'''
class Component():

    def __init__(self):
        self._components = []
        self.name = "smoothing"

    def components(self):
        # Components are tuples with the data (id, title, init, min, max, type, is_headsup)
        self._components.append( (1, "Horizontal Smoothing", False, 0, 0, 7, True) )
        self._components.append( (2, "Vertical Smoothing",   False, 0, 0, 7, True) )
        self._components.append( (3, "Diagonal Smoothing",   False, 0, 0, 7, True) )
        self._components.append( (4, "Smoothing Coefficient", 1.0, 0.0, 10.0, 5, True) )
        self._components.append( (5, "SC. is how heavily adjacent pixels are weighted.", 0, 0, 0, 6, False) )

    # TODO: implement diagonal smoothing.
    # TODO: add ranged averaging (use more adjacent pixels.) & option.
    def run(self, image_data, component_values, width, size):
        is_horizontal = component_values[0]
        smoothing = 1/component_values[3]

        new_image_data = []

        for index in range(0, len(image_data)):
            count = smoothing
            px_r = image_data[index][0] * smoothing
            px_g = image_data[index][1] * smoothing
            px_b = image_data[index][2] * smoothing

            if (is_horizontal):
                case_l = (index % width)==0
                case_r = ((index+1) % width)==0

                count += ((0 if case_l else 1) + (0 if case_r else 1))
                px_r += ( (0 if case_l else image_data[index-1][0]) + (0 if case_r else image_data[index+1][0]) )
                px_g += ( (0 if case_l else image_data[index-1][1]) + (0 if case_r else image_data[index+1][1]) )
                px_b += ( (0 if case_l else image_data[index-1][2]) + (0 if case_r else image_data[index+1][2]) )

            new_image_data.append( [int(px_r/count), int(px_g/count), int(px_b/count), 255] )

        return new_image_data  # This is returning the new image data list.
