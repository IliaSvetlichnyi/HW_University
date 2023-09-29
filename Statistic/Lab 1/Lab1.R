abalone<-read.table("//Users//ilya//Desktop//Lab 1//abalone.data", sep = ",", header = FALSE)
abalone
abalone$V1 # the lengths of my data
length(abalone$V1)
abalone_lengths<-abalone$V1[1:80] # 80 lengths of my sample data
stem(abalone_lengths)
hist(abalone$V2)
hist(abalone$V2, breaks = 80, xlab = "diameter, mm", ylab = "frequency", main = "Histogram of the diameter of abalonies") # breakes mean the numbers of bars in the histogram
plot(abalone$V1, abalone$V4, main = "length vs weight")
table(abalone$V6) # freq. table of the no. of rings
freq<-table(abalone$V6)
barplot(freq)
mean(abalone$V1)
sd(abalone$V1)
quantile(abalone$V1)
IQR(abalone$V1)
summary(abalone$V1)
summary(abalone)
