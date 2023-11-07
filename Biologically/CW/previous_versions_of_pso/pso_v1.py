import numpy as np


# Шаг 1: Определение класса частицы
class Particle:
    def __init__(self, dimensions):
        self.position = np.random.uniform(-5, 5, dimensions)
        self.velocity = np.random.randn(dimensions)
        self.best_position = np.copy(self.position)
        self.best_loss = float('inf')
        # Инициализация лучшей позиции информатора
        self.best_informant_position = np.copy(self.position)

     # Добавим новый метод для обновления лучшей позиции информатора

    def update_best_informant_position(self, positions, loss_func, network, train_data, train_labels):
        best_local_loss = self.best_loss
        for pos in positions:
            # Применяем позицию как веса ANN
            network.set_weights(pos)
            # Вычисляем предсказания сети
            predictions = network.forward(train_data)
            # Вычисляем потери для данной позиции
            local_loss = loss_func(predictions, train_labels)
            if local_loss < best_local_loss:
                best_local_loss = local_loss
                self.best_informant_position = np.copy(pos)

    def update_velocity(self, global_best_position, w=0.5, c1=1, c2=2):

        inertia = w * self.velocity
        cognitive = c1 * \
            np.random.rand(*self.position.shape) * \
            (self.best_position - self.position)
        social = c2 * np.random.rand(*self.position.shape) * \
            (global_best_position - self.position)
        self.velocity = inertia + cognitive + social

    def update_position(self, bounds=None):
        self.position += self.velocity
        if bounds is not None:
            self.position = np.clip(self.position, bounds[0], bounds[1])

    def evaluate(self, loss_func, network, train_data, train_labels):
        # Применить веса к сети
        network.set_weights(self.position)
        # Выполнить прямое распространение с новыми весами
        predictions = network.forward(train_data)
        # Вычислить потери
        loss = loss_func(predictions, train_labels)
        if loss < self.best_loss:
            self.best_loss = loss
            self.best_position = np.copy(self.position)


# Шаг 2: Определение класса роя


# Определение класса роя (Swarm)
class Swarm:
    def __init__(self, dimensions, num_particles=30, num_informants=3):
        self.particles = [Particle(dimensions) for _ in range(num_particles)]
        self.global_best_loss = float('inf')
        # Инициализация global_best_position с позицией первой частицы
        self.global_best_position = np.copy(self.particles[0].position)
        self.num_informants = num_informants
        self.assign_informants()  # Вызываем метод для назначения информаторов

    def assign_informants(self):
        for particle in self.particles:
            informants = np.random.choice(
                [p for p in self.particles if p is not particle],
                self.num_informants,
                replace=False)
            particle.informants = informants

    def evaluate_global_best(self):
        for particle in self.particles:
            if particle.best_loss < self.global_best_loss:
                self.global_best_loss = particle.best_loss
                self.global_best_position = particle.best_position

    def update_particles(self, loss_func, network, train_data, train_labels):
        # Нам нужно сначала обновить глобальное лучшее, если это первая итерация
        self.evaluate_global_best()
        for particle in self.particles:
            # Сначала обновляем лучшую позицию информатора для каждой частицы
            informant_positions = [
                informant.best_position for informant in particle.informants]
            particle.update_best_informant_position(
                informant_positions, loss_func, network, train_data, train_labels)
            # particle.update_best_informant_position(informant_positions)
            # Затем обновляем скорость, учитывая глобальную лучшую позицию
            particle.update_velocity(self.global_best_position)
            # Обновляем позицию и вычисляем новую потерю
            particle.update_position()
            particle.evaluate(loss_func, network, train_data, train_labels)
            # Для дебага
            print(
                f"Particle Position: {particle.position} Loss: {particle.best_loss}")
        # После обновления всех частиц, снова оцениваем глобальное лучшее
        self.evaluate_global_best()


# Шаг 3: Запуск PSO


def pso_test(num_particles, num_iterations, loss_func, network, train_data, train_labels, dimensions):
    swarm = Swarm(dimensions, num_particles)

    for i in range(num_iterations):
        # Обновляем частицы роя с учетом новой функции потерь и данных
        swarm.update_particles(loss_func, network, train_data, train_labels)
        print(
            f"Iteration {i+1}/{num_iterations}, Best Loss: {swarm.global_best_loss}")

    return swarm.global_best_position
