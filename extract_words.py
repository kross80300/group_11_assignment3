from collections import defaultdict
import re

def main():

    with open("frankenstein.txt", "r") as file:
        text = file.read().lower()

    words = re.findall(r"[a-z]+", text)

    with open("allwords.txt", "w") as file:
        for word in words:
            file.write(word+"\n")

    uniqueWords = defaultdict(int)
    for word in words:
        uniqueWords[word] +=1

    with open("uniquewords.txt", "w") as file:
        for word, count in uniqueWords.items():
            if count==1:
                file.write(word + "\n")

    frequency = defaultdict(int)
    for count in uniqueWords.values():
        frequency[count] +=1

    with open("wordfrequency.txt", "w") as file:
        for freqVal, count in sorted(frequency.items()):
            file.write(f"{freqVal}: {count}\n")

if __name__ == "__main__":
    main()
