from sklearn.preprocessing import LabelEncoder
from sklearn.preprocessing import MinMaxScaler
from sklearn.model_selection import train_test_split
import pandas as pd
import numpy as np
from ANNbuilder import build_network
from pso import pso_optimization
from loss import mse_loss, sparse_categorical_crossentropy
from sklearn.metrics import classification_report

# Data loading
data = pd.read_csv(
    "/Users/ilya/Desktop/GitHub_Repositories/HW_University/Data_Mining/datasets/Iris.csv")

data = data.drop(['Id'], axis=1)

# Deviding data into features and labels
X = data.drop("Species", axis=1).values
y = data["Species"]


# Data separation and scaling
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42)

scaler = MinMaxScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

# Encoding labels with LabelEncoder into numbers
encoder = LabelEncoder()

# Converting class labels for training data
y_train_encoded = encoder.fit_transform(y_train)

# Converting class labels for test data
y_test_encoded = encoder.transform(y_test)


# Defining the network architecture and activation functions
architecture = [4, 8, 3]  # Network architecture
activations = ['relu', 'softmax']  # Activation functions

# Use the build_network function to create the network
network = build_network(architecture, activations)

# Get the dimensionality of the network weights vector
dimensions = network.total_weights()

# Call pso_optimization function with the created network and parameters
best_weights = pso_optimization(
    num_particles=100,
    num_iterations=300,
    loss_func=sparse_categorical_crossentropy,
    network=network,
    train_data=X_train_scaled,
    train_labels=y_train_encoded,
    dimensions=dimensions)

# Set the best weights to the network
network.set_weights(best_weights)

# Test the network
output = network.forward(X_test_scaled)
predictions = np.argmax(output, axis=1)

# Print the results
print(classification_report(y_test_encoded, predictions))
