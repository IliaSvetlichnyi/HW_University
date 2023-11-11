# Loading the data
data = read.csv("/Users/ilya/Desktop/GitHub_Repositories/HW_University/Statistic/CW/filesizes.csv")


# MLE calculating
log_likelihood <- function(params) {
  -sum(dpareto(x = data$x, shape = params[1], scale = params[2], log = TRUE))
}

result <- optim(par = c(1, x_m), fn = log_likelihood)
alpha_MLE_optim <- result$par[1]

print(alpha_MLE_optim)

