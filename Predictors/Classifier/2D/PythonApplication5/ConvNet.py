import io
import numpy as np

def reformat(text):
    xpredict = np.zeros((10,16,16,16), dtype=np.float32)


    count = 0


    ## f = open('testp.txt', 'r')
    f = text
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

        xpredict[count,:,:,:] = A
        count = count +1
    return xpredict