import binascii
import sys
import textdistance

from capstone import *
import datetime

class monk():

    def filter(self,code,filter):
        codes = list(open(code,"r").readlines())
        filters = list(open(filter,"r").readlines())
        filtered_report = open(f"filtered_{code}","w")
        for i in codes:
            if i in filters:
                pass
            else:
                filtered_report.write(i)
        filtered_report.close()





    def disassemble(self,code):
        md = Cs(CS_ARCH_X86, CS_MODE_32)
        for i in md.disasm(code, 0x1000):
            return ("\t%s\t%s" % (i.mnemonic, i.op_str))


    def analyze(self,file_1, file_2, increaser, fuzzy_rate, increase="n", increase_v=0, times=0):

        increaser *= 2

        increase_v *= 2
        counter_1 = 0
        counter_2 = increaser
        report_list = []
        file_1_lines = open(file_1, "rb").read()
        file_2_lines = open(file_2, "rb").read()
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
        report = open(f"{str(datetime.datetime.today()).replace(':', '').replace(' ', '')}.txt", "w", encoding='ascii')
        if increase.lower() == "n":
            while i <= min:
                jaro = textdistance.jaro(hexdata_1[counter_1:counter_2],hexdata_2[counter_1:counter_2])
                if jaro > fuzzy_rate and len(hexdata_1[counter_1:counter_2]) > 0 and hexdata_1[counter_1:counter_2].count(b"00") < len(hexdata_1[counter_1:counter_2]) / 2:
                    p += 1
                    report.write(
                        f"Match: {hexdata_1[counter_1:counter_2]} : {binascii.unhexlify(hexdata_1[counter_1:counter_2])} : {self.disassemble(binascii.unhexlify(hexdata_1[counter_1:counter_2]))} : Jaro Matching: {jaro}\n")
                    counter_1 += increaser
                    counter_2 += increaser
                    i += 1
                    continue
                else:
                    counter_1 += increaser
                    counter_2 += increaser
                    i += 1
                    continue
            report.write(str(round(p / 100, 4)))
        else:
            inc = 0
            while inc <= times:
                while i != min:
                    jaro = textdistance.jaro(hexdata_1[counter_1:counter_2], hexdata_2[counter_1:counter_2])
                    if jaro > fuzzy_rate and len(hexdata_1[counter_1:counter_2]) > 0 and hexdata_1[counter_1:counter_2].count(b"00") < len(hexdata_1[counter_1:counter_2]) / 2:
                        p += 1
                        report.write(
                            f"Match: {hexdata_1[counter_1:counter_2]} : {binascii.unhexlify(hexdata_1[counter_1:counter_2])} : Jaro Matching: {jaro}\n")
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
                    counter_2 = increaser + increase_v
                    temp_value = counter_2
                else:
                    counter_2 = temp_value + increase_v
                i = 0
                report.write(str(round(p / 100, 4)))
                report.write("-------------------------------------------\n")
                p = 0
                continue

        report.close()



if __name__ == "__main__":
    monk = monk()
    print\
    ("""
    Choose:
    [1] Compare
    [2] Filter
    """)
    choose = int(input())
    if choose == 1:
        file_1 = input("First file: ")
        file_2 = input("Second file: ")
        bytes = int(input("How many bytes?: "))
        rate = float(input("Minimum comparison rate (0.0 -> 1): "))
        restart = input("Want to restart after finished? (y/n): ")
        if restart.lower() == "n":
            monk.analyze(file_1,file_2,bytes,rate,"n",0,0)
        else:
            hmany = int(input("How many bytes to add?: "))
            htimes = int(input("How many times?: "))
            monk.analyze(file_1,file_2,bytes,rate,"y",hmany,htimes)
    elif choose == 2:
        file = input("First file: ")
        filter = input("Filter: ")
        monk.filter(file,filter)
    else:
        print("Incorrect decision")
