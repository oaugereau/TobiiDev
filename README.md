eyex
====

Eye tracking software and logger for the Tobii EyeX Controller

****RecordingEyeGaze****
Record the eye gaze, the 3d position of the head and take a screenshot of the read text
1/ Launch the program
2/ Enter a file name (without extention) : this name will be use for all the output files.
3/ Record the data : press a key to start/pause the recording
4/ When the system is in pause, press "q" to exit

3 files will be written next to the .exe : "filename.csv" countain the eyegaze data, "filename eyePos.csv" contain the position of the eyes and "filename back.png" contain the screenshot.

****ReadingEyeGaze****
Create a video and a picture of the eye gaze on the screenshot.
Line breack detection is implemented here.

****RealTimeTagCloud****
A demo for creating a tag cloud based on the nearest read words.

****PrivateReading****
A demo for hidding the paragraphs that are not read.

****ComparingEyeGaze***
A demo for displaying two eye gaze at one time in a video.

****GroundTruth****
A software for creating the groundtruth of line break detection.
