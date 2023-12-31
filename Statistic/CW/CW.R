# Loading the data
data = read.csv("/Users/ilya/Desktop/GitHub_Repositories/HW_University/Statistic/CW/filesizes.csv")

# Plotting a histogram
hist(data$x, main="Histogram of File Sizes", xlab="File Size (kB)", ylab="Frequency")

# Calculating numerical summaries
summary(data$x)
mean_value = mean(data$x)
sd_value = sd(data$x)
print(mean_value)
print(sd_value)


# Exercise 2 (calculating alpha value from MLE)
n = dim(data)[1]
x_m = 2000
sum_of_ln_xi = sum(log(data$x))
alpha_MLE = n / (sum_of_ln_xi - (n * log(x_m)))

# Exercise 3 (calculating fisher information for alpha)
fisher_information = n / alpha_MLE**2

# Calculating distribution of alpha_hat
print(sprintf("Alpha_hat has approximately N(%.3f, %.3f)", alpha_MLE, 1 / fisher_information))

# Exercise 4 (calculating alpha value from MME)
alpha_MME = mean_value / (mean_value - x_m)

# Exercise 5 (calculating a 90% CI for alpha)
lower_bound = alpha_MLE - 1.645 * (alpha_MLE / n**0.5)
upper_bound = alpha_MLE + 1.645 * (alpha_MLE / n**0.5)

cat(sprintf("A 90%% CI for alpha is (%f, %f)", round(lower_bound, 3), round(upper_bound, 3)))

# Exercise 6 (compering alpha value from MLE and MME)
print(alpha_MLE)
print(alpha_MME)


# Exercise 7 (Mean simulation)
library(VGAM)

# Number of simulations
num_simulations = 1000

# Array to store Y' for each simulation
Y_primes = numeric(num_simulations)

for (i in 1:num_simulations) {
  # Simulate file sizes for each sample
  file_sizes <- rpareto(1500, x_m, alpha_MLE)
  
  # Calculate Y' for this sample and store it
  Y_primes[i] = mean(file_sizes)
}

# Plot a histogram of the Y' values
hist(Y_primes, main="Histogram of Y' across simulations", xlab="Y' (Mean File Size)", ylab="Frequency")

# Output numerical summaries of Y'
summary(Y_primes)
mean(Y_primes)
sd(Y_primes)

