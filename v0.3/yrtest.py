import yara
import textdistance

# Try Monk comparison and filter with YARA and then post the results here: https://github.com/Finch4/Monk/issues with the tag "Results"
rule = yara.compile(source='rule foo: bar {strings: $a = {} condition: $a}')
matches = rule.match(data=f'{open("YOUR_SAMPLE","rb").readlines()}')

for i in matches:
    print(matches)

# Some of the algorithms in textdistance; actually there are 30+
jaro = textdistance.jaro_winkler("0a00027b02000004","0a00027b02000004")
jaccard = textdistance.jaccard("a","a")
cosine = textdistance.cosine("a","a")
wunsch = textdistance.needleman_wunsch("a","a")
mra = textdistance.mra("a","a")

print(f"""
    Jaro: {jaro},
    Jaccard: {jaccard},
    Cosine: {cosine},
    Wunsch: {wunsch},
    Mra: {mra}
    """
    )
