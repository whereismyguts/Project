using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameCore GameCore;
        Texture2D dummyTexture;
        Texture2D planetTexture;
        Texture2D shipTexture;
        SpriteFont font;

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            GameCore = new GameCore();
            Helper.CalculateTileSize(GraphicsDevice.Viewport.Bounds, GameCore.Viewport);
            base.Initialize();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            planetTexture = Content.Load<Texture2D>("planet1");
            shipTexture = Content.Load<Texture2D>("ship1");
            font = Content.Load<SpriteFont>("Arial");
            // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            //  if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //      Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.Down))
                GameCore.Viewport.Move(0, 1);


                

            // TODO: Add your update logic here
            mousePosition = Mouse.GetState().Position;
            rotation+=0.1f;
            base.Update(gameTime);

        }
        float rotation = 0;


        Point mousePosition;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {

            //int pxlX = 0;
            //Color curColor = Color.Gray;

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            

            //for(int cellX = GameCore.Viewport.LeftTop.X; cellX < GameCore.Viewport.RightBottom.X; cellX++) {
            //    int pxlY = 0;
            //    curColor = curColor == Color.LightGray ? Color.Gray : Color.LightGray;
            //    for(int cellY = GameCore.Viewport.LeftTop.Y; cellY < GameCore.Viewport.RightBottom.Y; cellY++) {
            //        curColor = curColor == Color.LightGray ? Color.Gray : Color.LightGray;
            //        spriteBatch.Draw(dummyTexture, new Rectangle(pxlX, pxlY, Helper.TileSize, Helper.TileSize), curColor); //GameCore.World[cellX,cellY].Color

            //        pxlY += Helper.TileSize;
            //    }
            //    pxlX += Helper.TileSize;
            //}
            spriteBatch.DrawString(font, "TEST", new Vector2(100, 100), Color.Red);
            spriteBatch.Draw(planetTexture, new Rectangle(mousePosition.X, mousePosition.Y, 150, 150), Color.White);
            spriteBatch.Draw(shipTexture, new Vector2(100,100), null, Color.White, rotation, new Vector2(32,40), .2f, SpriteEffects.None, 0f);
            // spriteBatch.Draw(dummyTexture, new Rectangle(mousePosition.X, mousePosition.Y, 10, 10), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        
    }
}
