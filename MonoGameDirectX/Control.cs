using System;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Core.Objects;

namespace MonoGameDirectX {
    public class Control: InteractiveObject {
        protected Color BorderColor { get; }
        protected Color FillColor { get; }
        public virtual Color ActualBorderColor { get { return BorderColor; } }
        public virtual Color ActualFillColor { get { return FillColor; } }
        public Rectangle Rectangle { get; }

        public Control(Rectangle rect) {
            Rectangle = rect;
            FillColor = Color.Transparent;
            BorderColor = Color.Black;
        }

        public override bool Contains(object position) {
            return Rectangle.Contains((Point)position);
        }
    }
    public class Label: Control {
        public string Text { get; set; }
        public Color TextColor { get; internal set; }

        public Label(int x, int y, int w, int h, string text) : base(new Rectangle(x, y, w, h)) {
            Text = text;
            TextColor = Color.Black;
        }
    }
    public class Button: Label {
        public Button(int x, int y, int w, int h, string text) : base(x, y, w, h, text) {
        }

        public event EventHandler ButtonClick;
        protected override void HandleMouseClick() {
            ButtonClick?.Invoke(this, EventArgs.Empty);
            base.HandleMouseClick();
        }
        public override Color ActualFillColor {
            get {
                return IsSelected ? Color.DarkGray : IsHighlighted ? Color.LightGray : FillColor;
            }
        }
        public override Color ActualBorderColor {
            get {
                return IsSelected ? Color.AntiqueWhite : IsHighlighted ? Color.FloralWhite : BorderColor;
            }
        }

    }
}
