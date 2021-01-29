import binascii
import sys
import textdistance
import datetime

class monk():

    def compare_reports(self, report1, report2):
        codes = list(open(report1, "r").readlines())
        filters = list(open(report2, "r").readlines())
        compared_report = open(f"compared_report{report1}-{report2}", "w")
        for i in codes:
            if i in filters:
                compared_report.write(i)
            else:
                pass
        compared_report.close()

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
    def clear(self,string):
        return str(string.replace("b","").replace("'","")).encode("utf-8")

    
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
                    report.write(f"{hexdata_1[counter_1:counter_2]}\n")
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
                            f"{self.clear(hexdata_1[counter_1:counter_2])}")
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
    [3] Compare reports
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
    elif choose == 3:
        report1 = input("First report: ")
        report2 = input("Second report: ")
        monk.compare_reports(report1,report2)
    else:
        print("Incorrect decision")



