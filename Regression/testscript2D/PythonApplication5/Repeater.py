import sys
import os
import shutil


if __name__ == "__main__":
    x = int(sys.argv[1])
    y = sys.argv[2]
    call = "python testscript.py " + y
    storage_location = os.environ['HOME']+"/storage/Regression/2D/"
    for i in range(0,x):
        os.system(call)
        shutil.copytree("/tmp/Regression/2D", storage_location+str((x+1)*i), ignore=None)