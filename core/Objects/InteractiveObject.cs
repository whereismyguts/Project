using System;
using System.Linq;

namespace GameCore {
    public abstract class InteractiveObject: IControl {

        bool isHighlighted;
        bool isSelected;

        public virtual bool IsHighlighted {
            get { return isHighlighted; }
            set {
                if(isHighlighted == value)
                    return;
                isHighlighted = value;
            }
        }
        public virtual bool IsSelected {
            get { return isSelected; }
            set {
                if(isSelected == value)
                    return;
                isSelected = value;
            }
        }

        protected event EventHandler Click;
        protected event EventHandler Hover;
        protected event EventHandler KeyPress;

        protected virtual internal void DoClick(object tag) {
            if(Click != null)
                Click(tag, EventArgs.Empty);
        }
        protected internal virtual void HandleMouseHover(object position) {
            if(Hover != null)
                Hover(position, EventArgs.Empty);
        }
        protected internal virtual void HandleKeyPress(object key) {
            if(IsSelected && KeyPress != null)
                KeyPress((int)key, EventArgs.Empty);
        }

        public abstract bool Contains(object position);

        void IControl.DoClick(object tag) {
            DoClick(tag);
        }
    }
}
