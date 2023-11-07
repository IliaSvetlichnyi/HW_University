from numpy import array, random


class Particle:
    def __init__(self, dimensions):
        self.params = array([random.rand() for _ in range(dimensions)])
        self.fitness = float('inf')
        self.v = array([0.0 for _ in range(dimensions)])
        # self.v = 0.0
        self.best = self.params
        self.informants = []

    def add_informant(self, informant):
        self.informants.append(informant)

    def best_informant_position(self):
        if not self.informants:
            return self
        return min(self.informants, key=lambda x: self.fitness_function(x))

    def fitness_function(self, params):
        return params.fitness
