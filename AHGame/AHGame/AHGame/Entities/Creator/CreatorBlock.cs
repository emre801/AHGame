using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace AHGame
{
    public class CreatorBlock:Entity
    {
        enum BlockType { Normal, DeathBlock, GoalBlock }
        enum MousePosition { Ideal, PressedDown}
        MousePosition mousePosition;
        BlockType blockType;
        Vector2 position=Vector2.Zero;
        int rotation = 0;
        Sprite cursor;
        CreatorInput ci;
        Vector2 p1;
        int drawLevel = 0;
        String blockName;
        Sprite sprite;
        public CreatorBlock(Game1 g)
            : base(g)
        {
            blockType = BlockType.Normal;
            LoadContent();
            ci = new CreatorInput();
            blockName = "bigBlock";
            sprite = g.getSprite(blockName);
            mousePosition = MousePosition.Ideal;
        }

        public String getPositionAsString()
        {
            return (int)position.X + " " + (int)position.Y;
        }
        public String getRotationAsString()
        {
            return "" + rotation;
        }

        public String getBlockType()
        {
            switch (blockType)
            {
                case BlockType.Normal:
                    return "Normal";
                case BlockType.GoalBlock:
                    return "Goal";
                case BlockType.DeathBlock:
                    return "Death";
            }
            return "";
        }

        public override void LoadContent()
        {
            cursor = g.getSprite("mouse");
        }

        public override void Update()
        {
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            position= Vector2.Transform(mousePosition, Matrix.Invert(g.drawingTool.cam._transform));

            ci.Update();
            changeBlockType();
            rotateBlock();
            createBlock();

        }

        public void rotateBlock()
        {
            if (ci.isRotatePressed())
            {
                rotation += 90;
                rotation = rotation % 360;
            }
        }

        public void createBlock()
        {
            if (ci.isMousePressed())
            {
                p1 = position;
                mousePosition = MousePosition.PressedDown;
                if (blockType == BlockType.GoalBlock)
                    createGoalBlock();
            }
            if (ci.isMouseReleased())
            {
                switch (blockType)
                {
                    case BlockType.Normal:
                        createNormalBlock();
                        break;
                    case BlockType.DeathBlock:
                        createDeathBlock();
                        break;
                   
                }
                mousePosition = MousePosition.Ideal;
            }
        }

        public void createNormalBlock()
        {
            float width=Math.Abs(p1.X-position.X);
            float height=Math.Abs(p1.Y-position.Y);
            Block b = new Block(g, p1+new Vector2(width/2,height/2), blockName, height, width, drawLevel, false, (float)rotation, true);
            g.addEntity(b);
        }
        public void createDeathBlock()
        {
            float width = Math.Abs(p1.X - position.X);
            float height = Math.Abs(p1.Y - position.Y);
            Block b = new Block(g, p1 + new Vector2(width / 2, height / 2), blockName, height, width, drawLevel, true, (float)rotation, true);
            g.addEntity(b);
        }
        public void createGoalBlock()
        {
            
        }


        public void changeBlockType()
        {
            if (ci.isBlockTypePressed())
            {
                switch (blockType)
                {
                    case BlockType.Normal:
                        blockType = BlockType.GoalBlock;
                        break;
                    case BlockType.GoalBlock:
                        blockType = BlockType.DeathBlock;
                        break;
                    case BlockType.DeathBlock:
                        blockType = BlockType.Normal;
                        break;
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cursor.index, new Rectangle((int)position.X,(int)position.Y,40,40), Color.White);
            float width=Math.Abs(p1.X-position.X);
            float height=Math.Abs(p1.Y-position.Y);
            if(mousePosition==MousePosition.PressedDown)
                spriteBatch.Draw(sprite.index, new Rectangle((int)p1.X,(int)p1.Y,(int)width,(int)height), Color.White);
            //draw block based on thing

        }


    }
}
