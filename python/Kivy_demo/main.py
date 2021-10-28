from kivy.app import App
from kivy.uix.gridlayout import GridLayout
from kivy.uix.anchorlayout import AnchorLayout
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.scrollview import ScrollView
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.layout import Layout
from kivy.uix.screenmanager import ScreenManager, Screen, SlideTransition
from kivy.graphics import Rectangle
from kivy.graphics import Color
import sqlalchemy
import webbrowser
from sqlalchemy.orm import mapper
from sqlalchemy import create_engine
from sqlalchemy import Table, Column, Integer, String, MetaData, ForeignKey
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker
from kivy.core.window import Window
from sqlalchemy import create_engine
#from kivy.core.window import Window
#Window.size = (540,960)
#from kivy.config import Config
#Config.set('graphics','width', 540)
#Config.set('graphics','height', 960)
Window.size= (540,960)
#Window.clearcolor = (1, 1, 1, 1)


#Base = declarative_base()  # ALARM ALARM ALARM ALARM ALARM ALARM TODO: TODO:
#class Product(Base):
#    __tablename__ = 'product'
#    maker = Column(String)
#    model = Column(String, primary_key=True)
#    type = Column(String)

#    def __init__(self,maker,model,type):
#        self.maker = maker
#        self.model = model
#        self.type = type

#    def __repr__(self):
#        return "<Product('%s','%s','%s')>" % (self.maker, self.model, self.type)

class MainMenu(GridLayout):
    pass
class NeckSpace(Button):
    pass
class Screen_news(Screen):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        engine = create_engine('mysql://root:5533@localhost/computers')
        Session = sessionmaker(bind=engine)
        session = Session()
        products = session.query(Product).all()

        layout = GridLayout(cols=1, size_hint_y=None)
        layout.bind(minimum_height=layout.setter('height'))
        for prod in products:
            widget = MyProductWidget(prod,height=50,size_hint=(1,None))
            layout.add_widget(widget)

        root = ScrollView(size_hint=(1, None), size=(Window.width, Window.height-150)) # -150 for neckspace 50px -_-
        root.add_widget(layout)                                           # 3 widgets in background => -50*3 = - 150
        self.add_widget(root)

class Screen_league(Screen):
    pass
class Screen_user(Screen):
    pass
class Screen_schedule(Screen):
    pass
class Screen_more(Screen):
    @staticmethod
    def OpenURL(URL):
        webbrowser.open(URL)
    pass
class Background(GridLayout):
    pass


class MyProductWidget(BoxLayout):
    def __init__(self, product, **kwargs):
        super(MyProductWidget, self).__init__(**kwargs)
        self.add_widget(Label(text=product.maker, color=(1,1,1,1),font_size='15sp'))
        self.add_widget(Label(text=product.model, color=(0,0,0,1),font_size='15sp'))
        self.add_widget(Label(text=product.type, color=(1,1,1,0.5),font_size='15sp'))

class Product(object):     #Task separating style. #For inserting
    def __init__(self,_maker,_model,_type):
        self.maker = _maker
        self.model = _model
        self.type = _type

    def __repr__(self):
        return "<Product('%s','%s','%s')>" % (self.maker, self.model, self.type)


class MultistrokeApp(App):        
    def build(self):
        self.manager = ScreenManager(transition=SlideTransition(
                                     duration=.1))
        
        # MySQL via SqlAlchemy area. Task separating style.
        #engine = create_engine('mysql://root:5533@localhost/computers')
        metadata = MetaData()
        products_table = Table('product', metadata,
        Column('maker',String),
        Column('model', String, primary_key=True),
        Column('type',String))

        mapper(Product, products_table)
        # End of MySQL via SqkAkchemy area. Task separating style.

        # MySQL via SqlAlchemy area. Declarative style
        #engine = create_engine('mysql://root:5533@localhost/computers')
        #Session = sessionmaker(bind=engine)
        #session = Session()
        #testProd = Product("maker " + str(i) ,"model "+str(i),"type "+str(i)) # Add new product
        #session.add(testProd)
        #session.commit()
        # End of MySQL via SqlAlchemy area. Declarative style

        # Screen1
        self.manager.add_widget(Screen_news(name="screen1"))
        self.Screen_news = Screen_news()
        self.manager.current="screen1"

        # Screen2
        self.manager.add_widget(Screen_league())
        self.Screen_league = Screen_league()

        # Screen3
        self.manager.add_widget(Screen_user())
        self.Screen_user = Screen_user()

        # Screen4
        self.manager.add_widget(Screen_schedule())
        self.Screen_schedule = Screen_schedule()

        # Screen5
        #screen5_w = Label(text="LABEL SCREEN5") # Adding widgets to screen5
        #screen5.add_widget(screen5_w)
        #screen5 = Screen_more() # Может быть излишество
        self.manager.add_widget(Screen_more())
        self.Screen_more = Screen_more()
        
        layout = Background()
        layout.add_widget(NeckSpace())
        layout.add_widget(self.manager)
        layout.add_widget(MainMenu())
        return layout

if __name__ == "__main__":
    MultistrokeApp().run()