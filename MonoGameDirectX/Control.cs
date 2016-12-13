using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameCore;
using System.Text;

namespace MonoGameDirectX {
    public abstract class Control: InteractiveObject {
        protected Color BorderColor { get; }
        protected Color FillColor { get; }
        protected SpriteFont Font { get; }

        public virtual Color ActualBorderColor { get { return BorderColor; } }
        public virtual Color ActualFillColor { get { return FillColor; } }
        public Rectangle Rectangle { get; protected set; }

        public Control(Rectangle rect, SpriteFont font) {
            Rectangle = rect;
            FillColor = Color.Transparent;
            BorderColor = Color.Black;
            Font = font;
        }

        internal virtual void Draw(SpriteBatch spriteBatch, GameTime time) {
            DrawPrimitives.DrawRect(Rectangle, spriteBatch, 1, ActualBorderColor, ActualFillColor);
        }

        public override bool Contains(object position) {
            return Rectangle.Contains((Point)position);
        }
    }
    public class Label: Control {
        public StringBuilder Text { get; set; }
        public Color TextColor { get; internal set; }

        public Label(int x, int y, int w, int h, string text, SpriteFont font) : base(new Rectangle(x, y, w, h), font) {
            Text = new StringBuilder(text);
            TextColor = Color.Black;
        }

        protected virtual void TextDraw(SpriteBatch spriteBatch, string text) {
            Vector2 textSize = Font.MeasureString(text);
            Vector2 panSize = Rectangle.Size.ToVector2();
            Vector2 textLocation = Rectangle.Location.ToVector2() + (panSize - textSize) / 2;
            spriteBatch.DrawString(Font, text, textLocation, TextColor);
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime time) {
            base.Draw(spriteBatch, time);
            TextDraw(spriteBatch, Text.ToString());
        }
    }
    public class TextBox: Label {
        int caret = 0;
        public TextBox(int x, int y, int w, int h, string text, SpriteFont font) : base(x, y, w, h, text, font) {
            KeyPress += TextBox_KeyPress;


            caret = text.Length;

            int c = 'a';
            for(int i = 65; i <= 90; i++) {
                letters.Add(i, (char)c);
                c++;
            }
        }

        //A = 65,
        //B = 66,
        //C = 67,
        //D = 68,
        //E = 69,
        //F = 70,
        //G = 71,
        //H = 72,
        //I = 73,
        //J = 74,
        //K = 75,
        //L = 76,
        //M = 77,
        //N = 78,
        //O = 79,
        //P = 80,
        //Q = 81,
        //R = 82,
        //S = 83,
        //T = 84,
        //U = 85,
        //V = 86,
        //W = 87,
        //X = 88,
        //Y = 89,
        //Z = 90,


        protected override void TextDraw(SpriteBatch spriteBatch, string text) {

            if(IsSelected) {
                text = text.ToString().Insert(caret, "|");
            }
            base.TextDraw(spriteBatch, text.ToUpper());

        }

        private void TextBox_KeyPress(object key, EventArgs e) {
            int k = (int)key;
            switch(k) {
                case 39:
                    caret++;
                    break;
                case 37:
                    caret--;
                    break;
                case 8:
                    RemoveCurrentSymbol();
                    break;
                case 46:
                    DeleteCurrentSymbol();
                    break;

            }
            if(k >= 65 && k <= 90) {
                AppendText(k);
                caret++;
            }
            if(caret > Text.Length)
                caret = Text.Length;
            if(caret < 0)
                caret = 0;
            System.Diagnostics.Debug.WriteLine(key);
        }

     

        Dictionary<int, char> letters = new Dictionary<int, char>();

        private void AppendText(int k) {
            Text.Insert(caret, letters[k]);
        }
        void RemoveCurrentSymbol() {
            if(caret > 0) {
                Text.Remove(caret - 1, 1);
                caret--;
            }
        }
        void DeleteCurrentSymbol() {
            if(caret < Text.Length ) {
                Text.Remove(caret, 1);
            }
        }

        public override Color ActualBorderColor {
            get {
                return IsSelected ? Color.Red : IsHighlighted ? Color.Green : BorderColor;
            }
        }


    }

    public class Button: Label {
        public override Color ActualBorderColor {
            get {
                return IsSelected ? Color.AntiqueWhite : IsHighlighted ? Color.FloralWhite : BorderColor;
            }
        }
        public override Color ActualFillColor {
            get {
                return IsSelected ? Color.DarkGray : IsHighlighted ? Color.LightGray : FillColor;
            }
        }
        public object Tag { get; set; }
        public Button(int x, int y, int w, int h, string text, SpriteFont font) : base(x, y, w, h, text, font) {
        }

        public event EventHandler ButtonClick;

        protected override void HandleMouseClick(object position) {
            ButtonClick?.Invoke(this, EventArgs.Empty);
            base.HandleMouseClick(position);
        }
    }
    public class ListBox: Control {
        List<Button> buttons = new List<Button>();
        Point location;
        public override bool IsHighlighted {
            get {
                return base.IsHighlighted;
            }

            set {
                if(!value)
                    for(int i = 0; i < buttons.Count; i++)
                        buttons[i].IsHighlighted = false;
                base.IsHighlighted = value;
            }
        }

        public ListBox(Point location, SpriteFont font, params object[] objects) : base(new Rectangle(), font) {

            this.location = location;
            Update(objects);
            this.Click += ListBox_Click;


        }

        public event EventHandler ItemClick;

        private void ListBox_Click(object position, EventArgs e) {
            for(int i = 0; i < buttons.Count; i++)
                if(buttons[i].Contains((Point)position))
                    if(ItemClick != null)
                        ItemClick(buttons[i], EventArgs.Empty);
        }

        internal void Update(object[] objects) {
            int w = 0;
            int h = 0;
            int hStep = 0;
            for(int i = 0; i < objects.Length; i++) {
                Vector2 size = Font.MeasureString(objects[i].ToString());
                w = Math.Max((int)size.X + 10, w);
                hStep = Math.Max((int)size.Y + 10, hStep);
                h += (int)size.Y + 10;
                Rectangle = new Rectangle(location, new Point(w, h));
            }
            buttons.Clear();
            for(int i = 0; i < objects.Length; i++) {
                Button b = new Button(location.X, location.Y + hStep * i, w, hStep, objects[i].ToString(), Font) { Tag = objects[i] };
                buttons.Add(b);
            }
        }

        protected override void HandleMouseHover(object position) {
            for(int i = 0; i < buttons.Count; i++)
                buttons[i].IsHighlighted = buttons[i].Contains((Point)position) && IsHighlighted;

        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime time) {
            base.Draw(spriteBatch, time);
            for(int i = 0; i < buttons.Count; i++)
                buttons[i].Draw(spriteBatch, time);
        }

        public override bool Contains(object position) {
            for(int i = 0; i < buttons.Count; i++)
                if(buttons[i].Contains((Point)position))
                    return true;
            return false;
        }
    }
    public class ImageBox: Control {
        public ImageBox(Rectangle rect) : base(rect, null) {
            sprite = new Sprite(new SpriteInfo(), rect);

        }
        Sprite sprite;
        public void SetImage(SpriteInfo info) {
            sprite = new Sprite(info, Rectangle);
        }
        internal override void Draw(SpriteBatch spriteBatch, GameTime time) {
            base.Draw(spriteBatch, time);
            sprite.Draw(spriteBatch, time, true);
        }
    }
}
