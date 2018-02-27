import os
# save current folder as return point
thisfolder =os.getcwd()
# browse to the the location of the 2D network
os.chdir("./Classifier2/testscript2D/PythonApplication5/")
os.system("python Repeater.py 320 5000 80") # call the repeater to run 320 instances of 5000 steps saving the network every 80th run/
os.chdir(thisfolder) # return to the return point

# browse to the the location of the 2.5D network
os.chdir("./Classifier2/testscript2.5D/PythonApplication5/")
os.system("python Repeater.py 320 5000 80") # call the repeater to run 320 instances of 5000 steps saving the network every 80th run/
os.chdir(thisfolder) # return to the return point

# browse to the the location of the 3D network
os.chdir("./Classifier2/testscript3D/PythonApplication5/")
os.system("python Repeater.py 320 5000 80") # call the repeater to run 320 instances of 5000 steps saving the network every 80th run/
#os.chdir(thisfolder)

#os.chdir("./Regression/testscript2D/PythonApplication5/")
#os.system("python Repeater.py 20 20000")
#os.chdir(thisfolder)

#os.chdir("./Regression/testscript2.5D/PythonApplication5/")
#os.system("python Repeater.py 20 20000")
#os.chdir(thisfolder)

#os.chdir("./Regression/testscript3D/PythonApplication5/")
#os.system("python Repeater.py 20 20000")

raw_input('Press Enter to exit')