#Repeater.py runs the network x times with y steps and saves it every z runs.
import sys
import os
import shutil
import testscript

if __name__ == "__main__":
    x = int(sys.argv[1])
    y = int(sys.argv[2])
    z = int(sys.argv[3])
    #call = "python testscript.py " + str(y)
    storage_location = os.environ['HOME']+"/storage/Classifier2/2.5D/"
    testscript.numOfSteps=y
    for i in range(0,x):
        # os.system(call)
        testscript.main([]  )
        if (i%z == z-1 or i == x-1):
            shutil.copytree("/tmp/Classifier2/2.5D", storage_location+str(y*(i+1)), ignore=None)
        else:
            os.makedirs(storage_location+str(y*(i+1)))
        shutil.copy("./results.txt",storage_location+str(y*(i+1)))
        shutil.copy("./smallresults.txt",storage_location+str(y*(i+1)))
        shutil.copy("./largeresults.txt",storage_location+str(y*(i+1)))