from ANNbuilder import build_network
from pso import optimize
from loss import mse_loss
import numpy as np

# 1. Подготовка данных
# Здесь мы используем простой пример. В реальной жизни вам нужно использовать настоящий датасет.
data = np.array([[0, 0], [0, 1], [1, 0], [1, 1]])
target = np.array([[0], [1], [1], [0]])

# 2. Определение архитектуры и параметров
architecture = [2, 4, 1]
activations = ["relu", "logistic"]
network = build_network(architecture, activations)

# 3. Тренировка сети
dimensions = sum([layer.weights.size + layer.biases.size for layer in network.layers]) # общее количество весов и смещений
expected_dimensions = network.total_weights()
assert dimensions == expected_dimensions, f"Expected {expected_dimensions} dimensions but got {dimensions}"
optimize(network, data, target, dimensions, loss_function=mse_loss)

# 4. Тестирование сети
output = network.forward(data)
print("Predictions:", output)
print("True values:", target)
