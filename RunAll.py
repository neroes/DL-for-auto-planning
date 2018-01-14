import os

thisfolder =os.getcwd()

os.chdir("./Classifier/testscript2D/PythonApplication5/")
os.system("python Repeater.py 2 5")
os.chdir(thisfolder)

os.chdir("./Classifier/testscript2.5D/PythonApplication5/")
os.system("python Repeater.py 2 5")
os.chdir(thisfolder)

os.chdir("./Classifier/testscript3D/PythonApplication5/")
os.system("python Repeater.py 2 5")
os.chdir(thisfolder)

os.chdir("./Regression/testscript2D/PythonApplication5/")
os.system("python Repeater.py 2 5")
os.chdir(thisfolder)

os.chdir("./Regression/testscript2.5D/PythonApplication5/")
os.system("python Repeater.py 2 5")
os.chdir(thisfolder)

os.chdir("./Regression/testscript3D/PythonApplication5/")
os.system("python Repeater.py 2 5")

raw_input('Press Enter to exit')