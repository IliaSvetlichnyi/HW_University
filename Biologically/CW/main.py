import pandas as pd
from ANNbuilder import build_network
from Biologically.CW.previous_versions_of_pso.pso import optimize
from loss import mse_loss
import numpy as np

# 1. Загрузка и подготовка данных
data_path = "banknote_authentication.csv"
dataframe = pd.read_csv(data_path)

data = dataframe.iloc[:, :-1].values
target = dataframe.iloc[:, -1].values.reshape(-1, 1)

# Нормализация (в качестве примера)
data = (data - data.min(axis=0)) / (data.max(axis=0) - data.min(axis=0))

# 2. Определение архитектуры и параметров
architecture = [4, 8, 1]  # Пример архитектуры
activations = ["relu", "logistic"]
network = build_network(architecture, activations)

# 3. Тренировка сети
dimensions = sum([layer.weights.size + layer.biases.size for layer in network.layers])
expected_dimensions = network.total_weights()
assert dimensions == expected_dimensions, f"Expected {expected_dimensions} dimensions but got {dimensions}"
optimize(network, data, target, dimensions, loss_function=mse_loss)

# 4. Тестирование сети
output = network.forward(data)
correct_predictions = np.where((output > 0.5) == target, 1, 0).sum()
accuracy = correct_predictions / len(target)
print("Accuracy:", accuracy)
print("Predictions:", output)
print("True values:", target)
