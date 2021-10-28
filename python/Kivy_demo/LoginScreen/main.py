from kivy.app import App
from kivy.uix.screenmanager import Screen
from kivy.lang import Builder
from kivy.config import Config
Config.set('graphics','width', 540)
Config.set('graphics','height', 960)

Login_kv = Builder.load_file("Login.kv")

class LoginApp(App):
    def build(self):
        return Login_kv 

if __name__ == "__main__":
    LoginApp().run()