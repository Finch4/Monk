rule darkside_dll 
{
  meta:
    author = "Finch"
    description = "Developed with Monk"
  strings:
	  $hex1 = {1E5C754766837C1E045C753F66837C1E}
	  $hex2 = {10FF75F?FF15B0080110??4???0?000?}

  condition:
	  all of them and uint16(0) == 0x5a4d and filesize >= 40KB and filesize <= 80KB
}

// YARA Output

darkside_dll .\156335b95ba216456f1ac0894b7b9d6ad95404ac7df447940f21646ca0090673.dll
0x2ba0:$hex1: 1E 5C 75 47 66 83 7C 1E 04 5C 75 3F 66 83 7C 1E
0x4b00:$hex2: 10 FF 75 F0 FF 15 B0 08 01 10 EB 46 68 00 00 01
0x4cb0:$hex2: 10 FF 75 F8 FF 15 B0 08 01 10 C7 45 FC 01 00 00
0x93d5:$hex2: 10 FF 75 FC FF 15 B0 08 01 10 C7 45 FC 00 00 00
darkside_dll .\2dcac9f48c3989619e0abd200beaae901852f751c239006886ac3ec56d89e3ef.dll
0x2ba0:$hex1: 1E 5C 75 47 66 83 7C 1E 04 5C 75 3F 66 83 7C 1E
0x4b00:$hex2: 10 FF 75 F0 FF 15 B0 08 01 10 EB 46 68 00 00 01
0x4cb0:$hex2: 10 FF 75 F8 FF 15 B0 08 01 10 C7 45 FC 01 00 00
0x93d5:$hex2: 10 FF 75 FC FF 15 B0 08 01 10 C7 45 FC 00 00 00

// HybridAnalysis Results

https://www.hybrid-analysis.com/yara-search/results/d8c329c22815e53e25aba6a2c77f84207ccf9559f4bd86fe159818b313d5e3ee

// Note

I developed the rule in a few seconds with Monk, obviously can be better.
