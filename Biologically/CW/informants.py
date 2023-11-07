# informants.py

import random


def select_informants(particles, num_informants):
    """
    Создаёт группу информаторов для каждой частицы.
    particles: список всех частиц.
    num_informants: количество информаторов для каждой частицы.
    Возвращает словарь с ключами в виде индексов частиц и значениями в виде списков индексов их информаторов.
    """
    informants_group = {}
    for i in range(len(particles)):
        selected_informants = random.sample(
            range(len(particles)), num_informants)
        informants_group[i] = selected_informants
    return informants_group


def get_best_informant_position(particles, informants_group, i):
    """
    Находит лучшее решение среди информаторов частицы.
    particles: список всех частиц.
    informants_group: словарь информаторов для каждой частицы.
    i: индекс текущей частицы.
    Возвращает лучшую позицию среди информаторов.
    """
    best_position = None
    best_fitness = -float('inf')
    for idx in informants_group[i]:
        if particles[idx].best_fitness > best_fitness:
            best_position = particles[idx].best_position
            best_fitness = particles[idx].best_fitness
    return best_position
