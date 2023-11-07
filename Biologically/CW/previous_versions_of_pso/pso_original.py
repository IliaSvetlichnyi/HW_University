from numpy import array, random
from loss import mse_loss

# PSO parameters
iter_max = 1000
pop_size = 100
# coefficients determining the magnitude of the influence of the best particle solution 
# and the best global solution, respectively, on the particle velocity.
c1 = 2.5
c2 = 2.5
err_crit = 0.0001

# This class represents a particle in the PSO algorithm
class Particle:
    def __init__(self, dimensions):
        self.params = array([random.rand() for _ in range(dimensions)])
        self.fitness = float('inf') # initial value of the error
        self.v = 0.0 # particle velocity
        self.best = self.params # Initially, best is initialized with the current parameters of the particle.

def optimize(network, data, target, dimensions, loss_function=mse_loss):
    particles = [Particle(dimensions) for _ in range(pop_size)]

    gbest = particles[0]

    for i in range(iter_max):
        for p in particles:
            # For each particle, a forward pass through the network is performed 
            # using the weights of this particle, after which the error is calculated
            network.set_weights(p.params)
            output = network.forward(data)
            fitness = loss_function(output, target)

            # If the error is less than the current particle error, 
            # we update the best particle parameters.
            if fitness < p.fitness:
                p.fitness = fitness
                p.best = p.params

            # If the error is less than the global error, update the global best particle.
            if fitness < gbest.fitness:
                gbest = p

            # Updating the velocity and position of the particle
            v = p.v + c1 * random.rand() * (p.best - p.params) + c2 * random.rand() * (gbest.params - p.params)
            p.params += v

        # If this is the 100th, 200th, 300th, etc. iteration, we output the information
        if i % 100 == 0:
            print(f"Epoch {i}/{iter_max} - Loss: {gbest.fitness:.4f}")

        # Checking the stop condition
        if gbest.fitness < err_crit:
            break

    return gbest.params
