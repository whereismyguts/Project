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
        public static List<Keys> KeysDown { get; private set; } = new List<Keys>();

        public static UIState CurrentState {
            get { return MainCore.Instance.CurrentState; }
        }

        public static void ProcessInput(IEnumerable<Keys> newKeys) {
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
                }
            DoActions();
        }

        static Dictionary<int, ActorKeyPair> KeyActions = new Dictionary<int, ActorKeyPair>();

        static void DoActions() {
            foreach(var keys in KeysDown) {
                int key = (int)keys;
                if(KeyActions.ContainsKey(key))
                    CurrentState.DoAction(KeyActions[key]);
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

        internal static void AddState(params UIState[] states) {
            MainCore.AddStates(states);
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
}