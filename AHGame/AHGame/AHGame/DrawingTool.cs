﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AHGame
{
    public class DrawingTool
    {
        readonly GraphicsDeviceManager gdm;
        SpriteBatch spriteBatch;
        Game1 game;
        public Camera2d cam;

        public static DrawingTool instance { get; private set; }
        public int ActualScreenPixelWidth { get; private set; }
        public int ActualScreenPixelHeight { get; private set; }
        public float blackAlpha = 1f,blackAlphaOut=0;
        Ticker blackTicker;
        public Color bgColor = Color.Black;

        public DrawingTool(Game1 game)
        {
            this.game = game;
            instance = this;
            gdm = new GraphicsDeviceManager(game);
            gdm.PreferMultiSampling = false;
            gdm.ApplyChanges();

            gdm.PreferredBackBufferWidth = Constants.DESIRED_GAME_RESOLUTION_WIDTH;
            gdm.PreferredBackBufferHeight = Constants.DESIRED_GAME_RESOLUTION_HEIGHT;

            cam = new Camera2d(Constants.DESIRED_GAME_RESOLUTION_WIDTH, Constants.DESIRED_GAME_RESOLUTION_HEIGHT);
            cam.Zoom = 0.45f;
            cam.Pos = new Vector2(-100, -100);
            blackTicker = new Ticker(60);
        }
        public void initialize()
        {
            spriteBatch = new SpriteBatch(gdm.GraphicsDevice);
        }
        private void beginBatch()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        }
        private void beginBatchWithCam()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, null, cam.get_transformation(gdm.GraphicsDevice /*Send the variable that has your graphic device here*/));
        }

        private void endBatch()
        {
            spriteBatch.End();
        }

        public void drawTitle(Title title)
        {
            beginBatch();
            title.Draw(spriteBatch);
            endBatch();
        }
        public void drawPlayerSelect(PlayerSelect playerSelect)
        {

        }
        public void draw()
        {
            
            switch (game.currState)
            {
                case Game1.gameStates.TITLE:
                    gdm.GraphicsDevice.Clear(bgColor);
                    drawTitle(game.title);
                    break;
                case Game1.gameStates.GAME:
                    gdm.GraphicsDevice.Clear(Color.SkyBlue);
                    drawEntities(game.entities);
                    break;
                case Game1.gameStates.PLAYERSELECT:
                    gdm.GraphicsDevice.Clear(Color.Purple);
                    drawPlayerSelect(game.playerSelect);
                    break;
            }
            
        }

        internal void drawEntities(List<Entity> entities)
        {
            
            beginBatchWithCam();
            //beginBatch();
                    
            foreach (Entity e in entities)
                e.Draw(spriteBatch);
            endBatch();
            beginBatch();
            spriteBatch.Draw(game.black.index, new Rectangle(0, 0, (int)Constants.DESIRED_GAME_RESOLUTION_WIDTH, (int)Constants.DESIRED_GAME_RESOLUTION_HEIGHT), Color.White*blackAlpha);
            spriteBatch.Draw(game.black.index, new Rectangle(0, 0, (int)Constants.DESIRED_GAME_RESOLUTION_WIDTH, (int)Constants.DESIRED_GAME_RESOLUTION_HEIGHT), Color.White * blackAlphaOut);
            
            endBatch();
            bool hasTicked = blackTicker.hasTicked();
            if (hasTicked && blackAlpha > 0)
            {
                blackAlpha -= 0.2f;
            }
            if (hasTicked && blackAlphaOut <= 1 && game.blackOut)
            {
                blackAlphaOut += 0.2f;

            }

        }

        public void followPlayer()
        {
            Vector2 followPoint = game.getMidPointOfPlayers();
            float widthRatio = 0.85f;
            float heightRatio = 0.85f;
            float heightRatio2 = 0.70f;//
            float width = widthRatio * cam.ViewportWidth;// *zoomRatio;
            float height = heightRatio * cam.ViewportHeight;// *zoomRatio;
            float height2 = heightRatio2 * cam.ViewportHeight;

            if (cam._pos.X + cam.ViewportWidth < followPoint.X+width)
            {
                cam.Move(new Vector2((followPoint.X+width) - (cam._pos.X + cam.ViewportWidth / 1), 0));
            }
            if (cam._pos.X - cam.ViewportWidth > followPoint.X-width)
            {
                cam.Move(new Vector2((followPoint.X-width) - (cam._pos.X - cam.ViewportWidth / 1), 0));
            }
            if (followPoint.Y + height2 > cam._pos.Y + cam.ViewportHeight / 1)
            {
                cam.Move(new Vector2(0, (followPoint.Y + height2) - (cam._pos.Y + cam.ViewportHeight / 1)));
            }
            if (followPoint.Y - height < cam._pos.Y - cam.ViewportHeight / 1)
            {
                cam.Move(new Vector2(0, (followPoint.Y - height) - (cam._pos.Y - cam.ViewportHeight / 1)));     
            }
        }

        public void Update()
        {
            followPlayer();

        }
        public void moveCameraManually(Input input)
        {
            /*
            float x = input.moveHorizontal()*-10f;
            float y = input.moveVertical()*-10f;
            cam.Move(new Vector2(x,y));
            if (input.isJumpPressed())
                cam.Zoom -= 0.1f;*/
        }
    }
}