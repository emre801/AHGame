using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace AHGame
{
    public class Title : Entity
    {
        SpriteStripAnimationHandler titleLogo,bgSquare;
        SpriteStripAnimationHandler start, options, exit;
        float alpha = 0.6f,titleAlpha=1f;
        SpriteStripAnimationHandler cursorPos;
        float cursorPosY;

        Input input;
       // World backGroundWorld;
        bool inTransition = false;
        Ticker transitionTick;

        public Title(Game1 g)
            : base(g)
        {
            
        }
        public override void LoadContent()
        {
            titleLogo = g.getSpriteAni("AHLogo");
            start = g.getSpriteAni("start");
            options = g.getSpriteAni("options");
            exit = g.getSpriteAni("exit");
            //cursor = g.getSpriteAni("cursor");
            bgSquare = g.getSpriteAni("bgSquare");

            cursorPos = start;
            cursorPosY = 0.35f;
            input = g.playerOneControls;
            transitionTick = new Ticker(60);
            
        }

        public void updateCursorPosition()
        {
            if (g.currState == Game1.gameStates.TITLE)
            {
                if (inTransition)
                {
                    if (transitionTick.hasTicked())
                    {
                        alpha -= 0.03f;
                        titleAlpha -= 0.05f;
                    }
                    if (titleAlpha <= 0)
                    {
                        g.currState = Game1.gameStates.PLAYERSELECT;
                    }
                    return;
                }

                if (input.isDownPressed())
                {
                    if (cursorPos == start)
                        cursorPos = options;
                    else if (cursorPos == options)
                        cursorPos = exit;
                    else
                        cursorPos = start;
                    g.sfxControl.playSound("RollOver");
                }
                else if (input.isUpPressed())
                {
                    if (cursorPos == exit)
                        cursorPos = options;
                    else if (cursorPos == start)
                        cursorPos = exit;
                    else
                        cursorPos = start;
                    g.sfxControl.playSound("RollOver");
                }
                else if (input.isJumpPressed())
                {
                    if (cursorPos == start)
                    {
                        inTransition = true;
                        g.sfxControl.playSound("Select");
                    }
                    else if (cursorPos == options)
                    {
                        //TODO implement an options
                    }
                    else
                        g.gameExit();

                }
            }

        }

        public override void Update()
        {
            updateCursorPosition();
        }

        public void drawTitleInfo(SpriteBatch spriteBatch, SpriteStripAnimationHandler titleInfo, SpriteStripAnimationHandler moveBy, float alpha, float yPos, float xPos)
        {
            
            if (cursorPos == titleInfo)
                alpha = titleAlpha;
            titleInfo.draw(spriteBatch, new Rectangle((int)(Constants.DESIRED_GAME_RESOLUTION_WIDTH / 2) - (int)(moveBy.widthOf() * xPos),
               (int)(Constants.DESIRED_GAME_RESOLUTION_HEIGHT * yPos), (int)(titleInfo.widthOf() * 0.5f), (int)(titleInfo.heightOf() * 0.5f)), Color.Black, new Vector2(0, 0), true);

            titleInfo.draw(spriteBatch, new Rectangle((int)(Constants.DESIRED_GAME_RESOLUTION_WIDTH / 2) - (int)(moveBy.widthOf() * xPos),
                (int)(Constants.DESIRED_GAME_RESOLUTION_HEIGHT * yPos), (int)(titleInfo.widthOf() * 0.5f), (int)(titleInfo.heightOf() * 0.5f)), Color.White * alpha, new Vector2(0, 0), true);
            /*
            spriteBatch.Draw(titleInfo.index, new Rectangle((int)(Constants.GAME_WORLD_WIDTH / 2) - (int)(moveBy.index.Width * xPos),
                (int)(Constants.GAME_WORLD_WIDTH * yPos),
                (int)(titleInfo.index.Width * 0.5f),
                (int)(titleInfo.index.Height * 0.5f)), Color.Black);
            spriteBatch.Draw(titleInfo.index, new Rectangle((int)(Constants.GAME_WORLD_WIDTH / 2) - (int)(moveBy.index.Width * xPos),
                (int)(Constants.GAME_WORLD_WIDTH * yPos),
                (int)(titleInfo.index.Width * 0.5f),
                (int)(titleInfo.index.Height * 0.5f)), Color.White * alpha);*/

        }

        public void drawBackgroundPatter(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Constants.GAME_WORLD_WIDTH; i += bgSquare.widthOf() / 2)
                for (int j = 0; j < Constants.GAME_WORLD_HEIGHT; j += bgSquare.heightOf() / 2)
                    bgSquare.draw(spriteBatch, new Rectangle(i, j, (int)bgSquare.widthOf(), (int)bgSquare.heightOf()), Color.White * 0.35f * titleAlpha, new Vector2(0, 0), false);
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            drawBackgroundPatter(spriteBatch);

            drawTitleInfo(spriteBatch, titleLogo,titleLogo, titleAlpha,0.125f,0.25f);
            drawTitleInfo(spriteBatch, start, start, alpha, 0.55f, 0.25f);
            drawTitleInfo(spriteBatch, options, options, alpha, 0.65f, 0.25f);
            drawTitleInfo(spriteBatch, exit, exit, alpha, 0.75f, 0.25f);
        }
    }
}
