
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace AHGame
{
    public abstract class Input : Entity
    {
        public Input(Game1 g):base(g) { }

        public abstract float moveHorizontal();
        public abstract float moveVertical();

        public abstract bool isStartPressed();
        public abstract bool isBackPressed();
        public abstract bool isPausePressed();

        public abstract bool isJumpPressed();
        public abstract bool isJumpReleased();
        public abstract bool isTurboHeld();

        public abstract bool isUpPressed();
        public abstract bool isDownPressed();

        /*public abstract bool isNewButtonPressed(Buttons b);
        public abstract bool isButtonPressed(Buttons b);
        public abstract bool isNewButtonReleased(Buttons b);*/

    }
}
