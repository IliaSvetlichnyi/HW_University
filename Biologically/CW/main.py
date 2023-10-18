from ANNbuilder import build_network
import numpy as np

architecture = [2, 4, 1]
activations = ["relu", "logistic"]

network = build_network(architecture, activations)

input_data = np.array([[0.5, 0.2], [0.3, 0.8], [0.9, 0.1]])
output = network.forward(input_data)
print(output)
