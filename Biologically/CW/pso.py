from numpy import array, random
from loss import mse_loss

# PSO parameters
iter_max = 10000
pop_size = 100
# coefficients determining the magnitude of the influence of the best particle solution 
# and the best global solution, respectively, on the particle velocity.
c1 = 2
c2 = 2
err_crit = 0.00001

# This class represents a particle in the PSO algorithm
class Particle:
    def __init__(self, dimensions):
        self.params = array([random.rand() for _ in range(dimensions)])
        self.fitness = float('inf')  # initial value of the error
        self.best_params = None  # best parameters
        self.v = 0.0  # particle velocity

def optimize(network, data, target, dimensions, loss_function=mse_loss):
    particles = [Particle(dimensions) for _ in range(pop_size)]
    
    gbest = particles[0]
    err = float('inf')

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
                p.best_params = p.params

            # If the error is less than the global error, update the global best particle.
            if fitness < gbest.fitness:
                gbest = p

            # Updating the velocity and position of the particle
            p.v += c1 * random.rand() * (p.best_params - p.params) + c2 * random.rand() * (gbest.params - p.params)
            p.params += p.v

        # Checking the stop condition
        if gbest.fitness < err_crit:
            break

    return gbest.params
