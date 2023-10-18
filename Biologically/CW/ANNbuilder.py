from network import NeuralNetwork

# "architecture" is a list of numbers where each number represents number of neurons in each layer
def build_network(architecture, activations):
    network = NeuralNetwork()
    for i in range(len(architecture) - 1):
        network.add_layer(architecture[i], architecture[i+1], activations[i])
    return network