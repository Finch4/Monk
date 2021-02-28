rule .net_packed_mal
{

    meta:
        author = "Finch"
        info = "https://github.com/Finch4/Monk"
        
    strings:
        $hex = {00 00 B1 8F 0B FC 61 05}
		    $hex_2 = {6F 63 00 00 06 6F ?? 00}

    condition:
        all of them
}
