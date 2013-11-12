using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHGame
{
    public class PlayerSelect: Entity
    {
        public PlayerSelect(Game1 game)
            :base(game)
        {
            //TODO:implement this method......
        }

        public override void Update()
        {
            if (g.currState == Game1.gameStates.PLAYERSELECT)
            {
                g.currState = Game1.gameStates.GAME;
                g.levelControl.readLevel();
                g.musicControl.playRandomMusic();
            }
            //
        }
    }
}
