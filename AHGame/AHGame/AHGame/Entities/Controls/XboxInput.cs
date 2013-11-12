using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace AHGame
{
    class XboxInput:Input
    {
        public GamePadState gameState;
        public GamePadState previousGameState;
        PlayerIndex playerIndex;

        /*Buttons up { get; set; }
        Buttons down { get; set; }
        Buttons left { get; set; }
        Buttons right { get; set; }*/
        Buttons turbo { get; set; }
        Buttons jump { get; set; }
        Buttons back { get; set; }
        Buttons pause { get; set; }
        Buttons start { get; set; }

        
        public XboxInput(Game1 g,PlayerIndex playerIndex)
            :base(g)
        {
            this.playerIndex = playerIndex; 
            previousGameState = gameState;
            gameState = GamePad.GetState(playerIndex);
            turbo = Buttons.RightShoulder;
            jump = Buttons.A;
            back = Buttons.B;
            pause = Buttons.Back;
            start = Buttons.Start;

        }

        public override void Update()
        {
            previousGameState = gameState;
            gameState = GamePad.GetState(playerIndex);
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override bool isJumpPressed()
        {
            return previousGameState.IsButtonUp(jump) && gameState.IsButtonDown(jump);
        }
        public override bool isJumpReleased()
        {
            return previousGameState.IsButtonDown(jump) && gameState.IsButtonUp(jump);
        }
        public override bool isBackPressed()
        {
            return previousGameState.IsButtonUp(back) && gameState.IsButtonDown(back);
        }
        public override bool isPausePressed()
        {
            return previousGameState.IsButtonUp(pause) && gameState.IsButtonDown(pause);
        }
        public override bool isStartPressed()
        {
            return previousGameState.IsButtonUp(start) && gameState.IsButtonDown(start);
        }
        public override bool isTurboHeld()
        {
            return gameState.IsButtonDown(turbo);
        }

        public override bool isUpPressed()
        {
            return this.gameState.ThumbSticks.Left.Y > 0 && this.previousGameState.ThumbSticks.Left.Y <= 0;
        }


        public override bool isDownPressed()
        {
            return this.gameState.ThumbSticks.Left.Y < 0 && this.previousGameState.ThumbSticks.Left.Y >= 0;
        }

        //This is going off of the left thumbstick, this will allow for more accurate movement
        public override float moveHorizontal()
        {
            return gameState.ThumbSticks.Left.X;
        }
        public override float moveVertical()
        {
            return gameState.ThumbSticks.Left.Y;
        }
    }
}
