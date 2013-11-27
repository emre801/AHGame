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
        public int drawLevel = 0;
        String blockName;
        Sprite sprite;
        SpriteStripAnimationHandler bgSquare;
        public int spriteCounter = 0;
        public bool clickToDrag = false;
        public CreatorBlock(Game1 g)
            : base(g)
        {
            blockType = BlockType.Normal;
            LoadContent();
            bgSquare = g.getSpriteAni("bgSquare");
            ci = new CreatorInput();
            blockName = g.blockArray[spriteCounter];
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

        public int verticalValue()
        {
            if (ci.isUpPressed())
                return 1;
            if (ci.isDownPressed())
                return -1;
            return 0;

        }

        public int horizontalValue()
        {
            if (ci.isRightPressed())
                return 1;
            if (ci.isLeftPressed())
                return -1;
            return 0;
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
            writeLevel();
            deleteBlock();
            changeBlock();
            changeDrawLayer();

        }
        public void changeDrawLayer()
        {
            if (ci.isChangeLayerUpPressed())
                drawLevel++;
            if (ci.isChangeLayerDownPressed())
                drawLevel--;
            if (drawLevel < 0)
                drawLevel = 100;
            if (drawLevel > 100)
                drawLevel = 0;
        }

        public void changeBlock()
        {
            bool hasChange = false;
            if (ci.isIterateBlockUpPressed())
            {
                spriteCounter++;
                hasChange = true;
            }
            if (ci.isIterateBlockDownPressed())
            {
                spriteCounter--;
                hasChange = true;
            }
            if (hasChange)
            {
                if (spriteCounter < 0)
                    spriteCounter = g.blockArray.Length-1;
                else if (spriteCounter >= g.blockArray.Length)
                    spriteCounter = 0;
                blockName = g.blockArray[spriteCounter];
                sprite = g.getSprite(blockName);
            }

        }

        

        public void writeLevel()
        {
            if (ci.isWritePressed())
            {
                LinkedList<String> lines = new LinkedList<String>();
                String path = @"Content\\CreatorLevels\\Level1.txt";
                foreach (Entity e in g.entities)
                {
                    if (e is Interactable)
                    {
                        Interactable i = (Interactable)e;
                        lines.AddLast(i.getStringForWrite());
                    }
                }
                System.IO.File.WriteAllLines(path, lines);
            }

        }

        public void rotateBlock()
        {
            if (ci.isRotatePressed())
            {
                rotation += 90;
                rotation = rotation % 360;
            }
        }

        public void deleteBlock()
        {
            if (ci.isRightMouseReleased())
            {
                Rectangle delete = new Rectangle((int)position.X, (int)position.Y, 1, 1);
                foreach (Entity e in g.entities)
                {
                    if (e is Interactable)
                    {
                        Interactable i = (Interactable)e;
                        Rectangle iRect = new Rectangle((int)i.pos.X-(int)i.width/2, (int)i.pos.Y-(int)i.height/2,(int) i.width,(int) i.height);
                        if (iRect.Contains(delete) || iRect.Intersects(delete))
                        {
                            e.dispose = true;
                        }
                    }
                }
            }

        }


        public void createBlock()
        {
            if (ci.isShiftPressed())
            {
                clickToDrag = !clickToDrag;
            }
            if (clickToDrag)
            {
                if (ci.isLeftMousePressed())
                {
                    p1 = position;
                    mousePosition = MousePosition.PressedDown;
                    if (blockType == BlockType.GoalBlock)
                        createGoalBlock();
                }
                if (ci.isLeftMouseReleased())
                {
                    switch (blockType)
                    {
                        case BlockType.Normal:
                            createNormalBlock(Math.Abs(p1.X - position.X), Math.Abs(p1.Y - position.Y));
                            break;
                        case BlockType.DeathBlock:
                            createDeathBlock(Math.Abs(p1.X - position.X), Math.Abs(p1.Y - position.Y));
                            break;

                    }
                    mousePosition = MousePosition.Ideal;
                }
            }
            else
            {
                if (ci.isLeftMouseReleased())
                {
                    float width = sprite.index.Width;
                    p1 = position;
                    float height = sprite.index.Height;
                    switch (blockType)
                    {
                        case BlockType.Normal:
                            
                            createNormalBlock(width,height);
                            break;
                        case BlockType.DeathBlock:
                            createDeathBlock(width,height);
                            break;

                    }

                }


            }
        }

        public void createNormalBlock(float width,float height)
        {
            //float width=Math.Abs(p1.X-position.X);
            //float height=Math.Abs(p1.Y-position.Y);
            if (width == 0 || height == 0)
                return;
            Block b = new Block(g, p1+new Vector2(width/2,height/2), blockName, height, width, (drawLevel+1)/101f, false, (float)rotation, true);
            g.addEntity(b);
        }
        public void createDeathBlock(float width,float height)
        {
            //float width = Math.Abs(p1.X - position.X);
            //float height = Math.Abs(p1.Y - position.Y);
            Block b = new Block(g, p1 + new Vector2(width / 2, height / 2), blockName, height, width, (drawLevel+1)/101f, true, (float)rotation, true);
            g.addEntity(b);
        }
        public void createGoalBlock()
        {
            GoalBlock gb = new GoalBlock(g, position, "ahLogo", 63, 63);
            g.addEntity(gb);
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
            //spriteBatch.Draw(cursor.index, new Rectangle((int)position.X,(int)position.Y,40,40), Color.White);
            float width=Math.Abs(p1.X-position.X);
            float height=Math.Abs(p1.Y-position.Y);
            if (mousePosition == MousePosition.PressedDown && clickToDrag)
            {
                Vector2 shift = Vector2.Zero;
                if (rotation == 90)
                    shift = new Vector2(width, 0);
                if (rotation == 180)
                    shift = new Vector2(width, height);
                if (rotation == 270)
                    shift = new Vector2(0, height);
                
                spriteBatch.Draw(sprite.index, new Rectangle((int)(p1.X + shift.X), (int)(p1.Y + shift.Y), (int)width, (int)height), new Rectangle(0, 0, (int)sprite.index.Width, (int)sprite.index.Height), Color.White,
                     (float)(rotation * Math.PI / 180f), Vector2.Zero, SpriteEffects.None, 0);
                
                
            }
            else
            {
                Vector2 shift = Vector2.Zero;
                width = sprite.index.Width;
                height = sprite.index.Height;
                if (rotation == 90)
                    shift = new Vector2(width, 0);
                if (rotation == 180)
                    shift = new Vector2(width, height);
                if (rotation == 270)
                    shift = new Vector2(0, height);
                
                spriteBatch.Draw(sprite.index, new Rectangle((int)(position.X + shift.X), (int)(position.Y + shift.Y), (int)width, (int)height), new Rectangle(0, 0, (int)sprite.index.Width, (int)sprite.index.Height), Color.White,
                (float)(rotation * Math.PI / 180f), Vector2.Zero, SpriteEffects.None, 0);
            }

        }

        public void drawBackgroundPatter(SpriteBatch spriteBatch)
        {
            for (int i = -3000; i < 3000; i += bgSquare.widthOf() / 2)
                for (int j = -3000; j < 3000; j += bgSquare.heightOf() / 2)
                    bgSquare.draw(spriteBatch, new Rectangle(i, j, (int)bgSquare.widthOf(), (int)bgSquare.heightOf()), Color.White * 0.35f * 1, new Vector2(0, 0), false);

        }
        public void DrawMouse(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cursor.index, new Rectangle((int)position.X, (int)position.Y, 40, 40), Color.White);
        }


    }
}
