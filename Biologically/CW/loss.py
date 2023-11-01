import numpy as np

def mse_loss(predictions, targets):
    return np.mean((predictions - targets) ** 2)

def cross_entropy(predictions, targets, epsilon=1e-10):
    """
    Computes cross entropy between targets (encoded as one-hot vectors)
    and predictions. 
    """
    predictions = np.clip(predictions, epsilon, 1. - epsilon)
    N = predictions.shape[0]
    ce_loss = -np.sum(targets*np.log(predictions + 1e-9)) / N
    return ce_loss
