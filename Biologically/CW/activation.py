# This file implements activation functions that can be used in neural layers

import numpy as np

# returns value between -1 and 1
# implement some limitations in order to avoid exceptions of gion value of "x"


def logistic(x):
    x = np.clip(x, -500, 500)
    return 1 / (1 + np.exp(-x))

# if input_value is positive returns it, otherwise returns 0


def relu(x):
    return np.maximum(0, x)

# returns value between -1 and 1


def tanh(x):
    return np.tanh(x)
