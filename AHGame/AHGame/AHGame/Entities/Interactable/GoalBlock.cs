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
    class GoalBlock : Block
    {
        public GoalBlock(Game1 g, Vector2 pos, String sName, float height, float width)
            : base(g, pos, sName, height, width, 15, false, 0, true)
        {



        }
        public override bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            g.nextLevel = true;
            g.drawingTool.blackAlphaOut=0;
            g.blackOut = true;
            foreach (Player p in g.playerSelect.getPlayers())
                p.fixture.CollisionFilter.IgnoreCollisionWith(fixtureA);

            return false;
        }
    }
}
