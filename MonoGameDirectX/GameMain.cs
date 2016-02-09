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
        Point mousePosition;
        Character ship;
        string debugText = "";
        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            GameCore = new GameCore();
            Helper.CalculateTileSize(GraphicsDevice.Viewport.Bounds, GameCore.Viewport);
            ship = GameCore.Character;
            base.Initialize();
        }
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
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            //  if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //      Exit();
            if(Keyboard.GetState().IsKeyDown(Keys.Down))
                GameCore.Viewport.Move(0, 1);
            if(Keyboard.GetState().IsKeyDown(Keys.Up))
                GameCore.Viewport.Move(0, -1);
            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                GameCore.Viewport.Move(-1, 0);
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                GameCore.Viewport.Move(1, 0);
            foreach(GameObjectBase obj in GameCore.Objects)
                obj.Move();
            GameCore.Character.Move();
            mousePosition = Mouse.GetState().Position;

            debugText = ship.GetLocation().ToString();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            foreach(GameObjectBase obj in GameCore.Objects)
              //  if(obj.IsVisible) 
              {
                    var location = obj.GetLocation();
                    spriteBatch.Draw(planetTexture, new Rectangle((int)location.X, (int)location.Y, (int)obj.Size, (int)obj.Size), Color.White);
                }

            spriteBatch.DrawString(font, debugText, new Vector2(0, 0), Color.Red);
            spriteBatch.Draw(dummyTexture, new Rectangle(mousePosition.X, mousePosition.Y, 10, 10), Color.White);
            spriteBatch.Draw(shipTexture, Helper.ToVector2(ship.GetLocation()), null, Color.White, 0f, new Vector2(), .2f, SpriteEffects.None, 0f);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
