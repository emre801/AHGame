using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AHGame
{
    public class CreatorInput
    {
        public KeyboardState keyboardState;
        public KeyboardState previousKeyboardState;
        MouseState mouseState;
        MouseState previosMouseState;
        Keys up { get; set; }
        Keys down { get; set; }
        Keys left { get; set; }
        Keys right { get; set; }
        Keys changeBlockType { get; set; }
        Keys rotate { get; set; }
        Keys changeLayerUp{ get; set; }
        Keys changeLayerDown { get; set; }

        public CreatorInput()
        {
            up = Keys.Up;
            down = Keys.Down;
            left = Keys.Left;
            right = Keys.Right;
            changeBlockType = Keys.Q;
            rotate = Keys.R;
            changeLayerUp = Keys.A;
            changeLayerDown = Keys.S;
        }
        //TODO: add changing of layer stuff, taking break

        public bool isUpPressed()
        {
            return previousKeyboardState.IsKeyUp(up) && keyboardState.IsKeyDown(up);
        }
        public bool isDownPressed()
        {
            return previousKeyboardState.IsKeyUp(down) && keyboardState.IsKeyDown(down);
        }
        public bool isLeftPressed()
        {
            return previousKeyboardState.IsKeyUp(left) && keyboardState.IsKeyDown(left);
        }
        public bool isRightPressed()
        {
            return previousKeyboardState.IsKeyUp(right) && keyboardState.IsKeyDown(right);
        }
        public bool isRotatePressed()
        {
            return previousKeyboardState.IsKeyUp(rotate) && keyboardState.IsKeyDown(rotate);
        }
        public bool isChangeLayerUpPressed()
        {
            return previousKeyboardState.IsKeyUp(changeLayerUp) && keyboardState.IsKeyDown(changeLayerUp);
        }
        public bool isChangeLayerDownPressed()
        {
            return previousKeyboardState.IsKeyUp(changeLayerDown) && keyboardState.IsKeyDown(changeLayerDown);
        }
        public bool isBlockTypePressed()
        {
            return previousKeyboardState.IsKeyUp(changeBlockType) && keyboardState.IsKeyDown(changeBlockType);
        }

        public bool isMousePressed()
        {
            return previosMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool isMouseReleased()
        {
            return previosMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released;
        }

        public void Update()
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            previosMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

    }
}

