import io
import numpy as np

trainSize = 400
evalSize = 100
xtrain = np.zeros((trainSize,16,16,16), dtype=np.float32)
xeval = np.zeros((evalSize,16,16,16), dtype=np.float32)
ytrain = np.zeros(trainSize, dtype=np.float32)
yeval = np.zeros(evalSize, dtype=np.float32)
trainName = ["" for x in range(trainSize)]
evalName = ["" for x in range(evalSize)]

count = 0


f = open('../../../Data/TrainingData.txt', 'r')

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
    trainName[count]=line[itt:itt2]
    xtrain[count,:,:,:] = A
    ytrain[count]=end
    count = count +1
    if count >= trainSize:
        break
    
f = open('../../../Data/GoalData.txt', 'r')    
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