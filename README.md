# DL-for-auto-planning

## [Classifier2](Classifier2)
   The folder containing the 2D, 2.5D and 3D network implementations for the purpose of training
## [Combiner](Combiner)
   Used to combine files of levels already converted to the internal ConvNet data structure and generates the properties file needed for training network
## [Counter](Counter)
   Counts numer of instances of various levels
## [Instance Creator](InstanceCreator)
   Calculates every state possible for the maps given to it. and then prints out 1% of them
## Levels
   Levels used for training, consists of four different level types explained in the implementation section of the report.
## [MLConverter](MLConverter)
   Restores a level from it's convnet structure to it's original form
## [Manhattan Calculator](Manhattan_Calculator)
   calculates the manhattan distance for any convnet formattet level at any of the paths set by the program argument
## [RunnerV2](RunnerV2)
   tries to solve a level using the 3D network trained by classifier
