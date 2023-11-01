# generating a sample from uniform distribution
# runif(20, 0, 5)

# generating a sample from uniform distribution

theta = 5 # value of theta
m = 1000 # number of samples
n = 20 # sample size
alpha = 0.05 # level of confidence

MLE = numeric(0)
CI_L = numeric(0)
CI_U = numeric(0)

counter = 0

for (i in 1:m) {
  x = runif(n, 0, theta) # generate a random sample of size n from U(0, theta)
  y = sort(x) # sort the sample
  MLE[i] = y[20] 
  CI_L[i] = MLE[i] / qbeta(1 - alpha/2, n, 1)
  CI_U[i] = MLE[i] / qbeta(alpha/2, n, 1)
  if (CI_L[i] < theta & CI_U[i] > theta) counter = counter + 1
}

counter


