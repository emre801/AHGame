using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AHGame
{
    class KeyBoardInput: Input
    {
        public KeyboardState keyboardState;
        public KeyboardState previousKeyboardState;

        //This would allow for key remapping if someone felt like doing so
        Keys up{get;set;}
        Keys down{get;set;}
        Keys left{get;set;}
        Keys right{get;set;}
        Keys turbo{get;set;}
        Keys jump{get;set;}
        Keys back { get; set; }
        Keys pause { get; set; }
        Keys start { get; set; }
        public KeyBoardInput(Game1 g)
            :base(g)
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            up = Keys.W;
            down = Keys.S;
            left = Keys.A;
            right = Keys.D;
            jump=Keys.Space;
            turbo=Keys.LeftShift;
            back = Keys.Tab;
            pause = Keys.Enter;
            start = Keys.Right;
        }

        public override void Update()
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool isJumpPressed()
        {
            return previousKeyboardState.IsKeyUp(jump) && keyboardState.IsKeyDown(jump);
        }
        public override bool isJumpReleased()
        {
            return previousKeyboardState.IsKeyDown(jump) && keyboardState.IsKeyUp(jump);
        }
        public override bool isBackPressed()
        {
            return previousKeyboardState.IsKeyUp(back) && keyboardState.IsKeyDown(back);
        }
        public override bool isPausePressed()
        {
            return previousKeyboardState.IsKeyUp(pause) && keyboardState.IsKeyDown(pause);
        }
        public override bool isStartPressed()
        {
            return previousKeyboardState.IsKeyUp(start) && keyboardState.IsKeyDown(start);
        }
        public override bool isTurboHeld()
        {
            return keyboardState.IsKeyDown(turbo);
        }

        public override bool isUpPressed()
        {
            return previousKeyboardState.IsKeyUp(up) && keyboardState.IsKeyDown(up);
        }

        public override bool isDownPressed()
        {
            return previousKeyboardState.IsKeyUp(down) && keyboardState.IsKeyDown(down);
        }

        //this will be binary input while the controller will be more precise
        public override float moveHorizontal()
        {
            if (keyboardState.IsKeyDown(left))
                return -1;
            if (keyboardState.IsKeyDown(right))
                return 1;
            return 0;
        }
        public override float moveVertical()
        {
            if (keyboardState.IsKeyDown(up))
                return 1;
            if (keyboardState.IsKeyDown(down))
                return -1;
            return 0;
        }
           




        
    }
}
