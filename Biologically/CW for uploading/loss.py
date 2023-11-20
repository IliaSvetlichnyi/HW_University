import numpy as np


def mse_loss(predictions, targets):
    return np.mean((predictions - targets) ** 2)


def sparse_categorical_crossentropy(predictions, targets, epsilon=1e-10):
    """
    Computes the sparse categorical crossentropy loss between the predicted and target values.

    Args:
        predictions (numpy.ndarray): The predicted values of shape (batch_size, num_classes).
        targets (numpy.ndarray): The target values of shape (batch_size,).
        num_classes (int): The number of classes in the classification problem.
        epsilon (float): A small value to avoid division by zero.

    Returns:
        float: The mean sparse categorical crossentropy loss.
    """
    # print(f"Targets shape in sparse_categorical_crossentropy: {targets.shape}")
    # print(f"Predictions shape in sparse_categorical_crossentropy: {predictions.shape}")

    num_classes = predictions.shape[1]

    # Make sure that targets contains the appropriate indexes
    if np.any(targets >= num_classes) or np.any(targets < 0):
        raise ValueError("Targets contain invalid class indices.")
    # Clip predictions to avoid log(0) errors
    predictions = np.clip(predictions, epsilon, 1. - epsilon)

    # Select the predicted probabilities for the true class labels
    p = predictions[np.arange(predictions.shape[0]), targets]

    # Compute the negative log-likelihood loss
    loss = -np.log(p)

    # Compute the mean loss over the batch
    return np.mean(loss)
