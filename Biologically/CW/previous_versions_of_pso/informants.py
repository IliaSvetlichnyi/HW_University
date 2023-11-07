import random


def assign_random_informants(particles, num_informants=3):
    for particle in particles:
        # Exclude the particle itself
        potential_informants = [p for p in particles if p != particle]
        # this function is used to randomly select num_informants particles
        # from the list of potential informants
        informants = random.sample(potential_informants, num_informants)
        particle.informants = informants
