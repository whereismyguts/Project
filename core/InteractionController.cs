using Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GameCore {

    public class InteractionController {
        GameState gameState;
        bool oldMousePressed;
        Dictionary<GameState, List<InteractiveObject>> interfaces = new Dictionary<GameState, List<InteractiveObject>>();
        int pressCoolDown = 0;

        public void Add(InteractiveObject obj, GameState state) {
            if(!interfaces.ContainsKey(state))
                interfaces[state] = new List<InteractiveObject>();
            if(!interfaces[state].Contains(obj))
                interfaces[state].Add(obj);
        }

        public List<InteractiveObject> GetActualInterface() {
            return interfaces.ContainsKey(gameState) ? interfaces[gameState] : new List<InteractiveObject>();
        }

        public void HitTest(bool pressed, object position) {
            if(pressCoolDown < 0)
                pressCoolDown++;
            else
                if(interfaces.ContainsKey(gameState))
                foreach(InteractiveObject obj in interfaces[gameState])
                    if(obj.Contains(position)) if(!pressed && oldMousePressed) {
                            obj.HandleMouseClick();
                            obj.IsSelected = true;
                            pressCoolDown = -10;
                        }
                        else {
                            obj.HandleMouseHover();
                            obj.IsHighlighted = true;
                        }
                    else {
                        obj.IsHighlighted = false;
                        if(!pressed && oldMousePressed)
                            obj.IsSelected = false;
                    }
            oldMousePressed = pressed;
        }

        public void UpdateState(GameState state) {
            gameState = state;
        }
    }
}
