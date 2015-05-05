using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Cyber.GraphicsEngine
{
    class CameraBehavior
    {
        GamePadState currentGamePadState = new GamePadState();

        float cameraArc = 0;
        float cameraRotation = 0;
        float cameraDistance = 500;

        //Camera Input Handler
        public void UpdateCamera(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Obracanie kamery góra/dół wokół modelu
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                currentKeyboardState.IsKeyDown(Keys.W))
            {
                cameraArc += time * 0.1f;
                Debug.WriteLine("UP KEY PRESSED, or W perhaps");
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                currentKeyboardState.IsKeyDown(Keys.S))
            {
                cameraArc -= time * 0.1f;
                Debug.WriteLine("DOWN KEY PRESSED, or W perhaps");
            }

            cameraArc += currentGamePadState.ThumbSticks.Right.Y * time * 0.25f;

            // Ograniczenie na kąt obrotu.
            if (cameraArc > 90.0f)
                cameraArc = 90.0f;
            else if (cameraArc < -90.0f)
                cameraArc = -90.0f;

            // Obracanie kamery wokół obrotu.
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                currentKeyboardState.IsKeyDown(Keys.D))
            {
                cameraRotation += time * 0.1f;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                currentKeyboardState.IsKeyDown(Keys.A))
            {
                cameraRotation -= time * 0.1f;
            }

            cameraRotation += currentGamePadState.ThumbSticks.Right.X * time * 0.25f;

            // Zoom In & Out.
            if (currentKeyboardState.IsKeyDown(Keys.Z))
                cameraDistance += time * 0.25f;

            if (currentKeyboardState.IsKeyDown(Keys.X))
                cameraDistance -= time * 0.25f;

            cameraDistance += currentGamePadState.Triggers.Left * time * 0.5f;
            cameraDistance -= currentGamePadState.Triggers.Right * time * 0.5f;

            // Ograniczenie dystansu kamery.
            if (cameraDistance > 700.0f)
                cameraDistance = 700.0f;
            else if (cameraDistance < 10.0f)
                cameraDistance = 10.0f;

            if (currentGamePadState.Buttons.RightStick == ButtonState.Pressed ||
                currentKeyboardState.IsKeyDown(Keys.R))
            {
                cameraArc = 0;
                cameraRotation = 0;
                cameraDistance = 100;
            }
        }

    }
}
