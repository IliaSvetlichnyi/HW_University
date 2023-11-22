from swarm import Swarm

# Global variable to store loss history as a dictionary
global_best_loss_history = {}


def pso_optimization(num_particles, num_iterations, loss_func, network, train_data, train_labels, dimensions):
    global global_best_loss_history
    swarm = Swarm(dimensions, num_particles)
    global_best_loss_history.clear()  # Clear history before starting the optimization

    for i in range(num_iterations):
        # Update swarm particles based on new loss function and data
        swarm.update_particles(
            loss_func, network, train_data, train_labels, i + 1)
        # Evaluate global best with current iteration value
        swarm.evaluate_global_best(i + 1)
        # Store the current global best loss with the iteration number
        global_best_loss_history[i + 1] = swarm.global_best_loss

        if (i + 1) % 25 == 0:
            print(
                f"Iteration {i+1}/{num_iterations}, Current Best Loss: {swarm.global_best_loss}")

    # Find the iteration with the best loss
    best_iteration = min(global_best_loss_history,
                         key=global_best_loss_history.get)
    print(
        f"Final Best Loss: {global_best_loss_history[best_iteration]} on Iteration: {best_iteration}")
    return swarm.global_best_position
