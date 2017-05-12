using GameCore;
using Microsoft.Xna.Framework.Input;
using MonoGameDirectX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public static class InterfaceController {
        public static event KeysEventHandler OnKeysUp;
        public static event KeysEventHandler OnKeysDown;
        public static event ButtonsEventHandler OnButtonsUp;
        public static event ButtonsEventHandler OnButtonsDown;
        public static List<Keys> KeysDown { get; private set; } = new List<Keys>();
        public static List<Buttons> ButtonsDown { get; private set; } = new List<Buttons>();

        public static UIState CurrentState {
            get { return MainCore.Instance.CurrentState; }
        }

        public static void ProcessInput(IEnumerable<Keys> newKeys, MouseState mouse) {
            List<Keys> toRemove = new List<Keys>();
            foreach(var key in KeysDown)
                if(!newKeys.Contains(key)) {
                    //key up
                    if(OnKeysUp != null)
                        OnKeysUp(null, new KeysEventArgs(key));
                    toRemove.Add(key);
                }
            KeysDown.RemoveAll(k => toRemove.Contains(k));

            foreach(var key in newKeys)
                if(!KeysDown.Contains(key)) {
                    //key down
                    if(OnKeysDown != null)
                        OnKeysDown(null, new KeysEventArgs(key));
                    KeysDown.Add(key);
                    if(KeyActions.ContainsKey((int)key))
                        CurrentState.DoAction(KeyActions[(int)key], true);
                }
            MainCore.Instance.Cursor = MainCore.Instance.Viewport.Screen2WorldPoint(mouse.X, mouse.Y);

            if(!mousePrevPressed && mouse.LeftButton == ButtonState.Pressed)
                MainCore.Instance.MousePressed();
            if(mousePrevPressed && mouse.LeftButton == ButtonState.Released)
                MainCore.Instance.MouseReleased();

            mousePrevPressed = mouse.LeftButton == ButtonState.Pressed;
            DoActions();
        }

        static bool mousePrevPressed = false;

        public static void ProcessInput(IEnumerable<Buttons> newKeys) {
            List<Buttons> toRemove = new List<Buttons>();
            foreach(var key in ButtonsDown)
                if(!newKeys.Contains(key)) {
                    //key up
                    if(OnButtonsUp != null)
                        OnButtonsUp(null, new ButtonsEventArgs(key));
                    toRemove.Add(key);
                }
            ButtonsDown.RemoveAll(k => toRemove.Contains(k));

            foreach(var key in newKeys)
                if(!ButtonsDown.Contains(key)) {
                    //key down
                    if(OnButtonsDown != null)
                        OnButtonsDown(null, new ButtonsEventArgs(key));
                    ButtonsDown.Add(key);
                    if(ButtonActions.ContainsKey((int)key))
                        CurrentState.DoAction(ButtonActions[(int)key], true);
                }
            DoActions();
        }

        static Dictionary<int, ActorKeyPair> KeyActions = new Dictionary<int, ActorKeyPair>();
        static Dictionary<int, ActorKeyPair> ButtonActions = new Dictionary<int, ActorKeyPair>();

        static void DoActions() {
            foreach(var keys in KeysDown) {
                int key = (int)keys;
                if(KeyActions.ContainsKey(key))
                    CurrentState.DoAction(KeyActions[key], false);
            }
        }
        internal static void AddControl(int state, Control control, int actor = 0) {
            MainCore.AddControl(state, control, actor);
        }

        internal static IEnumerable<Control> GetActualControls() {
            return CurrentState.Controls.Cast<Control>();
        }

        internal static void AddKeyBinding(Keys key, int actor, PlayerAction action) {
            KeyActions.Add((int)key, new ActorKeyPair(actor, action));
        }

        internal static void AddButtonBinding(Buttons key, int actor, PlayerAction action) {
            ButtonActions.Add((int)key, new ActorKeyPair(actor, action));
        }

        internal static void AddState(params UIState[] states) {
            MainCore.AddStates(states);
        }

        internal static void Click(PlayerAction action, int p) {
            CurrentState.DoAction(new ActorKeyPair(p, action), true);
        }
    }
    public class KeysEventArgs: EventArgs {
        readonly Keys keys;
        public Keys Keys { get { return keys; } }
        internal KeysEventArgs(Keys keys) {
            this.keys = keys;
        }
    }
    public delegate void KeysEventHandler(object sender, KeysEventArgs e);

    public class ButtonsEventArgs: EventArgs {
        readonly Buttons buttons;
        public Buttons Buttons { get { return buttons; } }
        internal ButtonsEventArgs(Buttons buttons) {
            this.buttons = buttons;
        }
    }
    public delegate void ButtonsEventHandler(object sender, ButtonsEventArgs e);
}