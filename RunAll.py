import os

thisfolder =os.getcwd()

os.chdir("./Classifier/testscript2D/PythonApplication5/")
os.system("python Repeater.py 320 5000 20")
os.chdir(thisfolder)

os.chdir("./Classifier/testscript2.5D/PythonApplication5/")
os.system("python Repeater.py 320 5000 20")
os.chdir(thisfolder)

os.chdir("./Classifier/testscript3D/PythonApplication5/")
os.system("python Repeater.py 320 5000 20")
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