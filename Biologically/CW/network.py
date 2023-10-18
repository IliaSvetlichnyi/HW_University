from layer import NeuralLayer

class NeuralNetwork:
    def __init__(self):
        self.layers = []

    def add_layer(self, input_size, output_size, activation='logistic'):
        self.layers.append(NeuralLayer(input_size, output_size, activation))

    def forward(self, input_data):
        for layer in self.layers:
            input_data = layer.forward(input_data)
        return input_data