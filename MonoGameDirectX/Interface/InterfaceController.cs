using GameCore;
using Microsoft.Xna.Framework.Input;
using MonoGameDirectX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public static class InterfaceController {

        public static UIState CurrentState {
            get { return MainCore.Instance.CurrentState; }
        }



        static Dictionary<int, ActorKeyPair> KeyActions = new Dictionary<int, ActorKeyPair>();

        public static void DoActions() {
            //if(InteractionController.KeysUp.Contains(32))
            //    MainCore.Instance.Pause();
            foreach(var key in InteractionController.KeysPressed) {
                if(KeyActions.ContainsKey(key))
                    CurrentState.DoAction(KeyActions[key]);
            }
        }

        //public static void Switch(int) {
        //    if(state.GetType() != State.GetType())
        //        State = state;
        //}

        //internal static void SelectPrev(int actor) {
        //    CurrentState.Select(actor);
        //}

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
}