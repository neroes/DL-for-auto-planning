import io
import numpy as np


xtrain = np.zeros((75,70,70,72), dtype=np.float32)
xeval = np.zeros((25,70,70,72), dtype=np.float32)
ytrain = np.zeros(75, dtype=np.float32)
yeval = np.zeros(25, dtype=np.float32)

count = 0

f = open('data.txt', 'r')

for line in f:
    print(count)
    A=np.zeros( (70,70,72) )
    ## print(A)
    itt=0
    for i in range(0,70):
        for j in range(0,70):
            for k in range(0,72):
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

    
    if (count<75):
        xtrain[count,:,:,:] = A
        ytrain[count]=end
    else:
        xeval[count-75,:,:,:] = A
        yeval[count-75]=end
    count = count +1
