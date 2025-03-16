import wx
from wx.glcanvas import GLCanvas
from OpenGL.GL import *
from pyspout import SpoutReceiver


class StereoCanvas(GLCanvas):
    def __init__(self, parent):
        attrib_list = [wx.glcanvas.WX_GL_RGBA,  # Couleurs RGBA
                       wx.glcanvas.WX_GL_DOUBLEBUFFER,  # Double buffer
                       wx.glcanvas.WX_GL_DEPTH_SIZE, 16,  # Tampon de profondeur
                       wx.glcanvas.WX_GL_STEREO]  # Rendu stéréo
        super().__init__(parent, attribList=attrib_list)
        self.context = wx.glcanvas.GLContext(self)

        # Spout receivers pour les flux gauche et droit
        self.spout_left = SpoutReceiver()
        self.spout_right = SpoutReceiver()

        # Initialisation des récepteurs
        self.spout_left.create_receiver("Stereo_Left")
        self.spout_right.create_receiver("Stereo_Right")

        # Dimensions initiales
        self.width, self.height = 800, 600

        # Événements
        self.Bind(wx.EVT_PAINT, self.on_paint)
        self.Bind(wx.EVT_SIZE, self.on_size)
        self.init = False

    def init_gl(self):
        glEnable(GL_TEXTURE_2D)  # Activer la texture 2D
        glClearColor(0.0, 0.0, 0.0, 1.0)  # Fond noir
        self.init = True

    def on_size(self, event):
        self.width, self.height = self.GetClientSize()
        if self.GetContext():
            self.SetCurrent(self.context)
            glViewport(0, 0, self.width, self.height)

    def on_paint(self, event):
        self.SetCurrent(self.context)
        if not self.init:
            self.init_gl()
        self.render()

    def render(self):
        # Récupérer les textures depuis Spout
        left_texture = self.receive_spout(self.spout_left)
        right_texture = self.receive_spout(self.spout_right)

        # Rendu pour l’œil gauche
        glDrawBuffer(GL_BACK_LEFT)
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
        self.draw_texture(left_texture)

        # Rendu pour l’œil droit
        glDrawBuffer(GL_BACK_RIGHT)
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
        self.draw_texture(right_texture)

        self.SwapBuffers()

    def receive_spout(self, spout_receiver):
        """Récupère une texture depuis un flux Spout."""
        texture_id = glGenTextures(1)
        glBindTexture(GL_TEXTURE_2D, texture_id)

        # Recevoir l'image via Spout et charger la texture
        if spout_receiver.receive_texture(texture_id, GL_TEXTURE_2D, self.width, self.height):
            return texture_id
        else:
            glDeleteTextures([texture_id])
            return None

    def draw_texture(self, texture_id):
        """Dessine une texture sur tout l'écran."""
        if texture_id:
            glBindTexture(GL_TEXTURE_2D, texture_id)
            glBegin(GL_QUADS)
            glTexCoord2f(0.0, 0.0)
            glVertex2f(-1.0, -1.0)

            glTexCoord2f(1.0, 0.0)
            glVertex2f(1.0, -1.0)

            glTexCoord2f(1.0, 1.0)
            glVertex2f(1.0, 1.0)

            glTexCoord2f(0.0, 1.0)
            glVertex2f(-1.0, 1.0)
            glEnd()
            glBindTexture(GL_TEXTURE_2D, 0)
            glDeleteTextures([texture_id])


class MainFrame(wx.Frame):
    def __init__(self):
        super().__init__(None, title="Stereo Spout Receiver", size=(800, 600))
        self.canvas = StereoCanvas(self)
        self.Show()


if __name__ == "__main__":
    app = wx.App(False)
    frame = MainFrame()
    app.MainLoop()
