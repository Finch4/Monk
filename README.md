# Monk
## Explaination
# What is Monk?
Monk, is a program that helps you find unique sequence of bytes which you can use in YARA to indentify malware families.

        
# How it works?

For reasons of formatting is better to read the explaination here: http://finch4.pythonanywhere.com/
 
[Video](https://youtu.be/F7T1lGaJmj8)

Logo created by: [Stefano Tech](https://www.youtube.com/channel/UCf-ZEAjv-Fo7UlFLXSo0zgg)



![](https://github.com/Finch4/Monk/blob/master/Monk%20Logo%202.0.png?raw=true)



![](https://github.com/Finch4/Monk/blob/master/Monk_1.PNG?raw=true)

Common bytes seeker for Malware identification.
[Website version](http://finch4.pythonanywhere.com/)
## General

Currently there are three versions of Monk:

 - Python (Missing the .json database function)

 - C# (Complete [until I get new ideas])

 - Website [Django] (Currently just the function to analyze, anyway need also some fixes)




# Questions
__[1]__ What is the filter? where can I get one?:

The filter is a comparison of same executables, which [the executable] doesn't contain any instruction aside the fundamentals (main and imports).

__[2]__ Can I help you? if yes, how?:

Yes, there are two ways to help me, you can modify the code and make it better [The project is open for everyone, everyone can modify it] or you can test the results with YARA and then post them in the issues with the tag "Results", check /v0.3/yrtest.py


# Updates
## v0.2
- Added a menu
- New filter function
## v0.3
- Added [Jaro](https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance) comparison algorithm [Thanks to [textdistance](https://pypi.org/project/textdistance/)] [Need to check better if this is the best algorithm for my project]
- Added new comparison function parameter:  ```float rate # [0.0 -> 1.0]``` used as minimum rate for the comparison
## v0.4
- Report more clear, removed useless/incorrect information
- Added new function to compare reports
## v0.5
### THE BIGGEST UPDATE
- Completely rewritten in C#
- Multithread
- GUI
- New function to compare bytes with a .json database
- Everything open source
