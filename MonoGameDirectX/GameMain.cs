﻿using System;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain: Microsoft.Xna.Framework.Game {
        int width = 0;
        int height = 0;
        GraphicsDeviceManager graphics;
        int ScreenHeight { get { return Renderer.ScreenHeight; } }
        int ScreenWidth { get { return Renderer.ScreenWidth; } }
        UIState State {
            get { return Instance.CurrentState; }
        }
        GameCore.Viewport Viewport { get { return Instance.Viewport; } }
        //InteractionController Controller { get { return Instance.Controller; } }

        public MainCore Instance { get { return MainCore.Instance; } }

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        void ProcessInput() {
            var mouseState = Mouse.GetState();
            var keyState = Keyboard.GetState();
            var pressedKeys = keyState.GetPressedKeys();

            if(false) {
                GamePadCapabilities capabilities = GamePad.GetCapabilities(0);
                if(capabilities.IsConnected) {
                    GamePadState state = GamePad.GetState(PlayerIndex.One);
                    if(capabilities.HasLeftXThumbStick) {
                        if(state.ThumbSticks.Left.X < -0.5f) {
                            //position.X -= 10.0f;
                        }
                        if(state.ThumbSticks.Left.X > 0.5f) {
                            //   position.X += 10.0f;
                        }
                    }

                    // You can also check the controllers "type"
                    if(capabilities.GamePadType == GamePadType.GamePad) {
                        var buttons = Enum.GetValues(typeof(Buttons)).Cast<Buttons>();
                        List<Buttons> pressedButtons = new List<Buttons>();
                        pressedButtons.AddRange(buttons.Where(b => state.IsButtonDown(b)));
                        InterfaceController.ProcessInput(pressedButtons);
                    }
                }
            }
            InterfaceController.ProcessInput(pressedKeys);

            MainCore.Cursor = Viewport.Screen2WorldPoint(new CoordPoint(mouseState.X, mouseState.Y));
        }




        void InterfaceController_OnKeysDown(object sender, KeysEventArgs e) {
            Debugger.Lines.Add("key pressed: " + e.Keys + "; ");
            if(e.Keys == Keys.LeftControl)
                ctrlpressed = true;
        }

        bool ctrlpressed = false;

        void InterfaceController_OnKeysUp(object sender, KeysEventArgs e) {
            Debugger.Lines.Add("key released: " + e.Keys + "; ");
            switch(e.Keys) {
                case Keys.F:
                    SwitchFillScreen(); break;
                case Keys.D:
                    if(ctrlpressed)
                        Renderer.SwitchDebugMode(); break;
                case Keys.LeftControl:
                    ctrlpressed = false; break;
            }
        }

        void InterfaceController_OnButtonsUp(object sender, ButtonsEventArgs e) {
            Debugger.Lines.Add("button released: " + e.Buttons + "; ");
        }


        void SwitchFillScreen() {
            try {
                graphics.IsFullScreen = !graphics.IsFullScreen;

                if(!graphics.IsFullScreen) {
                    width = graphics.PreferredBackBufferWidth;
                    height = graphics.PreferredBackBufferHeight;
                }

                graphics.PreferredBackBufferWidth = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Width : width;
                graphics.PreferredBackBufferHeight = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Height : height;

                graphics.ApplyChanges();

                Viewport.SetViewportSize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

                Debugger.Lines.Add("viewport changed: " + Viewport.PxlWidth + "x" + Viewport.PxlHeight);
            }
            catch { }
        }

        //int width = 0;
        //int height = 0;
        //int mouseWheel = 0;

        protected override void Draw(GameTime gameTime) {

            //   System.Threading.Thread.Sleep(1000);
            //  if(Controller.Keys.ToList().Contains(32))
            Renderer.Render(gameTime);
            base.Draw(gameTime);
        }
        protected override void Initialize() {
            Renderer.Set(GraphicsDevice, Content.Load<SpriteFont>("Arial"));
            //Renderer.SpriteBatch = new SpriteBatch(GraphicsDevice);
            InterfaceController.OnKeysUp += InterfaceController_OnKeysUp;
            InterfaceController.OnKeysDown += InterfaceController_OnKeysDown;
            InterfaceController.OnButtonsUp += InterfaceController_OnButtonsUp;

            Viewport.SetViewportSize(ScreenWidth, ScreenHeight);

            InterfaceController.AddState(new MenuState(), new GameState()); // order is matters

            var b1 = new Button(100, 50, 100, 40, "b1");
            var b2 = new Button(100, 100, 100, 40, "b2");
            var b3 = new Button(100, 150, 100, 40, "b3");
            var b4 = new Button(100, 200, 100, 40, "b4");

            b1.ButtonClick += ButtonClicked;
            b2.ButtonClick += ButtonClicked;
            b3.ButtonClick += ButtonClicked;
            b4.ButtonClick += ButtonClicked;

            InterfaceController.AddControl(0, b1);
            InterfaceController.AddControl(0, b2);
            InterfaceController.AddControl(0, b3);
            InterfaceController.AddControl(0, b4);

            InterfaceController.AddKeyBinding(Keys.Up, 1, PlayerAction.Up);
            InterfaceController.AddKeyBinding(Keys.Down, 1, PlayerAction.Down);
            InterfaceController.AddKeyBinding(Keys.Left, 1, PlayerAction.Left);
            InterfaceController.AddKeyBinding(Keys.Right, 1, PlayerAction.Right);
            InterfaceController.AddKeyBinding(Keys.RightControl, 1, PlayerAction.Yes);
            InterfaceController.AddKeyBinding(Keys.Tab, 1, PlayerAction.Tab);

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

            base.Initialize();
        }

        void ButtonClicked(object sender, EventArgs e) {
            Debugger.Lines.Add((sender as Button).Text + " clicked");
        }

        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
        }
        protected override void UnloadContent() {
            WinAdapter.Unload();
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            if(IsActive)
                ProcessInput();
            Instance.Update();
            //    State = GameState.Space;
            //renderer.TraectoryPath = Player.Calculator.Calculate();
            base.Update(gameTime);
        }
    }
}
