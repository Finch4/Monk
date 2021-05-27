# Monk
## Explanation
# What is Monk?
Monk, is a program that helps you find unique sequence of bytes which you can use in YARA to indentify malware families.

        
# How it works?

<pre>
What is Monk?
        
Monk, is a program that helps you find unique sequence of bytes which you can use in YARA to indentify malware families.

        How it works?

[Attention, at the moment all this explaination is valid just for C# version of Monk, read "about" for the github release]
        
Remember:
    if jaro_rate (Jaro Similarity) (of the sequences of bytes compared) > {your_input_rate} = print in the report
          
[between * are the bytes compared in every scan]
        
    First Scan
   
  
    Sample 1
    Sample 2
  
    04000000ffff0000
    *04000000ffff0000*
  
  
    656375726974793E
    *6C7365222F3E0D0A*
  


    Second Scan
   
  
    Sample 1
    Sample 2
  
  
    4d5a900003000000
    *4d5a900003000000*
  
  
    *04000000ffff0000*
    *04000000ffff0000*
  
  
    656375726974793E
    *6C7365222F3E0D0A*
  


    Third Scan
   
  
    Sample 1
    Sample 2
  
  
    4d5a900003000000
    *4d5a900003000000*
  
  
    04000000ffff0000
    *04000000ffff0000*
  
  
    *656375726974793E*
    *6C7365222F3E0D0A*
  



As you can understand, this will take a lot of time on large files, so I suggest to cut with an Hex Editor the interested part or dump the section if is a PE.

[1]:
In the new version v0.6 it removes every 0 before calculate the jaro_rate, why this? because I figured out that useless sequence
of bytes were considerated useful just because there were some zeroes, so the zeroes were increasing the jaro rate.

[2]:
Imagine there are two sequences of bytes (bytes_1 = "0A1FB40E580B509C8" | bytes_2 = "0A1FB20E14B409CD", now they can be similar, but YARA doesn't work with jaro
instead there are the wild-cards: "?", Monk automatically compare every nibbles:



if bytes_1[i] == bytes_2[i]:
          pass
else:
          bytes_1[i] = "?"
          bytes_2[i] = "?" (Anyway just one sequence of bytes will be written, they will be equal)


Now doing this in our example it makes:
0A1FB?0E??B?09C?

The .json database
    

[This need to be fixed due the new methods that removes 0 and apply wild-cards]

Is where you can store your unique bytes in a very simple json and import it in Monk, it scan the database everytime you make a new analysis and add the matches in the table.
</pre>
 
[Video](https://youtu.be/F7T1lGaJmj8) [Old Version]

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
