using System;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain: Microsoft.Xna.Framework.Game {
        int width = 0;
        int height = 0;
        GraphicsDeviceManager graphics;

        Microsoft.Xna.Framework.Graphics.Viewport defaultViewport;
        Microsoft.Xna.Framework.Graphics.Viewport mainViewport;
        Microsoft.Xna.Framework.Graphics.Viewport leftViewport;
        Microsoft.Xna.Framework.Graphics.Viewport rightViewport;
        Microsoft.Xna.Framework.Graphics.Viewport bottomViewport;
        Microsoft.Xna.Framework.Graphics.Viewport mapViewport;

        int ScreenHeight { get { return Renderer.ScreenHeight; } }
        int ScreenWidth { get { return Renderer.ScreenWidth; } }
        UIState State {
            get { return Instance.CurrentState; }
        }
        GameCore.Viewport Viewport { get { return Instance.Viewport; } }
        //InteractionController Controller { get { return Instance.Controller; } }

        public MainCore Instance { get { return MainCore.Instance; } }


        public static WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();

        public static void PlayMusicFromURL(string url) {
            player.URL = url;

            player.settings.volume = 100;

            player.controls.play();
        }

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //PlayMusicFromURL("SpaceOddity.mp3");
            //PlayMusicFromURL("http://s1.castserver.net:8006");
        }

        void ProcessInput() {
            var mouseState = Mouse.GetState();
            var keyState = Keyboard.GetState();
            var pressedKeys = keyState.GetPressedKeys();

            // {
            //    GamePadCapabilities capabilities = GamePad.GetCapabilities(0);
            //    if(capabilities.IsConnected) {
            //        GamePadState state = GamePad.GetState(PlayerIndex.One);
            //        if(capabilities.HasLeftXThumbStick) {
            //            if(state.ThumbSticks.Left.X < -0.5f) {
            //                //position.X -= 10.0f;
            //            }
            //            if(state.ThumbSticks.Left.X > 0.5f) {
            //                //   position.X += 10.0f;
            //            }
            //        }

            //        // You can also check the controllers "type"
            //        if(capabilities.GamePadType == GamePadType.GamePad) {
            //            var buttons = Enum.GetValues(typeof(Buttons)).Cast<Buttons>();
            //            List<Buttons> pressedButtons = new List<Buttons>();
            //            pressedButtons.AddRange(buttons.Where(b => state.IsButtonDown(b)));
            //            InterfaceController.ProcessInput(pressedButtons);
            //        }
            //    }
            //}
            InterfaceController.ProcessInput(pressedKeys, mouseState);

            //  MainCore.Cursor = Viewport.Screen2WorldPoint(new CoordPoint(mouseState.X, mouseState.Y));
        }




        void InterfaceController_OnKeysDown(object sender, KeysEventArgs e) {
            //   Debugger.AddLine("key pressed: " + e.Keys + "; ");
            if(e.Keys == Keys.LeftControl)
                ctrlpressed = true;
        }

        bool ctrlpressed = false;

        void InterfaceController_OnKeysUp(object sender, KeysEventArgs e) {
            //   Debugger.AddLine("key released: " + e.Keys + "; ");
            switch(e.Keys) {
                case Keys.F:
                    if(ctrlpressed)
                        SwitchFullScreen();
                    break;
                case Keys.D:
                    if(ctrlpressed)
                        Renderer.SwitchDebugMode();
                    break;
                case Keys.LeftControl:
                    ctrlpressed = false;
                    break;
                case Keys.C:
                    if(ctrlpressed)
                        SwitchCameraMode();
                    break;

                case Keys.O:
                    objIndex++;
                    if(objIndex > MainCore.Instance.Objects.Count - 1)
                        objIndex = 0;
                    break;
                case Keys.OemPlus:
                    zoom -= zoom / 2f;
                    Debugger.AddLine("z: " + zoom.ToString());
                    break;
                case Keys.OemMinus:
                    zoom += zoom / 2f;
                    Debugger.AddLine("z: " + zoom.ToString());
                    break;
                case Keys.Space:
                    MainCore.Instance.Pause = !MainCore.Instance.Pause;
                    break;
            }
        }

        void SwitchCameraMode() {
            if(cameraMode > 1)
                cameraMode = 0;
            else cameraMode++;

        }

        void InterfaceController_OnButtonsUp(object sender, ButtonsEventArgs e) {
            //     Debugger.AddLine("button released: " + e.Buttons + "; ");
        }


        void SwitchFullScreen() {
            try {
                graphics.IsFullScreen = !graphics.IsFullScreen;

                if(!graphics.IsFullScreen) {
                    width = graphics.PreferredBackBufferWidth;
                    height = graphics.PreferredBackBufferHeight;
                }

                graphics.PreferredBackBufferWidth = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Width : width;
                graphics.PreferredBackBufferHeight = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Height : height;

                graphics.ApplyChanges();


                ArrangeViewports();
                //Viewport.SetViewportSize(
                //    graphics.PreferredBackBufferWidth / 3,
                //    graphics.PreferredBackBufferHeight / 3,
                //    graphics.PreferredBackBufferWidth / 3,
                //    graphics.PreferredBackBufferHeight / 3);

                //Debugger.Lines.Add("viewport changed: " + Viewport.PxlWidth + "x" + Viewport.PxlHeight);
            }
            catch { }
        }

        //int width = 0;
        //int height = 0;
        //int mouseWheel = 0;

        int cameraMode = 1;

        //protected override void Draw(GameTime gameTime) {
        //   return;
        //Render(gameTime);
        //base.Draw(gameTime);
        //}
        private void Render(GameTime gameTime) {
            // GraphicsDevice.Viewport = defaultViewport;
            GraphicsDevice.Clear(Color.White);
            //  GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.Viewport = bottomViewport;
            Renderer.RenderInterface(gameTime);

            //return;

            GraphicsDevice.Viewport = defaultViewport;
            Renderer.RenderTotalOverlay(gameTime, GraphicsDevice.Viewport.Bounds);

            switch(cameraMode) {
                case 0: // overall 
                    Viewport.PxlWidth = mainViewport.Width;
                    Viewport.PxlHeight = mainViewport.Height;

                    Viewport.SmoothUpdate = true;
                    MainCore.Instance.ZoomToObjects();

                    GraphicsDevice.Viewport = mainViewport;
                    Renderer.Render(gameTime);

                    break;
                case 1: //splitscreen
                    if (PlayerController.Players.Count > 1) {
                        Viewport.PxlWidth = mainViewport.Width;
                        Viewport.PxlHeight = mainViewport.Height;

                        var s = PlayerController.Players[0].Ship;
                        var l = s.Location;
                        var v = 200;

                        Viewport.SetWorldBounds(l.X - v, l.Y - v, l.X + v, l.Y + v);
                        //Viewport.Centerpoint = PlayerController.Players[0].Ship.Location;
                        Renderer.Render(gameTime);
                    }
                    else if (PlayerController.Players.Count > 1) {
                        Viewport.PxlWidth = leftViewport.Width;
                        Viewport.PxlHeight = leftViewport.Height;

                        Viewport.Scale = zoom;
                        Viewport.SmoothUpdate = false;

                        Viewport.Centerpoint = PlayerController.Players[0].Ship.Location;
                        GraphicsDevice.Viewport = leftViewport;
                        Renderer.Render(gameTime);

                        Viewport.Centerpoint = PlayerController.Players[1].Ship.Location;
                        GraphicsDevice.Viewport = rightViewport;
                        Renderer.Render(gameTime);
                    }
                    break;
                case 2:
                    Viewport.PxlWidth = mainViewport.Width;
                    Viewport.PxlHeight = mainViewport.Height;

                    Viewport.SmoothUpdate = false;

                    Viewport.Scale = zoom;

                    var objects = MainCore.Instance.Objects;

                    if(objIndex >= objects.Count)
                        objIndex = objects.Count - 1;
                    //   var index = Math.Min(objects.Count-1, objIndex);

                    Viewport.Centerpoint = objects[objIndex].Location;

                    GraphicsDevice.Viewport = mainViewport;
                    Renderer.Render(gameTime);
                    break;
            }
        }

        float zoom = 1;
        int objIndex = 0;

        protected override void Initialize() {
            //Renderer.Set(GraphicsDevice, Content.Load<SpriteFont>("Arial"));
            Renderer.Set(GraphicsDevice, Content.Load<SpriteFont>("Font"));
            //Renderer.SpriteBatch = new SpriteBatch(GraphicsDevice);
            InterfaceController.OnKeysUp += InterfaceController_OnKeysUp;
            InterfaceController.OnKeysDown += InterfaceController_OnKeysDown;
            InterfaceController.OnButtonsUp += InterfaceController_OnButtonsUp;

            ArrangeViewports();

            MainCore.Initialize(new GameCore.Viewport(new Vector2(0, 0), new Vector2(leftViewport.Width, leftViewport.Height)));

            //Viewport.SetViewportSize(ScreenWidth, ScreenHeight);
            MainCore.Instance.AddPlanets();

            InterfaceController.AddState(new MenuState(), new GameState()); // order is matters

            var startButton = new Button(100, 50, 200, 40, "start");
            //var b2 = new Button(100, 100, 200, 40, "b2");
            //var b3 = new Button(100, 150, 200, 40, "b3");
            var quitButton = new Button(100, 200, 200, 40, "quit");

            startButton.ButtonClick += ButtonClicked;
            quitButton.ButtonClick += ButtonClicked;
            startButton.ButtonClick += StartButton_ButtonClick;
            quitButton.ButtonClick += QuitButton_ButtonClick;

            SetKeys(startButton, quitButton);

            //    Renderer.Cover = TextureGenerator.CreateTexture(GraphicsDevice, ScreenWidth, ScreenHeight);

            Debugger.LineAdded += Debugger_LineAdded;
            th = new System.Threading.Thread(delegate () {
                OpenConsole();
            });

            th.Start();

            base.Initialize();
        }

        void Debugger_LineAdded(object sender, EventArgs e) {
            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + (sender as List<string>).LastOrDefault());
        }

        Thread th;
        void OpenConsole() {
            string[] logo = new string[] { "╦ ╦┬ ┬┬┌┬┐┌─┐  ╔═╗┌─┐┌─┐┌─┐┌─┐", "║║║├─┤│ │ ├┤   ╚═╗├─┘├─┤│  ├┤ ", "╚╩╝┴ ┴┴ ┴ └─┘  ╚═╝┴  ┴ ┴└─┘└─┘" };
            foreach(string s in logo) {
                Console.WriteLine(s);
            }
            for(;;) {
                string command = Console.ReadLine();
                if(command == "exit") {

                    break;
                }
                if(command == "start") {

                    InterfaceController.Click(PlayerAction.Yes, 1);
                }
                else
                    Console.WriteLine("you say: " + command);
            }
        }

        private static void SetKeys(Button startButton, Button quitButton) {
            InterfaceController.AddControl(0, startButton);
            //InterfaceController.AddControl(0, b2);
            //InterfaceController.AddControl(0, b3);
            InterfaceController.AddControl(0, quitButton);

            InterfaceController.AddKeyBinding(Keys.Up, 1, PlayerAction.Up);
            InterfaceController.AddKeyBinding(Keys.Down, 1, PlayerAction.Down);
            InterfaceController.AddKeyBinding(Keys.Left, 1, PlayerAction.Left);
            InterfaceController.AddKeyBinding(Keys.Right, 1, PlayerAction.Right);
            InterfaceController.AddKeyBinding(Keys.NumPad1, 1, PlayerAction.Yes);
            InterfaceController.AddKeyBinding(Keys.NumPad0, 1, PlayerAction.Tab);

            InterfaceController.AddKeyBinding(Keys.W, 2, PlayerAction.Up);
            InterfaceController.AddKeyBinding(Keys.S, 2, PlayerAction.Down);
            InterfaceController.AddKeyBinding(Keys.A, 2, PlayerAction.Left);
            InterfaceController.AddKeyBinding(Keys.D, 2, PlayerAction.Right);
            InterfaceController.AddKeyBinding(Keys.Z, 2, PlayerAction.Yes);
            InterfaceController.AddKeyBinding(Keys.X, 2, PlayerAction.Tab);

            InterfaceController.AddButtonBinding(Buttons.LeftThumbstickUp, 2, PlayerAction.Up);
            InterfaceController.AddButtonBinding(Buttons.LeftThumbstickDown, 2, PlayerAction.Down);
            InterfaceController.AddButtonBinding(Buttons.LeftThumbstickLeft, 2, PlayerAction.Left);
            InterfaceController.AddButtonBinding(Buttons.LeftThumbstickRight, 2, PlayerAction.Right);
            InterfaceController.AddButtonBinding(Buttons.X, 2, PlayerAction.Yes);
            InterfaceController.AddButtonBinding(Buttons.B, 2, PlayerAction.Tab);
        }

        private void ArrangeViewports() {
            defaultViewport = GraphicsDevice.Viewport;
            mainViewport = defaultViewport;
            leftViewport = defaultViewport;
            rightViewport = defaultViewport;
            bottomViewport = defaultViewport;

            leftViewport.Width /= 2;
            rightViewport.Width /= 2;
            rightViewport.X = leftViewport.Width;
            bottomViewport.Y = bottomViewport.Height - 100;
            bottomViewport.Height = 100;

            mainViewport.Height = bottomViewport.Y; ;
            rightViewport.Height = bottomViewport.Y;
            leftViewport.Height = bottomViewport.Y;
        }

        private void QuitButton_ButtonClick(object sender, EventArgs e) {
            Exit();
        }

        private void StartButton_ButtonClick(object sender, EventArgs e) {
            //MainCore.SwitchState();
        }

        void ButtonClicked(object sender, EventArgs e) {
            //     Debugger.AddLine((sender as Button).Text + " clicked");
        }

        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
        }
        protected override void UnloadContent() {
            WinAdapter.Unload();
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }
        int delay { get { return 2; } }
        int time = 0;
        protected override void Update(GameTime gameTime) {

            if(!th.IsAlive) {
                Exit();
                th.Abort();
            }
            if(IsActive)
                ProcessInput();
            time++;
            if(time >= delay)
                time = 0;
            else return;

            Instance.Step(gameTime);
            //    State = GameState.Space;
            //renderer.TraectoryPath = Player.Calculator.Calculate();
            base.Update(gameTime);



            Render(gameTime);
        }
    }
}
