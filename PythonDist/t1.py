from revChatGPT.ChatGPT import Chatbot
import os
import sys

with open (os.path.join(os.path.dirname(__file__), "config.json"), "r") as myfile:
    data = myfile.read()
chatbot = Chatbot({
   "session_token": data
}, conversation_id=None, parent_id=None) # You can start a custom conversation

while True:
    user_input = input("Enter a string: ")
    response = chatbot.ask(user_input, conversation_id=None, parent_id=None) # You can specify custom conversation and parent ids. Otherwise it uses the saved conversation (yes. conversations are automatically saved)

    print(response)

    # do something with the string

# {
#   "message": message,
#   "conversation_id": self.conversation_id,
#   "parent_id": self.parent_id,
# }