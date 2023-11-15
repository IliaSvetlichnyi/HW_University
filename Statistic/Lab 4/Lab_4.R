require("datasets")
data("airquality")
head(airquality)

par(nfrow = c(1, 3))
plot(airquality$Solar.R, airquality$Ozone, col = "red")
plot(airquality$Temp, airquality$Ozone, col = "red")
plot(airquality$Wind, airquality$Ozone, col = "red")

lmSolar = lm(airquality$Ozone ~ airquality$Solar.R)
lmSolar

lmTemp = lm(airquality$Ozone ~ airquality$Temp)
lmTemp

lmWind = lm(airquality$Ozone ~ airquality$Wind)
lmWind


par(nfrow = c(1, 3))
plot(airquality$Solar.R, airquality$Ozone, col = "red")
abline(lmSolar, col = "blue")
plot(airquality$Temp, airquality$Ozone, col = "red")
abline(lmTemp, col = "blue")
plot(airquality$Wind, airquality$Ozone, col = "red")
abline(lmWind, col = "blue")

summary(lmSolar)
summary(lmTemp)
summary(lmWind)

lmAll = lm(Ozone~Solar.R + Wind + Temp, data = airquality)
lmAll
summary(lmAll)

lmTempWind = lm(Ozone~Wind + Temp, data = airquality)
lmTempWind
summary(lmTempWind)

anova(lmAll)