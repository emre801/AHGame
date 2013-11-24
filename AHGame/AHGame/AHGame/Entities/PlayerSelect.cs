using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;


namespace AHGame
{
    public class PlayerSelect: Entity
    {
        public int p1=0, p2=1, p3=2, p4=4;
        Sprite p1Av, p2Av, p3Av, p4Av;
        bool p1Active, p2Active, p3Active, p4Active;
        List<Player> players;
        String[] names;
        public PlayerSelect(Game1 game)
            :base(game)
        {
            //TODO:implement this method......
            names = new String[6];
            names[0] = "Michael";
            names[1] = "Gavin";
            names[2] = "Ryan";
            names[3] = "Jack";
            names[4] = "Geoff";
            names[5] = "Ray";
        }

        public void setPlayers()
        {
            this.players = new List<Player>();
            if (p1Active)
                players.Add(new Player(g, new Vector2(0, 0), g.playerOneControls,names[p1]));
            if (p2Active)
                players.Add(new Player(g, new Vector2(10, 0), g.playerTwoControls, names[p2]));
            //if (p3Active)
               // players.Add(new Player(g, new Vector2(20, 0), g.playerThreeControls, names[p3]));
            /*if (p4Active)
                players.Add(new Player(g, new Vector2(0, 0), g.playerOneControls, names[p4]));//*/
            //return players;
        }

        public List<Player> getPlayers()
        {
            return players;

        }

        public bool isStartPressed()
        {
            return (g.playerOneControls.isStartPressed() || g.playerTwoControls.isStartPressed()
                || g.playerThreeControls.isStartPressed() || g.playerFourControls.isStartPressed()) && isOneActive();
        }

        public void updateCharacterSelect()
        {


        }
        public bool isOneActive()
        {
            return p1Active || p2Active || p3Active || p4Active;
        }

        public override void Update()
        {
            if (g.currState == Game1.gameStates.PLAYERSELECT)
            {
               //p1 = new Player(g, new Vector2(0, 0), g.playerOneControls, "Michael");
                //p1 = 0;
                playerEnterLeaves();
                selectUpDown();
                if (isStartPressed())
                {
                    g.currState = Game1.gameStates.GAME;
                    g.levelControl.readLevel();
                    g.musicControl.playRandomMusic();
                }
                /*g.currState = Game1.gameStates.GAME;
                g.levelControl.readLevel();
                g.musicControl.playRandomMusic();*/
            }
            //
        }

        public void selectUpDown()
        {
            if (p1Active)
                this.p1 = playerUpDown(this.p1, g.playerOneControls);
            if (p2Active)
                this.p2 = playerUpDown(this.p2, g.playerTwoControls);
            if (p3Active)
                this.p3 = playerUpDown(this.p3, g.playerThreeControls);

        }
        public int playerUpDown(int p,Input input)
        {
            if (input.isUpPressed())
                p++;
            else if (input.isDownPressed())
                p--;

            if (p < 0)
                p = 5;
            else if (p > 5)
                p = 0;

            return p;

        }

        public void playerEnterLeaves()
        {
            //I don't really like how this looks, will work on better way later
            if (EnterLeaves(g.playerOneControls) == 1)
            {
                p1Active = true;
            }
            else if (EnterLeaves(g.playerOneControls) == -1)
            {
                p1Active = false;
            }
            ///////////////////////////////
            if (EnterLeaves(g.playerFourControls) == 1)
            {
                p4Active = true;
            }
            else if (EnterLeaves(g.playerFourControls) == -1)
            {
                p4Active = false;
            }
            ///////////////////////////////
            if (EnterLeaves(g.playerTwoControls) == 1)
            {
                p2Active = true;
            }
            else if (EnterLeaves(g.playerTwoControls) == -1)
            {
                p2Active = false;
            }
            ///////////////////////////////
            
            if (EnterLeaves(g.playerThreeControls) == 1)
            {
                p3Active = true;
            }
            else if (EnterLeaves(g.playerThreeControls) == -1)
            {
                p3Active = false;
            }


        }
        public int EnterLeaves(Input input)
        {
            if (input.isJumpPressed())
                return 1;
            else if (input.isBackPressed())
                return -1;
            return 0;
        }

        public void updateAvatars()
        {
            if(p1Active)
                p1Av=g.getSprite(names[p1].ToLower()+"Select");
            if (p2Active)
                p2Av = g.getSprite(names[p2].ToLower() + "Select");
            if (p3Active)
                p3Av = g.getSprite(names[p3].ToLower() + "Select");
            
        }

        public void drawTitleInfo(SpriteBatch spriteBatch, Sprite titleInfo, Sprite moveBy, float alpha, float yPos, float xPos)
        {
            /*
            titleInfo.drawTitle(spriteBatch, new Rectangle((int)(Constants.DESIRED_GAME_RESOLUTION_WIDTH / 2) - (int)(moveBy.widthOf() * xPos),
               (int)(Constants.DESIRED_GAME_RESOLUTION_HEIGHT * yPos), (int)(titleInfo.widthOf() * 0.5f), (int)(titleInfo.heightOf() * 0.5f)), Color.Black, new Vector2(0, 0), true);

            titleInfo.drawTitle(spriteBatch, new Rectangle((int)(Constants.DESIRED_GAME_RESOLUTION_WIDTH / 2) - (int)(moveBy.widthOf() * xPos),
                (int)(Constants.DESIRED_GAME_RESOLUTION_HEIGHT * yPos), (int)(titleInfo.widthOf() * 0.5f), (int)(titleInfo.heightOf() * 0.5f)), Color.White * alpha, new Vector2(0, 0), true);
            //*/
            spriteBatch.Draw(titleInfo.index, new Rectangle((int)(Constants.DESIRED_GAME_RESOLUTION_WIDTH*xPos)- (int)(moveBy.index.Width*0.5f),
                (int)(Constants.DESIRED_GAME_RESOLUTION_HEIGHT * yPos), (int)(titleInfo.index.Width * 0.5f), (int)(titleInfo.index.Height * 0.5f)), Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            updateAvatars();
            if (p1Active)
            {
                drawTitleInfo(spriteBatch, p1Av, p1Av, 1f, 0.5f, 0.225f);
                /*drawTitleInfo(spriteBatch, p1Av, p1Av, 1f, 0.5f, 0.475f);
                drawTitleInfo(spriteBatch, p1Av, p1Av, 1f, 0.5f, 0.725f);
                drawTitleInfo(spriteBatch, p1Av, p1Av, 1f, 0.5f, 0.975f);*/
            }
            if (p2Active)
            {
                drawTitleInfo(spriteBatch, p2Av, p2Av, 1f, 0.5f, 0.475f);
            }
            //if (p3Active)
            //{
            //    drawTitleInfo(spriteBatch, p3Av, p3Av, 1f, 0.5f, 0.725f);
            //}
                //spriteBatch.Draw(p1Av.index, new Vector2(0, 0), Color.White);
        }



    }
}
