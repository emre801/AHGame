using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Storage;

namespace AHGame
{
    public class LevelController
    {
        public Game1 g;
        int level = 1;
        int world = 1;
        public LevelController(Game1 g)
        {
            this.g = g;
            initLevels("World1");
        }

        public void readLevel()
        {
            String path = "";
            path=Directory.GetCurrentDirectory();
            
            int pathIndex = path.IndexOf("bin");
            path = @"Level" + level + @".txt";
            string file;
            if (!templates.TryGetValue(path, out file))
            {
                try
                {
                    file = templates.Values.First<string>();
                    Console.WriteLine("Level " + path + " not found.");
                    level = 1;
                }
                catch
                {
                    level = 1;
                }
            }
            if (file != null)
                createLevel(file);
        }

        public void restartLevel()
        {
            g.entities.Clear();
            g.world = new FarseerPhysics.Dynamics.World(new Vector2(0, 5));
            g.world2 = new FarseerPhysics.Dynamics.World(new Vector2(0, 0));
            g.drawingTool.blackAlpha = 1;
            g.restartLevel = false;
            //g.players.Clear();
            g.drawingTool.cam._pos = new Vector2(-100, -100);
            readLevel();

        }
        public void nextLevel()
        {
            level++;
            g.drawingTool.blackAlpha = 1;
            g.drawingTool.blackAlphaOut = 0f;
            g.blackOut = false;
            g.nextLevel = false;
            g.drawingTool.cam._pos = new Vector2(-100, -100);
            restartLevel();
        }

        public void createLevel(String file)
        {

            StringReader sr = new StringReader(file);
            String line;
            char[] delimiterChars = { ' ', ',', ':', '\t' };

            LinkedList<Block> blocks = new LinkedList<Block>();

            while ((line = sr.ReadLine()) != null)
            {

                string[] words = line.Split(delimiterChars);
                if(words[0].Equals("CAM"))
                {
                    float zoom = System.Convert.ToSingle(words[1]);
                    g.drawingTool.cam.Zoom = zoom;
                    continue;

                }
                if (words[0].Equals("Demi"))
                {
                    g.drawingTool.demension = new Rectangle((int)System.Convert.ToSingle(words[1]), (int)System.Convert.ToSingle(words[2]),
                         (int)System.Convert.ToSingle(words[3]),
                         (int)System.Convert.ToSingle(words[4]));
                    continue;

                }
                if(words[0].Equals("CPOS"))
                {
                    float xCamPos = System.Convert.ToSingle(words[1]);
                    float yCamPos = System.Convert.ToSingle(words[2]);
                    g.drawingTool.cam.Pos = new Vector2(xCamPos, yCamPos);
                    continue;

                }
                if (words[0].Equals("BGCOLOR"))
                {
                    float red = System.Convert.ToSingle(words[1]);
                    float green = System.Convert.ToSingle(words[2]);
                    float blue = System.Convert.ToSingle(words[3]);
                    g.drawingTool.bgColor = new Color(new Vector3(red / 252f, green / 252f, blue / 252f));
                    continue;
                }



                float x = System.Convert.ToSingle(words[0]);
                float y = System.Convert.ToSingle(words[1]);
                String spriteName = words[2];
                if (words[3].Equals("Block"))
                {
                    bool interactWithPlayer=Convert.ToInt32(words[8])==1?true:false;
                    Block newBlock= new Block(g,new Vector2(x,y),spriteName,System.Convert.ToSingle(words[4]), System.Convert.ToSingle(words[5]), Convert.ToInt32(words[6])+10,false,(Convert.ToInt32(words[7])),interactWithPlayer);
                    blocks.AddFirst(newBlock);
                }
                if (words[3].Equals("DeathBlock"))
                {
                    Sprite sprite = g.getSprite(spriteName);
                    Block newBlock = new Block(g, new Vector2(x, y), spriteName, sprite.index.Height, sprite.index.Width, 0.1f, true, System.Convert.ToSingle(words[4]),true);
                    blocks.AddFirst(newBlock);

                }
                if (words[3].Equals("GoalBlock"))
                {
                    Sprite sprite = g.getSprite(spriteName);
                    GoalBlock goalBlock = new GoalBlock(g, new Vector2(x, y), spriteName, sprite.index.Height, sprite.index.Width);
                    blocks.AddFirst(goalBlock);
                }
                if (words[3].Equals("SuperBack"))
                {
                    Sprite sprite = g.getSprite(spriteName);
                    Block newBlock = new Block(g, new Vector2(x, y), spriteName, System.Convert.ToSingle(words[4]), System.Convert.ToSingle(words[5]), 0, false, 0, false);
                    blocks.AddFirst(newBlock);
                }

            }

            foreach (Block block in blocks)
                g.addEntity(block);

            //Player player = new Player(g, new Vector2(0, 0), g.playerOneControls,"Michael");
            g.playerSelect.setPlayers();
            List<Player> players = g.playerSelect.getPlayers();

            foreach (Player p in players)
            {
                g.addEntity(p);
                foreach (Player p2 in players)
                    p2.fixture.CollisionFilter.IgnoreCollisionWith(p.fixture);
            }
            //g.players.AddLast(player);
           // g.addEntity(player);

        }
        public void writeLevel()
        {
            //TODO add this later

        }
        private static Dictionary<string, string> templates = new Dictionary<string, string>();
        private static void initLevels(String path)
        {
            
            DirectoryInfo di = new DirectoryInfo(@"Content\" + path);
            String fullname = di.FullName;

            //This clears the cache templates so that new Maps can be loaded
            templates = new Dictionary<string, string>();
            foreach (FileInfo fi in di.GetFiles())
            {
                    
                String fullPathName = fi.FullName;
                Stream stream=TitleContainer.OpenStream(@"Content\"+path+"\\"+fi.Name);
                System.IO.StreamReader sReader = new System.IO.StreamReader(stream);
                String text = sReader.ReadToEnd();
                templates.Add(fi.Name, text);
            } 
        }
    }
}
