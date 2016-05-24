using System;
using System.Collections.Generic;
using System.Linq;


namespace GameCore {

    public class InteractionController {
        Dictionary<GameState, List<InteractiveObject>> interfaces = new Dictionary<GameState, List<InteractiveObject>>();

        public void Add(InteractiveObject obj, GameState state) {
            if(!interfaces.ContainsKey(state))
                interfaces[state] = new List<InteractiveObject>();
            if(!interfaces[state].Contains(obj))
                interfaces[state].Add(obj);
        }
        int pressCoolDown = 0;
        bool oldMousePressed;
        GameState gameState;

        public void HitTest(bool pressed, object position) {
            if(pressCoolDown < 0)
                pressCoolDown++;
            else
                if(interfaces.ContainsKey(gameState))
                foreach(InteractiveObject obj in interfaces[gameState])
                    if(obj.Contains(position)) {
                        if(!pressed && oldMousePressed) {
                            obj.HandleMouseClick();
                            obj.IsSelected = true;
                            pressCoolDown = -10;
                        }
                        else {
                            obj.HandleMouseHover();
                            obj.IsHighlited = true;
                        }
                    }
                    else {
                        obj.IsHighlited = false;
                        if(!pressed && oldMousePressed)
                            obj.IsSelected = false;
                    }
            oldMousePressed = pressed;
        }

        public List<InteractiveObject> GetActualInterface() {
            return interfaces.ContainsKey(gameState) ? interfaces[gameState] : new List<InteractiveObject>();
        }

        public void UpdateState(GameState state) {
            gameState = state;
        }
    }

    public abstract class InteractiveObject {
        bool isSelected;
        public virtual bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                SelectedChanged();
            }
        }

        protected virtual void SelectedChanged() {

        }

        bool isHighligted;
        public virtual bool IsHighlited
        {
            get
            {
                return isHighligted;
            }
            set
            {
                isHighligted = value;
                HighligtedChanged();
            }
        }

        protected virtual void HighligtedChanged() {

        }

        protected  event EventHandler Click;
        protected  event EventHandler Hover;
        protected internal virtual void HandleMouseClick() {
            if(Click != null)
                Click(this, EventArgs.Empty);
        }
        protected internal virtual void HandleMouseHover() {
            if(Hover != null)
                Hover(this, EventArgs.Empty);
        }
        public abstract bool Contains(object position);
    }
}
