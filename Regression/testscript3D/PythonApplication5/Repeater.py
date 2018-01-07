import sys
import os
import shutil


if __name__ == "__main__":
    x = int(sys.argv[1])
    y = int(sys.argv[2])
    call = "python testscript.py " + str(y)
    storage_location = os.environ['HOME']+"/storage/Regression/3D/"
    for i in range(0,x):
        os.system(call)
        shutil.copytree("/tmp/Regression/3D", storage_location+str(y*(i+1)), ignore=None)