import numpy as np


class Particle:
    def __init__(self, dimensions):
        # Initialize the particle's position and velocity randomly
        self.position = np.random.uniform(-5, 5, dimensions)
        self.velocity = np.random.randn(dimensions)
        # Set the particle's best position and best loss to the current position and infinity respectively
        self.best_position = np.copy(self.position)
        self.best_loss = float('inf')
        # Set the particle's best informant position to the current position
        self.best_informant_position = np.copy(self.position)
        # Initialize the list of informants
        self.informants = []

    def update_best_informant_position(self, positions, loss_func, network, train_data, train_labels):
        # print(f"Train labels shape in update_best_informant_position: {train_labels.shape}")
        # Initialize the best local loss to the particle's best loss
        best_local_loss = self.best_loss
        # Iterate over all the positions of the informants
        for pos in positions:
            # Set the weights of the neural network to the current position
            network.set_weights(pos)
            # Compute the predictions of the neural network for the training data
            predictions = network.forward(train_data)
            # Compute the loss for the current position
            # print(f"Train labels shape before calling loss function: {train_labels.shape}")
            local_loss = loss_func(predictions, train_labels)
            # Update the best local loss and best informant position if the current position has a lower loss
            if local_loss < best_local_loss:
                best_local_loss = local_loss
                self.best_informant_position = np.copy(pos)

    def update_velocity(self, global_best_position, informant_best_position, w=0.5, c1=1, c2=2, c3=1):
        # Compute the components of the velocity update equation
        inertia = w * self.velocity
        cognitive = c1 * np.random.rand(*self.position.shape) * (self.best_position - self.position)
        social = c2 * np.random.rand(*self.position.shape) * (global_best_position - self.position)
        informant_influence = c3 * np.random.rand(*self.position.shape) * (informant_best_position - self.position)
        # Update the velocity of the particle
        self.velocity = inertia + cognitive + social + informant_influence

    def update_position(self, bounds=None):
        # Update the position of the particle based on its velocity
        self.position += self.velocity
        # Clip the position to the given bounds if they are provided
        if bounds is not None:
            self.position = np.clip(self.position, bounds[0], bounds[1])

    def evaluate(self, loss_func, network, train_data, train_labels):
        # print(f"Train labels shape in evaluate: {train_labels.shape}")
        # Set the weights of the neural network to the current position
        network.set_weights(self.position)
        # Compute the predictions of the neural network for the training data
        predictions = network.forward(train_data)
        # Compute the loss for the current position
        loss = loss_func(predictions, train_labels)
        # Update the particle's best position and best loss if the current position has a lower loss
        if loss < self.best_loss:
            self.best_loss = loss
            self.best_position = np.copy(self.position)
