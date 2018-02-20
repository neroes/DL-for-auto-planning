#ConvNet.py takes the dataset of levels and converts it to the 16x16x16 datastructure used for the neural network.

import io
import numpy as np

f = open('../../../Data2/properties.txt'); #properties contains the values for how many training/test levels are present.
trainSize = int(f.readline())
evalSize = int(f.readline())
evalSmallSize = int(f.readline())
evalLargeSize = int(f.readline())
xtrain = np.zeros((trainSize,16,16,16), dtype=np.float32)
ytrain = np.zeros(trainSize, dtype=np.float32)
xeval = np.zeros((evalSize,16,16,16), dtype=np.float32)
yeval = np.zeros(evalSize, dtype=np.float32)
xsmalleval = np.zeros((evalSmallSize,16,16,16), dtype=np.float32)
ysmalleval = np.zeros(evalSmallSize, dtype=np.float32)
xlargeeval = np.zeros((evalLargeSize,16,16,16), dtype=np.float32)
ylargeeval = np.zeros(evalLargeSize, dtype=np.float32)
#trainName = np.chararray(trainSize)
evalName = np.chararray(evalSize,itemsize=64)
evalLargeName = np.chararray(evalLargeSize,itemsize=64)
evalSmallName = np.chararray(evalSmallSize,itemsize=64)
#trainName = ["" for x in range(trainSize)]
#evalName = ["" for x in range(evalSize)]

count = 0


f = open('../../../Data2/TrainingData.txt', 'r') #The training data file is converted in this section

for line in f:
    if count%100 == 0:
        print(count)
    A=np.zeros( (16,16,16) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[i,j,k] = line[itt]
                itt = itt + 1
    itt2 = -2
    i = 1
    end = 0
    while True:
        if (line[itt2] == ' '):
            break        
        end = end + int(line[itt2])*i
        i = i*10
        itt2 = itt2 -1
 #   trainName[count]=line[itt:itt2]
    xtrain[count,:,:,:] = A
    ytrain[count]=end
    count = count +1
    if count >= trainSize:
        break
    
f = open('../../../Data2/GoalData.txt', 'r')    #The goal data file is converted from this point onwards
count = 0    
for line in f:
    if count%100 == 0:
        print(count)
    A=np.zeros( (16,16,16) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[i,j,k] = line[itt]
                itt = itt + 1
    itt2 = -2
    i = 1
    end = 0
    while True:
        if (line[itt2] == ' '):
            break        
        end = end + int(line[itt2])*i
        i = i*10
        itt2 = itt2 -1
    evalName[count]=line[itt:itt2]
    xeval[count,:,:,:] = A
    yeval[count]=end
    count = count +1
    if count >= evalSize:
        break

f = open('../../../Data2/SmallGoalData.txt', 'r')    #For the results section, a smaller dataset is also created for small sized levels.
count = 0    
for line in f:
    if count%100 == 0:
        print(count)
    A=np.zeros( (16,16,16) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[i,j,k] = line[itt]
                itt = itt + 1
    itt2 = -2
    i = 1
    end = 0
    while True:
        if (line[itt2] == ' '):
            break        
        end = end + int(line[itt2])*i
        i = i*10
        itt2 = itt2 -1
    evalSmallName[count]=line[itt:itt2]
    xsmalleval[count,:,:,:] = A
    ysmalleval[count]=end
    count = count +1
    if count >= evalSize:
        break

f = open('../../../Data2/LargeGoalData.txt', 'r')    #For the results section, a smaller dataset is also created for larger sized levels.
count = 0    
for line in f:
    if count%100 == 0:
        print(count)
    A=np.zeros( (16,16,16) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[i,j,k] = line[itt]
                itt = itt + 1
    itt2 = -2
    i = 1
    end = 0
    while True:
        if (line[itt2] == ' '):
            break        
        end = end + int(line[itt2])*i
        i = i*10
        itt2 = itt2 -1
    evalLargeName[count]=line[itt:itt2]
    xlargeeval[count,:,:,:] = A
    ylargeeval[count]=end
    count = count +1
    if count >= evalLargeSize:
        break