import io
import numpy as np


xtrain = np.zeros((3626,16,16,16), dtype=np.float32)
xeval = np.zeros((390,16,16,16), dtype=np.float32)
ytrain = np.zeros(3626, dtype=np.float32)
yeval = np.zeros(390, dtype=np.float32)

count = 0


f = open('TrainingData.txt', 'r')

for line in f:
    print(count)
    A=np.zeros( (16,16,16) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[i,j,k] = line[itt]
                itt = itt + 1
    itt = -2
    i = 1
    end = 0
    while True:
        if (line[itt] == ' '):
            break        
        end = end + int(line[itt])*i
        i = i*10
        itt = itt -1

    xtrain[count,:,:,:] = A
    ytrain[count]=end
    count = count +1
    
f = open('GoalData.txt', 'r')    
count = 0    
for line in f:
    print(count)
    A=np.zeros( (16,16,16) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[i,j,k] = line[itt]
                itt = itt + 1
    itt = -2
    i = 1
    end = 0
    while True:
        if (line[itt] == ' '):
            break        
        end = end + int(line[itt])*i
        i = i*10
        itt = itt -1

    xeval[count,:,:,:] = A
    yeval[count]=end
    count = count +1