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
        Keys iterateBlockUp { get; set; }
        Keys iterateBlockDown { get; set; }
        Keys writeLevel { get; set; }
        Keys shift { get; set; }
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
            writeLevel = Keys.W;
            iterateBlockUp = Keys.Y;
            iterateBlockDown = Keys.T;
            shift=Keys.LeftShift;
        }
        //TODO: add changing of layer stuff, taking break

        public bool isUpPressed()
        {
            return  keyboardState.IsKeyDown(up);
        }
        public bool isDownPressed()
        {
            return keyboardState.IsKeyDown(down);
        }
        public bool isLeftPressed()
        {
            return keyboardState.IsKeyDown(left);
        }
        public bool isRightPressed()
        {
            return keyboardState.IsKeyDown(right);
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

        public bool isLeftMousePressed()
        {
            return previosMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool isLeftMouseReleased()
        {
            return previosMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released;
        }

        public bool isRightMousePressed()
        {
            return previosMouseState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed;
        }

        public bool isRightMouseReleased()
        {
            return previosMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released;
        }

        public bool isWritePressed()
        {
            return previousKeyboardState.IsKeyUp(writeLevel) && keyboardState.IsKeyDown(writeLevel);
        }

        public bool isIterateBlockUpPressed()
        {
            return previousKeyboardState.IsKeyUp(iterateBlockUp) && keyboardState.IsKeyDown(iterateBlockUp);
        }
        public bool isIterateBlockDownPressed()
        {
            return previousKeyboardState.IsKeyUp(iterateBlockDown) && keyboardState.IsKeyDown(iterateBlockDown);
        }
        public bool isShiftPressed()
        {
            return previousKeyboardState.IsKeyUp(shift) && keyboardState.IsKeyDown(shift);
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

