# Monk
Common bytes seeker for Malware identification.
## Explaination
- increaser = How many bytes start to parse, example: [0:n] -> [0+n:n+n] -> [0+n+n:n+n+n] etc....
- increase = If after finished to parse the entire file restart again increasing the [increaser] (y/n)
- increase_v = How many bytes add to increaser after restarting
- times = How many times add bytes number to parse to [increase], example: if [increase_v] == 2 and [times] == 2 and [increase] == "y" and [increaser] = 8, the first parse will be
[0:increaser] and etc...., the second time will be [0:increaser+increase_v] (let's call it temp_value), the third time will be [0:temp_value+increase_v].

[Video](https://youtu.be/lk6bFiqNY6o) [v0,1]


## Report
The report is divided in:

Bytes | Jaro rate

The work to indentify if a match is unique and valuable for the analysis is work of the analyst.

[v0.1]

P.S: The example report is the comparison of the same executable, see images [In this case the assemby is correct].

![](https://raw.githubusercontent.com/Finch4/Monk/main/hex.png?token=AMRB74AGCTDGDA7CXFGAW7K77HAUY)

![](https://raw.githubusercontent.com/Finch4/Monk/main/assembly.png?token=AMRB74BLJJQMLPHHAS4UZ7277HAVU)

![](https://raw.githubusercontent.com/Finch4/Monk/main/match.png?token=AMRB74CD5DNF3O4FV62Q4W277HAW4)

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
