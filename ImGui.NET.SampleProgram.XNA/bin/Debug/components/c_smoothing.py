
''' This component smooths the image by avaraging nearby pixels to adjacent pixels.  Can be used as anti-aliasing.'''
class Component():

    def __init__(self):
        self._components = []
        self.name = "smoothing"

    def components(self):
        # Components are tuples with the data (id, title, init, min, max, type, is_headsup)
        self._components.append( (1, "Horizontal Smoothing", False, 0, 0, 7, True) )
        self._components.append( (2, "Vertical Smoothing",   False, 0, 0, 7, True) )
        self._components.append( (3, "Diagonal Smoothing (not yet implemented)",   False, 0, 0, 7, True) )
        self._components.append( (4, "Smoothing Coefficient", 1.0, 0.0000001, 8.0, 2, True) )
        self._components.append( (5, "SC is how heavily adjacent pixels are weighted.", 0, 0, 0, 6, False) )
        self._components.append( (6, "Range", 1, 1, 1000, 4, True) )
        self._components.append( (7, "Range effects how far the blur goes.", 0, 0, 0, 6, False) )
        self._components.append( (7, "Range values above 1 may take a long time.", 0, 0, 0, 6, False) )

    # TODO: implement diagonal smoothing.
    def run(self, image_data, component_values, width, height):
        is_horizontal = component_values[0]
        is_vertical = component_values[1]
        is_diagonal = component_values[2]
        smoothing = 1/component_values[3]
        iterations = component_values[5]

        for c_iteration in range(1, iterations+1):
            new_image_data = []

            for index in range(0, len(image_data)):
                count = smoothing
                px_r = image_data[index][0] * smoothing
                px_g = image_data[index][1] * smoothing
                px_b = image_data[index][2] * smoothing

                if (is_horizontal):
                    case_l = (index % width)==0
                    case_r = ((index+1) % width)==0

                    count += (0 if case_l else 1) + (0 if case_r else 1)
                    px_r += (0 if case_l else image_data[index-1][0]) + (0 if case_r else image_data[index+1][0])
                    px_g += (0 if case_l else image_data[index-1][1]) + (0 if case_r else image_data[index+1][1])
                    px_b += (0 if case_l else image_data[index-1][2]) + (0 if case_r else image_data[index+1][2])

                if (is_vertical):
                    case_u = index < width
                    case_d = index > (width*height)-1-width

                    count += (0 if case_u else 1) + (0 if case_d else 1)
                    px_r += (0 if case_u else image_data[index-width][0]) + (0 if case_d else image_data[index+width][0])
                    px_g += (0 if case_u else image_data[index-width][1]) + (0 if case_d else image_data[index+width][1])
                    px_b += (0 if case_u else image_data[index-width][2]) + (0 if case_d else image_data[index+width][2])

                if (is_diagonal):
                    #TODO: this.
                    pass

                new_image_data.append( [int(px_r/count), int(px_g/count), int(px_b/count), 255] )

            if c_iteration == iterations:
                return new_image_data  # This is returns the new image data list.
            else:
                image_data = new_image_data
