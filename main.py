from kivy.app import App
from kivy.lang import Builder
from kivy.uix.gesturesurface import GestureSurface
from kivy.uix.gridlayout import GridLayout
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.layout import Layout
from kivy.uix.screenmanager import ScreenManager, Screen, SlideTransition
import kivy.properties as kyprops
from kivy.config import Config
Config.set('graphics','width', 540)
Config.set('graphics','height', 960)

class MainMenu(GridLayout):
    pass




class MultistrokeApp(App):        
    def build(self):
        self.manager = ScreenManager(transition=SlideTransition(
                                     duration=.01))
        


        # Screen1
        screen1_w = Label(text="LABEL SCREEN1")
        screen1 = Screen(name='screen1')
        screen1.add_widget(screen1_w)
        self.manager.add_widget(screen1)
        self.screen1 = screen1

        # Screen2
        screen2_w = Label(text="LABEL SCREEN2")
        screen2 = Screen(name='screen2')
        screen2.add_widget(screen2_w)
        self.manager.add_widget(screen2)
        self.screen2 = screen2

        # Screen3
        screen3_w = Label(text="LABEL SCREEN3")
        screen3 = Screen(name='screen3')
        screen3.add_widget(screen3_w)
        self.manager.add_widget(screen3)
        self.screen3 = screen3

        # Screen4
        screen4_w = Label(text="LABEL SCREEN4")
        screen4 = Screen(name='screen4')
        screen4.add_widget(screen4_w)
        self.manager.add_widget(screen4)
        self.screen4 = screen4

        # Screen5
        screen5_w = Label(text="LABEL SCREEN5")
        screen5 = Screen(name='screen5')
        screen5.add_widget(screen5_w)
        self.manager.add_widget(screen5)
        self.screen5 = screen5
        
        layout = GridLayout(cols=1)
        layout.add_widget(self.manager)
        layout.add_widget(MainMenu())
        return layout

if __name__ == "__main__":
    MultistrokeApp().run()