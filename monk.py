import binascii
from capstone import *
import datetime


def disassemble(code):
    md = Cs(CS_ARCH_X86, CS_MODE_32)
    for i in md.disasm(code, 0x1000):
        return("\t%s\t%s" % (i.mnemonic, i.op_str))

def analyze(file_1,file_2,increaser,increase="n",increase_v=0,times=0):
    increaser *= 2
    increase_v *= 2
    counter_1 = 0
    counter_2 = increaser
    file_1_lines = open(file_1,"rb").read()
    file_2_lines = open(file_2,"rb").read()
    hexdata_1 = binascii.hexlify(file_1_lines)
    hexdata_2 = binascii.hexlify(file_2_lines)
    min = 0
    if len(hexdata_1) > len(hexdata_2):
        min = len(hexdata_2)
    else:
        min = len(hexdata_1)

    i = 0
    p = 0
    temp_value = 0
    report = open(f"{str(datetime.datetime.today()).replace(':','').replace(' ','')}.txt", "w",encoding='ascii')
    if increase == "n":
        while i <= min:
            if hexdata_1[counter_1:counter_2] in hexdata_2 and len(hexdata_1[counter_1:counter_2]) > 0 and hexdata_1[counter_1:counter_2].count(b"00") < len(hexdata_1[counter_1:counter_2])/2:
                p += 1
                report.write(f"Match: {hexdata_1[counter_1:counter_2]} : {binascii.unhexlify(hexdata_1[counter_1:counter_2])} : {disassemble(binascii.unhexlify(hexdata_1[counter_1:counter_2]))}\n")
                counter_1 += increaser
                counter_2 += increaser
                i += 1
                continue
            else:
                counter_1 += increaser
                counter_2 += increaser
                i += 1
                continue
        report.write(str(round(p/100,4)))
    else:
        inc = 0
        while inc <= times:
            while i != min:
                if hexdata_1[counter_1:counter_2] in hexdata_2 and len(hexdata_1[counter_1:counter_2]) > 0 and hexdata_1[counter_1:counter_2].count(b"00") < len(hexdata_1[counter_1:counter_2])/2:
                    p += 1
                    report.write(f"Match: {hexdata_1[counter_1:counter_2]} : {binascii.unhexlify(hexdata_1[counter_1:counter_2])}\n")
                    counter_1 += increaser
                    counter_2 += increaser
                    i += 1
                    continue
                else:
                    counter_1 += increaser
                    counter_2 += increaser
                    i += 1
                    continue
            inc += 1
            counter_1 = 0
            counter_2 = 0
            if inc != times:
                counter_2 = increaser+increase_v
                temp_value = counter_2
            else:
                counter_2 = temp_value+increase_v
            i = 0
            report.write(str(round(p / 100, 4)))
            report.write("-------------------------------------------\n")
            p = 0
            continue


    report.close()


analyze("YOUR_FILE","YOUR_FILE",16,"n",4,3)



