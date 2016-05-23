using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX {

    public class InteractionController {
        List<InteractiveObject> objects = new List<InteractiveObject>();

        public void Add(InteractiveObject obj) {
            if(!objects.Contains(obj))
                objects.Add(obj);
        }
        int pressCoolDown = 0;
        private ButtonState oldState;

        public void HitTest(MouseState state) {
            if(pressCoolDown < 0)
                pressCoolDown++;
            else
                foreach(InteractiveObject obj in objects)
                    if(obj.Contains(state.Position)) {
                        if(state.LeftButton == ButtonState.Released && oldState == ButtonState.Pressed) {
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
                        if(state.LeftButton == ButtonState.Released && oldState == ButtonState.Pressed)
                            obj.IsSelected = false;
                    }
            oldState = state.LeftButton;
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

        public event EventHandler Click;
        public event EventHandler Hover;
        public virtual void HandleMouseClick() {
            if(Click != null)
                Click(this, EventArgs.Empty);
        }
        public virtual void HandleMouseHover() {
            if(Hover != null)
                Hover(this, EventArgs.Empty);
        }
        public abstract bool Contains(Point position);
    }
}
