# This file implements a class for representing a layer in a neural network.

import numpy as np
from activation import logistic, relu, tanh

class NeuralLayer:
    def __init__(self, input_size, output_size, activation='logistic'):
        # by multiplying by 0.01 we just scale initial weights in order to do them small
        np.random.seed(42)
        self.weights = np.random.rand(input_size, output_size) * 0.01
        self.biases = np.zeros((1, output_size))
        self.activation_function = self.get_activation_function(activation)
    
    def get_activation_function(self, name):
        if name == 'logistic':
            return logistic
        elif name == 'relu':
            return relu
        elif name == 'tanh':
            return tanh
        else:
            raise ValueError(f"Unknown activation function: {name}")
        
    def forward(self, input_data):
        self.input_data = input_data
        self.output = self.activation_function(np.dot(input_data, self.weights) + self.biases)
        return self.output