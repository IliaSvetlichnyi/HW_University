import random

class Particle:
    def __init__(self, dim, minx, maxx):
        self.__pos = [random.uniform(minx, maxx) for i in range(dim)]
        self.__vel = [random.uniform(minx - maxx, maxx - minx) for i in range(dim)]
        self.__best_pos = self.__pos[:]
        self.__best_err = float('inf')
        
    def set_pos(self, pos):
        self.__pos = pos[:]
        
    def set_vel(self, vel):
        self.__vel = vel[:]
        
    def get_pos(self):
        return self.__pos[:]
    
    def get_vel(self):
        return self.__vel[:]
    
    def get_best_pos(self):
        return self.__best_pos[:]
    
    def get_best_err(self):
        return self.__best_err
    
    def evaluate(self, costFunc):
        err = costFunc(self.__pos)
        if err < self.__best_err:
            self.__best_err = err
            self.__best_pos = self.__pos[:]
            
    def update_velocity(self, best_pos_g, w, c1, c2, r1, r2):
        for i in range(len(self.__vel)):
            vel_cognitive = c1 * r1 * (self.__best_pos[i] - self.__pos[i])
            vel_social = c2 * r2 * (best_pos_g[i] - self.__pos[i])
            self.__vel[i] = w * self.__vel[i] + vel_cognitive + vel_social
            
    def update_position(self, bounds):
        for i in range(len(self.__pos)):
            self.__pos[i] = self.__pos[i] + self.__vel[i]
            
            if self.__pos[i] < bounds[i][0]:
                self.__pos[i] = bounds[i][0]
                self.__vel[i] = 0.0
                
            elif self.__pos[i] > bounds[i][1]:
                self.__pos[i] = bounds[i][1]
                self.__vel[i] = 0.0
                
class PSO:
    def __init__(self, costFunc, dim, size, minx, maxx, max_iter, w, c1, c2, num_informants):
        self.__particles = [Particle(dim, minx, maxx) for i in range(size)]
        self.__best_pos_g = [0.0 for i in range(dim)]
        self.__best_err_g = float('inf')
        self.__max_iter = max_iter
        self.__costFunc = costFunc
        self.__bounds = [(minx, maxx) for i in range(dim)]
        self.__w = w
        self.__c1 = c1
        self.__c2 = c2
        self.__num_informants = num_informants
        
    def run(self):
        for i in range(self.__max_iter):
            for j in range(len(self.__particles)):
                self.__particles[j].evaluate(self.__costFunc)
                
                if self.__particles[j].get_best_err() < self.__best_err_g:
                    self.__best_err_g = self.__particles[j].get_best_err()
                    self.__best_pos_g = self.__particles[j].get_best_pos()
                    
            for j in range(len(self.__particles)):
                informants = self.get_informants(j)
                r1, r2 = random.random(), random.random()
                self.__particles[j].update_velocity(self.__best_pos_g, self.__w, self.__c1, self.__c2, r1, r2)
                self.__particles[j].update_position(self.__bounds)
                
        return self.__best_pos_g, self.__best_err_g
    
    def get_informants(self, index):
        informants = []
        for i in range(self.__num_informants):
            while True:
                idx = random.randint(0, len(self.__particles) - 1)
                if idx != index and idx not in informants:
                    informants.append(idx)
                    break
        return [self.__particles[i] for i in informants]
