using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;


namespace AHGame
{
    public abstract class Interactable :Entity
    {
        public Body body;
        public Fixture fixture;
        public Vector2 pos,origPos,origin;
        public float height, width;
        public float drawLevel;
        public string sName;
        public float centerOffset;
        public float rotationAngle = 0;

        public Interactable(Game1 g, Vector2 pos, String sName,float height,float width, float drawLevel,float rotation)
            : base(g)
        {
            this.pos = pos;
            this.origPos = pos;
            this.width = width;
            this.height = height;
            this.drawLevel = drawLevel;
            this.sName = sName;
            this.rotationAngle = rotation;
            //SetUpPhysics(pos);
            //LoadContent();
            //fixture.OnCollision += new OnCollisionEventHandler(OnCollision);
        }
        public abstract bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact);
        protected abstract void SetUpPhysics(Vector2 position);

    }
}
