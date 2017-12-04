import io
import numpy as np


train = np.zeros((3626,16*16*16+1), dtype=np.float32)
eval = np.zeros((390,16*16*16+1), dtype=np.float32)

count = 0


f = open('TrainingData.txt', 'r')

for line in f:
    print(count)
    A=np.zeros( (16*16*16+1) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[itt] = line[itt]
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
    A[-1]=end

    train[count,:] = A
    count = count +1
    
f = open('GoalData.txt', 'r')    
count = 0    
for line in f:
    print(count)
    A=np.zeros( (16*16*16+1) )
    ## print(A)
    itt=0
    for i in range(0,16):
        for j in range(0,16):
            for k in range(0,16):
                A[itt] = line[itt]
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
    A[-1]=end
    eval[count,:] = A
    count = count +1