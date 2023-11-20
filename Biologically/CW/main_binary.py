from sklearn.preprocessing import MinMaxScaler
from sklearn.model_selection import train_test_split
import pandas as pd
from ANNbuilder import build_network
from pso import pso_optimization
from loss import mse_loss
from sklearn.metrics import classification_report


data = pd.read_csv("banknote_authentication.csv", header=None)

X = data.drop(4, axis=1).values
y = data[4].values.reshape(-1, 1)


# Data separation and scaling
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42)

scaler = MinMaxScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

# Defining the network architecture and activation functions
architecture = [4, 8, 1]  # Network architecture
activations = ['relu', 'logistic']  # Activation functions

# Use the build_network function to create the network
network = build_network(architecture, activations)

# Get the dimensionality of the network weights vector
dimensions = network.total_weights()

# Call pso_optimization function with the created network and parameters
best_weights = pso_optimization(
    num_particles=50,
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
predictions = (output > 0.5).astype(int)

# Print the results
print(classification_report(y_test, predictions))
