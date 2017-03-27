using GameCore;
using System;
using System.Collections.Generic;

namespace GameCore {
    public static class InterfaceController {

        public static UIState State {
            get { return MainCore.Instance.State; }
            set { MainCore.Instance.State = value; }
        }


        static Dictionary<int, ActorKeyPair> KeyActions = new Dictionary<int, ActorKeyPair>();

        public static void DoActions() {
            //if(InteractionController.KeysUp.Contains(32))
            //    MainCore.Instance.Pause();

            foreach(var key in InteractionController.KeysUp) {
                if(KeyActions.ContainsKey(key))
                    State.DoAction(KeyActions[key]);
            }
        }

        public static void Switch(UIState state) {
            if(state.GetType() != State.GetType())
                State = state;
        }

        internal static void SelectPrev(int actor) {
            State.SelectPrev(actor);
        }
    }

    public class ActorKeyPair {
        //        public int Key { get; set; }
        public int Actor { get; set; }
        public PlayerAction Action { get; set; }
    }








}