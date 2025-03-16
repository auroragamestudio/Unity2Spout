import pygame
from OpenGL.GL import *
import os

# Configuration pour forcer le GPU spécifique (changer l'ID selon vos besoins)
os.environ["CUDA_VISIBLE_DEVICES"] = "0"  # GPU 0 pour la première fenêtre

# Initialisation de la première fenêtre
pygame.init()
window1 = pygame.display.set_mode((800, 600), pygame.OPENGL | pygame.DOUBLEBUF)
glClearColor(0.0, 0.0, 0.5, 1.0)  # Bleu pour GPU 0

# Création d'un deuxième contexte pour le second GPU
os.environ["CUDA_VISIBLE_DEVICES"] = "1"  # GPU 1 pour la seconde fenêtre
window2 = pygame.display.set_mode((800, 600), pygame.OPENGL | pygame.DOUBLEBUF)
glClearColor(0.5, 0.0, 0.0, 1.0)  # Rouge pour GPU 1

# Main loop
running = True
while running:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

    # Rendu pour la fenêtre 1
    glClear(GL_COLOR_BUFFER_BIT)
    pygame.display.flip()

    # Rendu pour la fenêtre 2
    glClear(GL_COLOR_BUFFER_BIT)
    pygame.display.flip()

pygame.quit()
