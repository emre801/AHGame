using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace AHGame
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        public DrawingTool drawingTool;
        public World world,world2;

        public List<Entity> entities = new List<Entity>();
        public List<Entity> entitiesToAdd = new List<Entity>();
        public List<Entity> entitiesToRemove = new List<Entity>();
        public LinkedList<String> blockNames = new LinkedList<String>();
        public String[] blockArray;
        //Player controls
        public Input playerOneControls;
        public Input playerTwoControls;
        public Input playerThreeControls;
        public Input playerFourControls;

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        public Sprite[] spriteDict;
        Dictionary<string, SpriteStripAnimationHandler> spritesAni = new Dictionary<string, SpriteStripAnimationHandler>();
        public Title title;
        public PlayerSelect playerSelect;

        public enum gameStates { TITLE, GAME, PLAYERSELECT,CREATOR }
        public gameStates currState = gameStates.TITLE;

        public LevelController levelControl;
        public SFXController sfxControl;
        public MusicController musicControl;
        public bool restartLevel = false;
        public bool nextLevel = false;

        public Sprite black;
        public bool blackOut=false;

        public SpriteFont[] fonts = new SpriteFont[1];
        public CreatorBlock cb;

        //public LinkedList<Player> players = new LinkedList<Player>();

        public Game1()
        {
            world = new World(new Vector2(0, 5.0f));
            world2 = new World(new Vector2(0, 0));
            Content.RootDirectory = "Content";
            ConvertUnits.SetDisplayUnitToSimUnitRatio(30);
            drawingTool = new DrawingTool(this);
            setUpControls();
            title = new Title(this);
            playerSelect = new PlayerSelect(this);
            levelControl = new LevelController(this);
            sfxControl = new SFXController(Content);
            musicControl = new MusicController(Content);
        }

        public Vector2 getMidPointOfPlayers()
        {
            Vector2 midPoint = Vector2.Zero;
            List<Player> tempPlayers = playerSelect.getPlayers();
            if (tempPlayers == null)
                return Vector2.Zero;
            foreach (Player p in tempPlayers)
            {
                midPoint += p.Position;
            }
            midPoint = new Vector2(midPoint.X / tempPlayers.Count,
                midPoint.Y / tempPlayers.Count);
            return midPoint;

        }



        public void setUpControls()
        {
            int numOfKeyBoards = 1;
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                playerOneControls = new XboxInput(this, PlayerIndex.One);
            else
            {
                playerOneControls = new KeyBoardInput(this);
                numOfKeyBoards--;
            }

            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                playerTwoControls = new XboxInput(this, PlayerIndex.Two);
            else
            {
                playerTwoControls = new KeyBoardInput(this);
                if (numOfKeyBoards == 0)
                    ((KeyBoardInput)playerTwoControls).secondControls();
                numOfKeyBoards--;
            }

            if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                playerThreeControls = new XboxInput(this, PlayerIndex.Three);
            else
            {
                playerThreeControls = new KeyBoardInput(this);//fix this
                if(numOfKeyBoards==0)
                    ((KeyBoardInput)playerThreeControls).secondControls();
                numOfKeyBoards--;
            }

            if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                playerFourControls = new XboxInput(this, PlayerIndex.Four);
            else
                playerFourControls = new KeyBoardInput(this);//fix this

        }

        public void addEntity(Entity e)
        {
            entitiesToAdd.Add(e);
        }
        public int addSprite(String fname, String path)
        {
            blockNames.AddLast(fname);
            sprites.Add(fname, new Sprite(Content, path));
            return 0;
        }

        public void addFont(String fileName)
        {
            fonts[0] = Content.Load<SpriteFont>("Fonts\\"+fileName);
        }
        public int addSpriteAni(String fname, String path)
        {
            int p1 = fname.IndexOf("$");
            int p2 = fname.IndexOf("@");

            int stateCount=Convert.ToInt16(fname.Substring(p1+1,p2-p1-1));
            int frameRate=Convert.ToInt16(fname.Substring(p2+1));
            Sprite sprite=new Sprite(Content,path);
            SpriteStripAnimationHandler sAni = new SpriteStripAnimationHandler(sprite, stateCount, frameRate);

            String name = fname.Substring(0, p1);
            spritesAni.Add(name, sAni);
            return 0;
        }

        public SpriteStripAnimationHandler getSpriteAni(String fName)
        {
            if (spritesAni.ContainsKey(fName))
                return spritesAni[fName];
            else
                return spritesAni["Error"];
        }

        public Sprite getSprite(String fName)
        {
            if (sprites.ContainsKey(fName))
                return sprites[fName];
            else
                return sprites["Error"];
        }
       
        protected override void Initialize()
        {
            drawingTool.initialize();
            //title.Initialize();
            base.Initialize();
        }

        public int addUserSprite(String fname, String path)
        {
            Texture2D file;
            RenderTarget2D result;
            GraphicsDevice gd = drawingTool.getGraphicsDevice();
            path = "Content//" + path + ".png";
            using (FileStream sourceStream = new FileStream(path, FileMode.Open))
            {
                file = Texture2D.FromStream(gd, sourceStream);
            }
            result = new RenderTarget2D(gd, file.Width, file.Height);
            gd.SetRenderTarget(result);
            gd.Clear(Color.Black);

            var blendColor = new BlendState
            {
                ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue,
                AlphaDestinationBlend = Blend.Zero,
                ColorDestinationBlend = Blend.Zero,
                AlphaSourceBlend = Blend.SourceAlpha,
                ColorSourceBlend = Blend.SourceAlpha
            };

            var spriteBatch = new SpriteBatch(gd);
            spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
            spriteBatch.Draw(file, file.Bounds, Color.White);
            spriteBatch.End();

            //Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
            var blendAlpha = new BlendState
            {
                ColorWriteChannels = ColorWriteChannels.Alpha,
                AlphaDestinationBlend = Blend.Zero,
                ColorDestinationBlend = Blend.Zero,
                AlphaSourceBlend = Blend.One,
                ColorSourceBlend = Blend.One
            };

            spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
            spriteBatch.Draw(file, file.Bounds, Color.White);
            spriteBatch.End();

            //Release the GPU back to drawing to the screen
            gd.SetRenderTarget(null);

            Texture2D finalFile = result as Texture2D;
            //Sprite sp = new Sprite(finalFile, fname);

            blockNames.AddLast(fname);
            sprites.Add(fname, new Sprite(finalFile, fname));

            return 0;
        }

        
        protected override void LoadContent()
        {
            //Add Title Assets
            LoadFileFromFolder("Title",addSpriteAni);
            //Add World1 Assets
            LoadFileFromFolder("World1Assets",addSprite);

            //Add Michael Assets
            LoadFileFromFolder("Michael",addSpriteAni);
            //Add Gavin Assets
            LoadFileFromFolder("Gavin", addSpriteAni);
            //Add Jack Assets
            LoadFileFromFolder("Jack", addSpriteAni);
            //Add Ray Assets
            LoadFileFromFolder("Ray", addSpriteAni);
            //Add Ryan Assets
            LoadFileFromFolder("Ryan", addSpriteAni);
            //Add Geoff Assets
            LoadFileFromFolder("Geoff", addSpriteAni);

            //Add Menu sounds
            LoadFileFromFolder("MenuSounds", sfxControl.addSound);
            //Add Player sounds
            LoadFileFromFolder("PlayerSounds", sfxControl.addSound);
            //LoadMusic
            LoadFileFromFolder("Songs", musicControl.addMusic);
            //Load trees
            LoadFileFromFolder("Trees", addSprite);
            //Load playerSelectImages
            LoadFileFromFolder("playerSelectImages", addSprite);
            //Load mouse
            LoadFileFromFolder("Mouse", addSprite);
            //Load User images
            LoadFileFromFolder("UserImages", addUserSprite);

            addFont("PressStart2P");
            title.LoadContent();

            spriteDict = new Sprite[sprites.Count];
            sprites.Values.CopyTo(spriteDict,0);
            playerSelect.LoadContent();

            black = getSprite("black");
            blockArray = new String[blockNames.Count];
            blockNames.CopyTo(blockArray, 0);
        }



        //pass in the folder name, and it'll automatically run trough files
        //then it'll run the appropriate add method ex addSprite, addMusic,addSFX
        private void LoadFileFromFolder(String path, Func<string, string,int> addMethod)
        {

            DirectoryInfo di = new DirectoryInfo(@"Content\" + path);
            String fullname = di.FullName;

            //This clears the cache templates so that new Maps can be loaded
            foreach (FileInfo fi in di.GetFiles())
            {
                int length = fi.Name.Length - 4;
                String name = fi.Name.Substring(0, length);
                //addSprite(name, path + "\\" + name);
                addMethod(name, path + "\\" + name);

            }
        }


        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        public void updatePlayerControls()
        {
            playerOneControls.Update();
            playerTwoControls.Update();
            playerThreeControls.Update();
            playerFourControls.Update();
        }

        //The reason for having this is later on during development I might need to do some house work before I exit
        //ie save a file, make sure a connection closes, etc
        public void gameExit()
        {
            this.Exit();
        }
        
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            //drawingTool.moveCamera(playerOneControls);
            //updateWorld by step
            if(!this.blackOut)
                world.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.002));
            else
                world.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.0002));
            updatePlayerControls();
            if (currState == gameStates.TITLE)
            {
                title.Update();
            }
            else if (currState == gameStates.PLAYERSELECT)
            {
                playerSelect.Update();
            }
            else
            {
                
                updateGameState();
            }

            base.Update(gameTime);
        }


        public void updateGameState()
        {
            foreach (Entity e in entities)
                {
                    if (!e.dispose)
                    {
                        e.Update();
                    }
                    else
                    {
                        entitiesToRemove.Add(e);
                    }
                }
                
                
                foreach (Entity e in entitiesToAdd)
                    entities.Add(e);
                foreach (Entity e in entitiesToRemove)
                    entities.Remove(e);

                entitiesToAdd.Clear();
                entitiesToRemove.Clear();
                if (restartLevel)
                    levelControl.restartLevel();
                if (nextLevel)
                {
                    if(drawingTool.blackAlphaOut>=1)
                    levelControl.nextLevel();
                }
                drawingTool.Update();
            

        }
        
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            
            drawingTool.draw();

            base.Draw(gameTime);
        }
    }
}
