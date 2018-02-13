import os

thisfolder =os.getcwd()

os.chdir("./Classifier2/testscript2D/PythonApplication5/")
os.system("python Repeater.py 4 5 2")
os.chdir(thisfolder)

os.chdir("./Classifier2/testscript2.5D/PythonApplication5/")
os.system("python Repeater.py 4 5 2")
os.chdir(thisfolder)

os.chdir("./Classifier2/testscript3D/PythonApplication5/")
os.system("python Repeater.py 4 5 2")
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