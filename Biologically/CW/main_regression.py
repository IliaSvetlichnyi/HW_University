from sklearn.metrics import mean_squared_error
import pandas as pd
import numpy as np
from ANNbuilder import build_network
from pso import pso_optimization
from loss import mse_loss
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler

# Loading the data
data = pd.read_csv(
    "/Users/ilya/Desktop/GitHub_Repositories/HW_University/Data_Mining/datasets/Real estate.csv")

data = data.drop(['No'], axis=1)

# Devide the data into features and labels
X = data.drop("Y house price of unit area", axis=1).values
y = data["Y house price of unit area"].values.reshape(-1, 1)

# Data separation
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42)

# Data scaling
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)


# Defining the network architecture and activation functions
architecture = [6, 12, 1]  # Network architecture
activations = ['relu', 'linear']  # Activation functions

# Use the build_network function to create the network
network = build_network(architecture, activations)

# Get the dimensionality of the network weights vector
dimensions = network.total_weights()

# Call pso_optimization function with the created network and parameters
best_weights = pso_optimization(
    num_particles=100,
    num_iterations=300,
    loss_func=mse_loss,
    network=network,
    train_data=X_train_scaled,
    train_labels=y_train,
    dimensions=dimensions)

# Set the best weights to the network
network.set_weights(best_weights)

# Test the network
output = network.forward(X_test_scaled)
predictions = output

# Print the results
print("Test RMSE:", mean_squared_error(y_test, predictions)**0.5)
