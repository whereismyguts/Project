using System;
using System.Linq;

namespace Core.Objects {
    public abstract class InteractiveObject {

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

        protected internal virtual void HandleMouseClick(object position) {
            if(Click != null)
                Click(position, EventArgs.Empty);
        }
        protected internal virtual void HandleMouseHover(object position) {
            if(Hover != null)
                Hover(position, EventArgs.Empty);
        }

        public abstract bool Contains(object position);
    }
}
