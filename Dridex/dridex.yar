rule dridex
{

    meta:
        author = "Finch"
        
    strings:
        $s1 = {21 09 AA 78 A2 3A FD 4D}
        $s2 = {69 7A E3 FF 5B 6B E0 FF}
        $s3 = {D3 DE D3 E3 21 2C 2A 12}
        $s4 = {C2 E3 63 21 22 81 BD E2}

    condition:
        all of them and filesize < 200KB
}
