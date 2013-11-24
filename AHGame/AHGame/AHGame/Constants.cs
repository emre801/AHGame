using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AHGame
{
    public class Constants
    {
        public const float GAME_WORLD_WIDTH = 480f*2f;  
        public const float GAME_WORLD_HEIGHT = 320f*2f;

        public const bool ENABLE_CREATOR = true;

        public static readonly Vector2 player1SpawnLocation = new Vector2(Constants.GAME_WORLD_WIDTH * .01f, Constants.GAME_WORLD_HEIGHT * 0.85f);
        public const double GAMEWORLD_ASPECT_RATIO = 5f / 4f;
        public const int DESIRED_GAME_RESOLUTION_WIDTH = 900;//480
        public const int DESIRED_GAME_RESOLUTION_HEIGHT = (int)(DESIRED_GAME_RESOLUTION_WIDTH / GAMEWORLD_ASPECT_RATIO); //320
    }
}