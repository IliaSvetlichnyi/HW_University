import numpy as np

def mse_loss(predictions, targets):
    return np.mean((predictions - targets) ** 2)