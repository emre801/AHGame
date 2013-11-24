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
    public class Player : Entity
    {
        Vector2 pos;
        public Body body;
        public Fixture fixture;
        public float centerOffset;
        bool onGround = false;
        public enum PlayerMode { ONGROUND, INAIR, ONWALLLEFT, ONWALLRIGHT}
        PlayerMode playerMode;
        Game1 g;
        Vector2 origin;
        Sprite currSprite;
        SpriteStripAnimationHandler currSpriteAni;
        Input input;
        bool lookLeft = true;
        int shiftValu = 0;
        int numJumps = 0;
        int maxNumJumps = 1;
        public String ahName;
        public Player(Game1 g, Vector2 pos, Input input,String ahName)
            :base(g)
        {
            this.g = g;
            this.pos = pos;
            this.ahName = ahName;
            LoadContent();
            SetUpPhysics(pos);
            this.input = input;
            fixture.OnCollision += new OnCollisionEventHandler(OnCollision);
            playerMode = PlayerMode.INAIR;
            onGround = false;

        }

        public void updateInput()
        {
            this.input.Update();
        }
        protected virtual void SetUpPhysics(Vector2 position)
        {
            World world = g.world;
            float mass = 0.8f;
            float width = currSprite.index.Width;
            float height = currSprite.index.Height;
            this.origin = new Vector2(currSprite.index.Width / 2, currSprite.index.Height / 2);
            fixture = FixtureFactory.CreateRectangle(world, (float)ConvertUnits.ToSimUnits(width), (float)ConvertUnits.ToSimUnits(height), mass);
            body = fixture.Body;
            fixture.Body.BodyType = BodyType.Dynamic;
            fixture.Restitution = 0.3f;
            fixture.Friction = 1f;
            body.Position = ConvertUnits.ToSimUnits(position);
            centerOffset = position.Y - (float)ConvertUnits.ToDisplayUnits(body.Position.Y); //remember the offset from the center for drawing
            body.IgnoreGravity = false;
            body.FixedRotation = true;
            body.AngularDamping = 1f;
            fixture.OnSeparation += new OnSeparationEventHandler(OnSeparation);


        }
        void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if ((playerMode == PlayerMode.ONWALLLEFT || playerMode == PlayerMode.ONWALLRIGHT))
            {
                playerMode = PlayerMode.INAIR;
            }
        }


        public bool isGoal(Fixture fix)
        {
            foreach (Entity e in g.entities)
            {
                if (e is GoalBlock)
                {
                    GoalBlock gb = (GoalBlock)e;
                    if (gb.fixture.Equals(fix))
                        return true;

                }

            }
            return false;
        }


        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            
            if (contact.IsTouching())
            {
                Manifold colis = new Manifold();
                contact.GetManifold(out colis);
                Vector2 pColis = colis.LocalNormal;

                if (isGoal(fixtureB))
                    return false;

                if (pColis.X == 0 && pColis.Y != 0)
                {
                    body.LinearVelocity = new Vector2(body.LinearVelocity.X, 0);
                    playerMode = PlayerMode.ONGROUND;
                    numJumps = maxNumJumps;
                    return true;
                }
                else if (pColis.X > 0 && pColis.Y == 0)//{X:1 Y:0}
                {
                    if (playerMode == PlayerMode.INAIR)
                    {
                        if (body.LinearVelocity.Y < 0)
                            body.LinearVelocity = new Vector2(0, body.LinearVelocity.Y + body.LinearVelocity.X * 0.15f);
                        else
                            body.LinearVelocity = new Vector2(0, body.LinearVelocity.Y - body.LinearVelocity.X * 0.15f);
                        playerMode = PlayerMode.ONWALLRIGHT;
                        numJumps = maxNumJumps;
                    }

                }
                else if (pColis.X < 0 && pColis.Y == 0)
                {
                    if (playerMode == PlayerMode.INAIR)
                    {
                        if (body.LinearVelocity.Y < 0)
                            body.LinearVelocity = new Vector2(0, body.LinearVelocity.Y - body.LinearVelocity.X * 0.15f);
                        else
                            body.LinearVelocity = new Vector2(0, body.LinearVelocity.Y + body.LinearVelocity.X * 0.15f);
                        playerMode = PlayerMode.ONWALLLEFT;
                        numJumps = maxNumJumps;
                    }

                }
                else
                {
                     int k = 0;

                }

            }

            return true;
        }
        public override void LoadContent()
        { 
            currSpriteAni = g.getSpriteAni(ahName+"Basic");
            currSprite = currSpriteAni.spriteStrip;
        }
        public override void Update()
        {
            float xDirect = input.moveHorizontal();
            float yDirect = input.moveVertical();
            float runningValue = 1;
            if (input.isTurboHeld())
                runningValue = 2.5f;
            if ((xDirect > 0 && body.LinearVelocity.X < 0) || (xDirect < 0 && body.LinearVelocity.X > 0))
            {
                body.LinearVelocity = new Vector2(body.LinearVelocity.X * 0.88f, body.LinearVelocity.Y);

            }
            if (playerMode==PlayerMode.ONGROUND)
            {
                if ((Math.Abs(body.LinearVelocity.X) < 8 * runningValue))
                {
                    body.ApplyLinearImpulse(new Vector2(xDirect * 0.45f * runningValue, 0));
                }
                
                if (xDirect == 0)
                    fixture.Friction = 80;
                else
                    fixture.Friction = 0;
            }
            else
            {
                if ((Math.Abs(body.LinearVelocity.X) < 10 * runningValue))
                {
                    body.ApplyLinearImpulse(new Vector2(xDirect * 0.55f * runningValue, 0));
                }
                if (playerMode == PlayerMode.INAIR)
                {
                    if (yDirect < 0)
                    {
                        body.ApplyLinearImpulse(new Vector2(0, -yDirect*0.3f));
                    }
                    else if (yDirect > 0)
                    {
                        body.ApplyLinearImpulse(new Vector2(0, -yDirect*0.001f));
                    }

                }


                if (input.moveHorizontal() == 0)
                    fixture.Friction = 10;
                else
                    fixture.Friction = 0;
            }

            

            /*
            if (body.LinearVelocity.X > 7*runningValue && playerMode==PlayerMode.ONGROUND)
            {

                //body.LinearVelocity = new Vector2(7*runningValue, body.LinearVelocity.Y);
                body.ApplyLinearImpulse(new Vector2(xDirect * 0.45f * runningValue, 0));// inputState.getYDirection() * 300f));
                
            }
            else if (body.LinearVelocity.X < -7 * runningValue && playerMode == PlayerMode.ONGROUND)
            {
                //body.LinearVelocity = new Vector2(-7*runningValue, body.LinearVelocity.Y);

            }*/


            if (playerMode == PlayerMode.ONGROUND && input.isJumpPressed())
            {
                body.IgnoreGravity = false;
                body.ApplyLinearImpulse(new Vector2(0, -30f + body.LinearVelocity.Y));
                playerMode = PlayerMode.INAIR;
                g.sfxControl.playSound("jump");
                numJumps--;
            }
            else if (playerMode!= PlayerMode.ONGROUND && input.isJumpReleased())
            {
                if(body.LinearVelocity.Y<0)
                    body.LinearVelocity = new Vector2(body.LinearVelocity.X, body.LinearVelocity.Y / 2f);
            }
            else if ((playerMode == PlayerMode.ONWALLLEFT || playerMode == PlayerMode.ONWALLRIGHT )&& input.isJumpPressed())
            {
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, 0);
                if(playerMode == PlayerMode.ONWALLLEFT)
                    body.ApplyLinearImpulse(new Vector2(-14f, -30f));
                else
                    body.ApplyLinearImpulse(new Vector2(14f, -30f));
                playerMode = PlayerMode.INAIR;
                g.sfxControl.playSound("jump");
                numJumps--;
            }

             else if (playerMode == PlayerMode.INAIR &&  input.isJumpPressed() && numJumps>0)
            {
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, 0);
                body.ApplyLinearImpulse(new Vector2(0, -30f + body.LinearVelocity.Y));
                g.sfxControl.playSound("jump");
                numJumps--;


            }
            if (Math.Abs(body.LinearVelocity.Y) > 0.01f && !(playerMode == PlayerMode.ONWALLLEFT || playerMode == PlayerMode.ONWALLRIGHT))
            {
                playerMode = PlayerMode.INAIR;

            }

            if (input.isBackPressed())
                g.restartLevel = true;

            updateCurrAni();

        }

        public void updateCurrAni()
        {
            shiftValu = 0;
            if (playerMode == PlayerMode.ONGROUND)
            {
                if (body.LinearVelocity.X != 0)
                {

                    currSpriteAni = g.getSpriteAni(ahName + "Walking");
                    currSpriteAni.addSound(g.sfxControl.getSound("running"));
                    float velocity = Math.Abs(body.LinearVelocity.X);
                    if (velocity > 10)
                    {
                        currSpriteAni = g.getSpriteAni(ahName + "Running");
                        currSpriteAni.addSound(g.sfxControl.getSound("running"));
                    }
                    currSpriteAni.updateFrameRate(12f * (velocity/10));
                }
                else
                {
                    currSpriteAni = g.getSpriteAni(ahName + "Stand");
                }
                if (body.LinearVelocity.X > 0)
                {
                    lookLeft = false;
                }
                else if(body.LinearVelocity.X < 0)
                {
                    lookLeft = true;
                }
            }
            else if (playerMode == PlayerMode.ONWALLLEFT || playerMode == PlayerMode.ONWALLRIGHT)
            {
                currSpriteAni = g.getSpriteAni(ahName + "Wall");
                if (playerMode == PlayerMode.ONWALLLEFT)
                {
                    
                    lookLeft = false;
                }
                else
                {
                    shiftValu = 10;
                    lookLeft = true;
                }

            }
            else if (playerMode == PlayerMode.INAIR)
            {
                if (body.LinearVelocity.X > 0)
                {
                    lookLeft = false;
                }
                else if (body.LinearVelocity.X < 0)
                {
                    lookLeft = true;
                }
                if (body.LinearVelocity.Y > 0)
                {
                    currSpriteAni = g.getSpriteAni(ahName + "AirDown");
                }
                else if (body.LinearVelocity.Y <= 0)
                {
                    currSpriteAni = g.getSpriteAni(ahName + "AirUp");
                }


            }
            else
            {
                currSpriteAni = g.getSpriteAni(ahName + "Basic");

            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            currSpriteAni.Update();
            /*if (currSpriteAni.getStateCount() == 1)
            {
                spriteBatch.Draw(currSprite.index, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X),
                   (int)ConvertUnits.ToDisplayUnits(body.Position.Y), (int)currSprite.index.Width, (int)currSprite.index.Height), null, Color.White,
                   body.Rotation, origin, SpriteEffects.None, (10 + 1) / 100f);
            }
            else
            {*/
                currSpriteAni.draw(spriteBatch, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)-shiftValu,
                   (int)ConvertUnits.ToDisplayUnits(body.Position.Y), (int)currSpriteAni.widthOf(), (int)currSpriteAni.heightOf()), Color.White,origin,lookLeft);

            //}
        
        }


        public Vector2 Position
        {
            get
            {
                return (ConvertUnits.ToDisplayUnits(body.Position) + Vector2.UnitY * centerOffset);
            }
        }
    }
}
