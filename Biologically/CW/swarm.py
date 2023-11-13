from particle import Particle
import numpy as np


class Swarm:
    def __init__(self, dimensions, num_particles=30, num_informants=3):
        """
        Initializes a Swarm object with a given number of particles and informants.

        Args:
        - dimensions (int): The number of dimensions in the search space.
        - num_particles (int): The number of particles in the swarm. Default is 30.
        - num_informants (int): The number of informants for each particle. Default is 3.
        """
        self.particles = [Particle(dimensions) for _ in range(num_particles)]
        self.global_best_loss = float('inf')
        # Initialize global_best_position with the position of the first particle
        self.global_best_position = np.copy(self.particles[0].position)
        self.num_informants = num_informants
        self.assign_informants()  # Call method to assign informants
        # Added to track the iteration of the best result
        self.global_best_iteration = 0

    def assign_informants(self):
        """
        Assigns informants to each particle in the swarm.
        """
        for particle in self.particles:
            # Choose num_informants particles randomly from the swarm, excluding the current particle
            informants = np.random.choice(
                [p for p in self.particles if p is not particle],
                self.num_informants,
                replace=False)
            particle.informants = informants

    def evaluate_global_best(self, current_iteration):
        """
        Evaluates the global best position and loss for the swarm.

        Args:
        - current_iteration (int): The current iteration of the swarm.
        """
        for particle in self.particles:
            if particle.best_loss < self.global_best_loss:
                self.global_best_loss = particle.best_loss
                self.global_best_position = particle.best_position
                # Update the iteration when the result is improved
                self.global_best_iteration = current_iteration

    def update_particles(self, loss_func, network, train_data, train_labels, current_iteration):
        """
        Updates the particles in the swarm.

        Args:
        - loss_func (function): The loss function to use for evaluating particle positions.
        - network (object): The neural network to use for evaluating particle positions.
        - train_data (ndarray): The training data to use for evaluating particle positions.
        - train_labels (ndarray): The training labels to use for evaluating particle positions.
        - current_iteration (int): The current iteration of the swarm.
        """
        # print(f"Train labels shape in update_particles: {train_labels.shape}")
        # First update the global best if this is the first iteration
        self.evaluate_global_best(current_iteration)
        for particle in self.particles:
            # Update the best informant position for each particle first
            informant_positions = [
                informant.best_position for informant in particle.informants]
            particle.update_best_informant_position(
                informant_positions, loss_func, network, train_data, train_labels)
            # Then update the velocity, taking into account the global best position
            particle.update_velocity(
                self.global_best_position, particle.best_informant_position)
            # Update the position and evaluate the new loss
            particle.update_position()
            particle.evaluate(loss_func, network, train_data, train_labels)
            # For debugging
            # print(
            #     f"Particle Position: {particle.position} Loss: {particle.best_loss}")
        # After updating all particles, evaluate the global best again
        self.evaluate_global_best(current_iteration)
