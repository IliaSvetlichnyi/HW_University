from numpy import array, random
from loss import mse_loss

# # PSO parameters
# iter_max = 500
# pop_size = 150
# c1 = 2.0  # Cognitive coefficient
# c2 = 2.0  # Social coefficient
# c3 = 0.5  # Informants coefficient, reduced to give less influence
# err_crit = 0.0001
# informants_per_particle = 3  # Reduced number of informants per particle


class Particle:
    def __init__(self, dimensions):
        self.params = array([random.rand() for _ in range(dimensions)])
        self.fitness = float('inf')
        self.v = array([0.0 for _ in range(dimensions)])
        self.best = self.params
        self.best_fitness = float('inf')
        self.informants = []
        self.best_informant_params = self.params
        self.best_informant_fitness = float('inf')

    def select_informants(self, particles, informants_per_particle):
        # Select random informants from the list of particles, excluding itself
        self.informants = random.choice([p for p in particles if p != self],
                                        informants_per_particle, replace=False)

    def update_informant_best(self):
        # Find the best position among informants
        for informant in self.informants:
            if informant.best_fitness < self.best_informant_fitness:
                self.best_informant_fitness = informant.best_fitness
                self.best_informant_params = informant.best


def optimize(network, data, target, dimensions, pso_params, loss_function=mse_loss):
    pop_size = pso_params.get('pop_size', 150)
    c1 = pso_params.get('c1', 2.0)
    c2 = pso_params.get('c2', 2.0)
    c3 = pso_params.get('c3', 0.5)
    informants_per_particle = pso_params.get('informants_per_particle', 3)
    iter_max = pso_params.get('iter_max', 500)
    err_crit = pso_params.get('err_crit', 0.0001)

    particles = [Particle(dimensions) for _ in range(pop_size)]

    # Initialize informants for each particle
    for particle in particles:
        particle.select_informants(particles, informants_per_particle)

    gbest = min(particles, key=lambda p: p.fitness)

    for i in range(iter_max):
        for p in particles:
            # Evaluate fitness
            network.set_weights(p.params)
            output = network.forward(data)
            fitness = loss_function(output, target)

            # Update personal best
            if fitness < p.fitness:
                p.fitness = fitness
                p.best = p.params
                p.best_fitness = fitness

            # Update informants best, if needed
            p.update_informant_best()

            # Update global best
            if fitness < gbest.fitness:
                gbest = p

            # Update the particle's velocity and position
            inertia = p.v
            cognitive = c1 * random.rand() * (p.best - p.params)
            social = c2 * random.rand() * (gbest.params - p.params)
            informants_influence = c3 * random.rand() * (p.best_informant_params - p.params)
            p.v = inertia + cognitive + social + informants_influence
            p.params += p.v

        # Periodically update informants for each particle
        if i % 100 == 0 or gbest.fitness < err_crit:
            for particle in particles:
                particle.select_informants(particles, informants_per_particle)
                particle.update_informant_best()
            print(f"Epoch {i}/{iter_max} - Loss: {gbest.fitness:.4f}")

        # Check the stop condition
        if gbest.fitness < err_crit:
            break

    return gbest.params
