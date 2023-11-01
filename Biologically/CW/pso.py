from numpy import array, random
from loss import mse_loss, cross_entropy
from particle import Particle
from informants import assign_random_informants

# PSO parameters
iter_max = 150
pop_size = 50 # 100
c1 = 2.5
c2 = 2.5
err_crit = 0.0001
w = 0.5  # коэффициент инерции, значение может быть между [0.4, 0.9]

def optimize(network, data, target, dimensions, loss_function=mse_loss):
    particles = [Particle(dimensions) for _ in range(pop_size)]

    # Assign informants
    assign_random_informants(particles)
    
    gbest = min(particles, key=lambda x: x.fitness)
    gbest_fitness_history = []
    
    for i in range(iter_max):
        for p in particles:
            network.set_weights(p.params)
            output = network.forward(data)
            fitness = loss_function(output, target)

            if fitness < p.fitness:
                p.fitness = fitness
                p.best = p.params

            informant_best = p.best_informant_position()

            if fitness < gbest.fitness:
                gbest.fitness = fitness
                gbest.params = p.params.copy()

            v = w * p.v + c1 * random.rand() * (p.best - p.params) + c2 * random.rand() * (informant_best - p.params)
            
            p.params += v

        # Logging
        current_best = min([p.fitness for p in particles])
        print(f"Epoch {i}/{iter_max} - Current Best Loss: {current_best:.4f} - Global Best Loss: {gbest.fitness:.4f}")
        gbest_fitness_history.append(gbest.fitness)

        if gbest.fitness < err_crit:
            break

    return gbest.params
