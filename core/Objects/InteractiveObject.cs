using System;
using System.Linq;

namespace GameCore {
    public abstract class InteractiveObject: IControl {

        bool isHighlighted;
        bool isSelected;

        public virtual bool IsHighlighted {
            get { return isHighlighted; }
            set {
                if (isHighlighted == value)
                    return;
                isHighlighted = value;
            }
        }
        public virtual bool IsSelected {
            get { return isSelected; }
            set {
                if (isSelected == value)
                    return;
                isSelected = value;
            }
        }

        public PlayerAction Tag { get; set; }

        protected event EventHandler Click;
        protected event EventHandler Hover;
        protected event EventHandler KeyPress;
        protected event EventHandler Up;
        protected event EventHandler Down;


        protected internal virtual void HandleMouseHover (object position) {
            if (Hover != null)
                Hover(position, EventArgs.Empty);
        }
        protected internal virtual void HandleKeyPress (object key) {
            if (IsSelected && KeyPress != null)
                KeyPress((int)key, EventArgs.Empty);
        }



        //void IControl.RaiseClick (object tag) {
        //    RaiseClick(tag);
        //}

        public abstract bool Contains (object position);
        public abstract bool Contains (float X, float Y);

        public virtual void RaiseMouseClick (object tag) {
            if (Click != null)
                Click(tag, EventArgs.Empty);
        }

        

        public void RaiseMouseMove (object tag) {
            if(Hover !=null)
                Hover(tag, EventArgs.Empty);
        }

        bool pressed = false;

        public void RaiseMouseUp (object tag) {
            if (pressed)
                RaiseMouseClick(tag);
            pressed = false;
            if (Up != null)
                Up(tag, EventArgs.Empty);
        }

        public void RaiseMouseDown (object tag) {
            pressed = true;
            if (Down != null)
                Down(tag, EventArgs.Empty);
        }
    }
}
