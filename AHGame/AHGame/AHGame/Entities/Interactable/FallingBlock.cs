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
    class FallingBlock:Block
    {
        public FallingBlock(Game1 g, Vector2 pos, String sName, float height, float width, float drawLevel,bool isDeathBlock, float rotation, bool interactWithPlayer)
            :base(g, pos,sName, height, width, drawLevel,isDeathBlock, rotation, interactWithPlayer)
        {

        }
        protected override void SetUpPhysics(Vector2 position)
        {
            float mass = 1000;
            float width = this.width;
            float height = this.height;
            World world = g.world;
            fixture = FixtureFactory.CreateRectangle(world, (float)ConvertUnits.ToSimUnits(width), (float)ConvertUnits.ToSimUnits(height), mass);
            body = fixture.Body;
            fixture.Body.BodyType = BodyType.Dynamic;
            fixture.Restitution = 0.3f;
            fixture.Friction = 0.1f;
            body.Position = ConvertUnits.ToSimUnits(position);
            centerOffset = position.Y - (float)ConvertUnits.ToDisplayUnits(body.Position.Y); //remember the offset from the center for drawing
            body.IgnoreGravity = false;
            body.FixedRotation = true;
            body.LinearDamping = 0.5f;
            body.AngularDamping = 1f;
            body.Rotation = rotationAngle * (float)Math.PI / 180f;
        }
    }
}
