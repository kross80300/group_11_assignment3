import re
from pathlib import Path
import os
import string
from collections import Counter, defaultdict

def load_file():
    # Make path relative to this test file's directory
    path = "frankenstein.txt"
    with open(path, "r", encoding="utf-8") as f:
        return f.read()



def extract_words(text):
    """Return list of words from text and changes to lowercase."""
    text = text.lower()
    words = re.compile(r"[a-z]+")
    return [w for w in words.findall(text)]

def allwords(words):
    """Writes all words to allwords.txt."""
    with open("allwords.txt", "w") as f:
        for w in words:
            f.write(w + "\n")

def uniquewords(words):
    """Writes unique words to uniquewords.txt."""
    count = Counter(words)
    with open("uniquewords.txt", "w") as f:
        for w, count in sorted(count.items()):
            if count == 1:
                f.write(w + "\n")

def wordfrequency(words):
    """Writes word frequency distribution to wordfrequency.txt."""
    count = Counter(words)
    freqs = defaultdict(int)
    for w, c in count.items():
        freqs[c] += 1
    with open("wordfrequency.txt", "w") as f:
        for freq in sorted(freqs.keys()):
            f.write(f"{freq} {freqs[freq]}\n")

def main():
    #print("Running...") prints for debugging
    #print("Working dir:", os.getcwd()) prints working directory for debugging
    text = load_file()
    #print("The text:" + text)
    words = extract_words(text)
    #print("The words:" + str(words))
    allwords(words)
    uniquewords(words)
    wordfrequency(words)




if __name__ == "__main__":
    main()