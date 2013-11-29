using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace AHGame
{
    public class SpriteStripAnimationHandler
    {
        public readonly Sprite spriteStrip;
        private readonly Texture2D index;
        private readonly int stateCount, widthOfSingleState;
        private int _currentState;
        private Rectangle texBounds;
        private SoundEffect sound;
        int soundRate = 0;
        int rateOfSound = 2;
        public int CurrentState
        {
            get { return _currentState; }
            set
            {
                if (value < 0 || value >= stateCount) throw new IndexOutOfRangeException();
                setState(value);
            }
        }

        private float frameRate;
        private float origFrameRate;
        public Stopwatch stopWatch = new Stopwatch();
        public int numberOfTotalCycles = 0;
        public Ticker tick;

        public SpriteStripAnimationHandler(Sprite strip, int stateCount, float frameRate)
        {
            this.spriteStrip = strip;
            if (strip.index.Width % stateCount != 0)
            {
                throw new Exception("SpriteStrip from " + strip.fileName + " was not divisible by the requested number of states: " + stateCount);
            }
            this.stateCount = stateCount;
            this.widthOfSingleState = strip.index.Width / stateCount;
            texBounds = new Rectangle(0, 0, widthOfSingleState, strip.index.Height);
            stopWatch.Start();
            this.frameRate = frameRate;
            this.origFrameRate = frameRate;
            tick = new Ticker((1f/frameRate)*1000);
        }
        public Texture2D getIndex()
        {
            return spriteStrip.index;
        }

        public int getStateCount()
        {
            return this.stateCount;
        }

        public void nextState()
        {
            setState((CurrentState + 1) % stateCount);
        }
        public int widthOf()
        {
            return this.widthOfSingleState;
        }
        public int heightOf()
        {
            return spriteStrip.index.Height;
        }

        public void previousState()
        {
            setState((CurrentState - 1) % stateCount);
        }
        public int getCycles()
        {
            return _currentState;
        }

        public void setStatePub(int state)
        {
            setState(state);
        }
        private void setState(int state)
        {
            texBounds.X = state * widthOfSingleState;
            _currentState = state;
        }
        public void changeFrameRate(int frameRate)
        {
            this.frameRate = frameRate;
        }
        public float getFrameRate()
        {
            return frameRate;
        }
        public void resetFrameRate()
        {
            this.frameRate = this.origFrameRate;
        }
        public void Update()
        {
            if (tick.hasTicked())
            {
                nextState();
                soundRate++;
                if (sound != null && soundRate%rateOfSound==0)
                    sound.Play();
                
            }
        }
        public void updateFrameRate(float frameRate)
        {
            tick.setTickBeat((1f / frameRate) * 1000);

        }
        public void addSound(SoundEffect sound)
        {
            this.sound = sound;
        }
        public void changeSoundRate(int rateofSound)
        {
            this.rateOfSound = rateofSound;
        }
        public void drawCurrentState(SpriteBatch spriteBatch,Vector2 drawPos, Vector2 origin, Rectangle rect, Boolean direction, Vector2 shiftPosition, float rotation)
        {
            if (direction)
                spriteBatch.Draw(spriteStrip.index, drawPos, texBounds, Color.White, rotation, origin + shiftPosition, 1, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(spriteStrip.index, drawPos, texBounds, Color.White, rotation, origin - shiftPosition, 1, SpriteEffects.FlipHorizontally, 0);
        }

        public void draw(SpriteBatch spriteBatch, Rectangle desti, Color color,Vector2 origin, bool lookLeft, float drawLevel)
        {
            Rectangle shiftedDesti = new Rectangle(desti.X + 5, desti.Y - 5, desti.Width, desti.Height);
            if (lookLeft)
            {
                spriteBatch.Draw(spriteStrip.index, desti, texBounds, color, 0, origin, SpriteEffects.None, drawLevel+0.001f);

                spriteBatch.Draw(spriteStrip.index, shiftedDesti, texBounds, Color.Black*0.3f, 0, origin, SpriteEffects.None, drawLevel);
            }
            else
            {
                spriteBatch.Draw(spriteStrip.index, desti, texBounds, color, 0, origin, SpriteEffects.FlipHorizontally, drawLevel + 0.001f);
                spriteBatch.Draw(spriteStrip.index, shiftedDesti, texBounds, Color.Black * 0.3f, 0, origin, SpriteEffects.FlipHorizontally, drawLevel);
            }
        }

        public void drawTitle(SpriteBatch spriteBatch, Rectangle desti, Color color, Vector2 origin, bool lookLeft)
        {
            Rectangle shiftedDesti = new Rectangle(desti.X + 5, desti.Y - 5, desti.Width, desti.Height);
            if (lookLeft)
            {
                spriteBatch.Draw(spriteStrip.index, shiftedDesti, texBounds, Color.Black * 0.3f, 0, origin, SpriteEffects.None, 0.1f);
                spriteBatch.Draw(spriteStrip.index, desti, texBounds, color, 0, origin, SpriteEffects.None, 0.11f);
            }
            else
            {
                spriteBatch.Draw(spriteStrip.index, shiftedDesti, texBounds, Color.Black * 0.3f, 0, origin, SpriteEffects.FlipHorizontally, 0.1f);
                spriteBatch.Draw(spriteStrip.index, desti, texBounds, color, 0, origin, SpriteEffects.FlipHorizontally, 0.11f);
            }
        }



        public void drawCurrentState(SpriteBatch spriteBatch, Vector2 drawPos, Vector2 origin, Rectangle rect, Boolean direction, Vector2 shiftPosition)
        {
            drawCurrentState(spriteBatch, drawPos, origin, rect, direction, shiftPosition, MathHelper.ToRadians(0));
        }
        public void drawCurrentState(SpriteBatch spriteBatch, Rectangle rect, Vector2 origin)
        {
            spriteBatch.Draw(spriteStrip.index, rect, null, Color.White, 0, origin, SpriteEffects.None, 0f);
        }
        public void drawCurrentState(SpriteBatch spriteBatch, Vector2 drawPos, Vector2 origin, Boolean direction)
        {
            if (direction)
                spriteBatch.Draw(spriteStrip.index, drawPos, texBounds, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(spriteStrip.index, drawPos, texBounds, Color.White, 0, origin, 1, SpriteEffects.FlipHorizontally, 0);
        }
    }
}
