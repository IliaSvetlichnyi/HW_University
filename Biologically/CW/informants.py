import random

def assign_random_informants(particles, num_informants=3):
    for particle in particles:
        potential_informants = [p for p in particles if p != particle]  # Exclude the particle itself
        informants = random.sample(potential_informants, num_informants)
        particle.informants = informants
