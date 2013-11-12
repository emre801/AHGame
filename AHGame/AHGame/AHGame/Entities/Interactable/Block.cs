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
    public class Block: Interactable
    {
        bool isDeathBlock, interactWithPlayer;
        Sprite currSprite;

        float heightDiff = 0, widthDiff = 0;
        SpriteStripAnimationHandler aniSprite;
        public Block(Game1 g, Vector2 pos, String sName, float height, float width, float drawLevel,bool isDeathBlock, float rotation, bool interactWithPlayer)
            :base(g,pos,sName,height,width,drawLevel,rotation)
        {
            this.isDeathBlock = isDeathBlock;
            this.interactWithPlayer = interactWithPlayer;
            SetUpPhysics(pos);
            LoadContent();
            heightDiff = currSprite.index.Height - height;
            widthDiff = currSprite.index.Width - width;
            origin = new Vector2(currSprite.index.Width/2f, currSprite.index.Height/2f);
            fixture.OnCollision += new OnCollisionEventHandler(OnCollision);
        }

        public override void LoadContent()
        {
            currSprite = g.getSprite(sName);                
        }
        public override bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (isDeathBlock)
            {
                foreach (Player p in g.players)
                {
                    if (fixtureB == p.fixture)
                    {
                        g.restartLevel = true;

                    }


                }
            }
            return true;
        }
        protected override void SetUpPhysics(Vector2 position)
        {
            float mass = 1000;
            float width = this.width;
            float height = this.height;
            World world = g.world;
            if (!this.interactWithPlayer)
                  world = g.world2;
            fixture = FixtureFactory.CreateRectangle(world, (float)ConvertUnits.ToSimUnits(width), (float)ConvertUnits.ToSimUnits(height), mass);
            body = fixture.Body;
            fixture.Body.BodyType = BodyType.Static;
            fixture.Restitution = 0.3f;
            fixture.Friction = 0.1f;
            body.Position = ConvertUnits.ToSimUnits(position);
            centerOffset = position.Y - (float)ConvertUnits.ToDisplayUnits(body.Position.Y); //remember the offset from the center for drawing
            body.IgnoreGravity = true;
            body.FixedRotation = true;
            body.LinearDamping = 0.5f;
            body.AngularDamping = 1f;
            body.Rotation = rotationAngle * (float)Math.PI / 180f; 
        }
        public override void Update()
        {
            if (this.sName.Equals("cloudPix"))
                body.Position -= new Vector2(0.005f, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            Draw(spriteBatch, 1f);
        }

        public void Draw(SpriteBatch spriteBatch, float alpha)
        {
            spriteBatch.Draw(currSprite.index, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X),
                (int)ConvertUnits.ToDisplayUnits(body.Position.Y), (int)width, (int)height), null, Color.White*alpha, 
                body.Rotation, origin, SpriteEffects.None, (drawLevel+1)/100f);
            spriteBatch.Draw(currSprite.index, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)+5,
                (int)ConvertUnits.ToDisplayUnits(body.Position.Y)-5, (int)width, (int)height), null, Color.Black*0.3f,
                body.Rotation, origin, SpriteEffects.None, 0);
        }
    }
}
