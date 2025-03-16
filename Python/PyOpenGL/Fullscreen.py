import pygame
import sys

# Initialize Pygame
pygame.init()
clock=pygame.time.Clock()

# Set the screen to windowed mode
screen = pygame.display.set_mode((2560, 1440), flags = pygame.OPENGL | pygame.FULLSCREEN, display=1)

# Start the main loop
while True:
    clock.tick(60)
    # Check for events
    for event in pygame.event.get():
        # Check for the quit event
        if event.type == pygame.QUIT:
            # Quit the game
            pygame.quit()
            sys.exit()

        # Check for the fullscreen toggle event
        if event.type == pygame.KEYDOWN and event.key == pygame.K_F11:
            # Toggle fullscreen mode
            pygame.display.toggle_fullscreen()
            
            
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_ESCAPE:
                pygame.quit()
                sys.exit()