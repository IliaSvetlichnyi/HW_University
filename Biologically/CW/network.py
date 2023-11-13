# This file implements a class for representing a multilayer neural network.

from layer import NeuralLayer
import numpy as np


class NeuralNetwork:
    def __init__(self):
        self.layers = []

    def add_layer(self, input_size, output_size, activation='logistic'):
        self.layers.append(NeuralLayer(input_size, output_size, activation))

    def forward(self, input_data):
        output = input_data
        for layer in self.layers:
            output = layer.forward(output)
            # print(f"Output shape after layer: {output.shape}")
        return output

    def set_weights(self, weights):
        idx = 0
        for layer in self.layers:
            # get number of weights and biases for this layer
            w_size = layer.weights.size
            b_size = layer.biases.size

            # reshape weights and set them to the layer
            layer.weights = weights[idx:idx +
                                    w_size].reshape(layer.weights.shape)
            idx += w_size

            # set biases for the layer
            layer.biases = weights[idx:idx +
                                   b_size].reshape(layer.biases.shape)
            idx += b_size

        if idx != len(weights):
            raise ValueError(
                "Total size of provided weights does not match the network")

    def total_weights(self):
        total = 0
        for layer in self.layers:
            total += layer.weights.size + layer.biases.size
        return total
