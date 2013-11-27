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
        SpriteStripAnimationHandler titleLogo,bgSquare,animatedLogo;
        SpriteStripAnimationHandler start, options,creator, exit;
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
            creator = g.getSpriteAni("creator");
            exit = g.getSpriteAni("exit");
            //cursor = g.getSpriteAni("cursor");
            bgSquare = g.getSpriteAni("bgSquare");
            animatedLogo = g.getSpriteAni("LogoAnimated");

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
                        if (cursorPos == start)
                            g.currState = Game1.gameStates.PLAYERSELECT;
                        else
                        {
                            g.currState = Game1.gameStates.CREATOR;
                            g.cb=new CreatorBlock(g);
                            g.addEntity(g.cb);
                        }
                    }
                    return;
                }

                if (input.isDownPressed())
                {
                    if (!Constants.ENABLE_CREATOR)
                    {
                        if (cursorPos == start)
                            cursorPos = options;
                        else if (cursorPos == options)
                            cursorPos = exit;
                        else
                            cursorPos = start;
                    }
                    else
                    {
                        if (cursorPos == exit)
                            cursorPos = start;
                        else if (cursorPos == start)
                            cursorPos = options;
                        else if (cursorPos == creator)
                            cursorPos = exit;
                        else
                            cursorPos = creator;
                    }
                    g.sfxControl.playSound("RollOver");
                }
                else if (input.isUpPressed())
                {
                    if (Constants.ENABLE_CREATOR)
                    {
                        if (cursorPos == exit)
                            cursorPos = creator;
                        else if (cursorPos == start)
                            cursorPos = exit;
                        else if (cursorPos == creator)
                            cursorPos = options;
                        else
                            cursorPos = start;

                    }
                    else
                    {
                        if (cursorPos == exit)
                            cursorPos = options;
                        else if (cursorPos == start)
                            cursorPos = exit;
                        else
                            cursorPos = start;

                    }
                    g.sfxControl.playSound("RollOver");
                }
                else if (input.isJumpPressed())
                {
                    if (cursorPos == start || cursorPos==creator)
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
            animatedLogo.Update();
        }

        public void drawTitleInfo(SpriteBatch spriteBatch, SpriteStripAnimationHandler titleInfo, SpriteStripAnimationHandler moveBy, float alpha, float yPos, float xPos)
        {
            
            if (cursorPos == titleInfo)
                alpha = titleAlpha;
            titleInfo.drawTitle(spriteBatch, new Rectangle((int)(Constants.DESIRED_GAME_RESOLUTION_WIDTH / 2) - (int)(moveBy.widthOf() * xPos),
               (int)(Constants.DESIRED_GAME_RESOLUTION_HEIGHT * yPos), (int)(titleInfo.widthOf() * 0.5f), (int)(titleInfo.heightOf() * 0.5f)), Color.Black, new Vector2(0, 0), true);

            titleInfo.drawTitle(spriteBatch, new Rectangle((int)(Constants.DESIRED_GAME_RESOLUTION_WIDTH / 2) - (int)(moveBy.widthOf() * xPos),
                (int)(Constants.DESIRED_GAME_RESOLUTION_HEIGHT * yPos), (int)(titleInfo.widthOf() * 0.5f), (int)(titleInfo.heightOf() * 0.5f)), Color.White * alpha, new Vector2(0, 0), true);

        }

        public void drawBackgroundPatter(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Constants.DESIRED_GAME_RESOLUTION_WIDTH; i += bgSquare.widthOf() / 2)
                for (int j = 0; j < Constants.DESIRED_GAME_RESOLUTION_HEIGHT; j += bgSquare.heightOf() / 2)
                    bgSquare.draw(spriteBatch, new Rectangle(i, j, (int)bgSquare.widthOf(), (int)bgSquare.heightOf()), Color.White * 0.35f * titleAlpha, new Vector2(0, 0), false);
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            drawBackgroundPatter(spriteBatch);

            drawTitleInfo(spriteBatch, titleLogo, titleLogo, titleAlpha, 0.20f, 0.20f);
            drawTitleInfo(spriteBatch, animatedLogo, animatedLogo, titleAlpha, 0.20f, 1.25f);
            drawTitleInfo(spriteBatch, start, start, alpha, 0.55f, 0.25f);
            drawTitleInfo(spriteBatch, options, options, alpha, 0.65f, 0.25f);
            if (Constants.ENABLE_CREATOR)
            {
                drawTitleInfo(spriteBatch, creator, creator, alpha, 0.75f, 0.25f);
                drawTitleInfo(spriteBatch, exit, exit, alpha, 0.85f, 0.25f);
            }
            else
            {
                drawTitleInfo(spriteBatch, exit, exit, alpha, 0.75f, 0.25f);
            }
        }
    }
}
