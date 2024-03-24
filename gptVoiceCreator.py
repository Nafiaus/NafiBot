
# gptVoiceCreator.py
# 
# Just add the text you want converted to speech using cmd line arguments
# EX: gptVoiceCreator.exe nameOfFile extensionOfFile I want this converted to voice.
# 

from pathlib import Path
import openai
import sys
import os

client = openai.OpenAI(api_key="your openai api key")

argLength = len(sys.argv)
argList = []

for x in range(3, argLength):
       argList.append(sys.argv[x])
       
string = ' '.join(argList)

speech_file_path = os.path.join("path", "to", "gpt", "voices")
speech_file_name = sys.argv[1]
speech_file_extension = sys.argv[2]
speech_file_name_extension = '.'.join([ speech_file_name, speech_file_extension])
speech_file = os.path.join(speech_file_path, speech_file_name_extension)

print(speech_file)
print(string)

response = client.audio.speech.create(
    model="tts-1-hd",
    voice="onyx",
    input=string
)

response.stream_to_file(speech_file)
