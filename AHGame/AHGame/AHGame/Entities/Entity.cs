using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AHGame
{
    public abstract class Entity
    {
        public Game1 g;
        public bool dispose=false;
        public Entity(Game1 g)
        {
            this.g = g;
            dispose = false;
        }

        public virtual void LoadContent(){}

        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
