# Loading the data
data <- read.csv("/Users/ilya/Desktop/GitHub_Repositories/HW_University/Statistic/CW/filesizes.csv")

# Plotting a histogram
hist(data$x, main="Histogram of File Sizes", xlab="File Size (kB)", ylab="Frequency")

# Calculating numerical summaries
mean_value <- mean(data$x)
sd_value <- sd(data$x)
median_value <- median(data$x)
min_value <- min(data$x)
max_value <- max(data$x)
quantiles_value <- quantile(data$x, probs = c(0.25, 0.75))

# Printing all numerical summaries
print(paste("Mean Size:", round(mean_value, digits = 3)))
print(paste("Standard Deviation:", round(sd_value, digits = 3)))
print(paste("Median:", round(median_value, digits = 3)))
print(paste("Min:", round(min_value, digits = 3)))
print(paste("Max:", round(max_value, digits = 3)))
print(paste("25% Quantile:", round(quantiles_value[1], digits = 3)))
print(paste("75% Quantile:", round(quantiles_value[2], digits = 3)))