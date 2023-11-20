# This file implements informants logic for PSO algorithm

import random


def select_informants(particles, num_informants):
    """
    Creates a group of informants for each particle.
    particles: list of all particles.
    num_informants: number of informants for each particle.
    Returns a dictionary with keys as particle indices and values as lists of indices of their informants.
    """
    informants_group = {}
    for i in range(len(particles)):
        selected_informants = random.sample(
            range(len(particles)), num_informants)
        informants_group[i] = selected_informants
    return informants_group


def get_best_informant_position(particles, informants_group, i):
    """
    Finds the best solution among the informants of a particle.
    particles: list of all particles.
    informants_group: dictionary of informants for each particle.
    i: index of the current particle.
    Returns the best position among the informants.
    """
    best_position = None
    best_fitness = -float('inf')
    for idx in informants_group[i]:
        if particles[idx].best_fitness > best_fitness:
            best_position = particles[idx].best_position
            best_fitness = particles[idx].best_fitness
    return best_position
