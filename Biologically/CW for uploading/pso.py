from swarm import Swarm


def pso_optimization(num_particles, num_iterations, loss_func, network, train_data, train_labels, dimensions):
    swarm = Swarm(dimensions, num_particles)

    for i in range(num_iterations):
        # Update swarm particles based on new loss function and data
        swarm.update_particles(
            loss_func, network, train_data, train_labels, i + 1)
        # Evaluate global best with current iteration value
        swarm.evaluate_global_best(i + 1)

        # Print current best loss every 25 iterations
        if (i + 1) % 25 == 0:
            print(
                f"Iteration {i+1}/{num_iterations}, Current Best Loss: {swarm.global_best_loss}")

    # Print final best loss and iteration number after all iterations
    print(
        f"Final Best Loss: {swarm.global_best_loss} on Iteration: {swarm.global_best_iteration}")
    
    return swarm.global_best_position



