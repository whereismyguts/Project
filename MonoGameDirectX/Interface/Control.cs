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

        public virtual Color ActualBorderColor { get { return BorderColor; } }
        public virtual Color ActualFillColor { get { return FillColor; } }
        public Rectangle Rectangle { get; protected set; }

        public Control (Rectangle rect) {
            Rectangle = rect;
            FillColor = Color.Transparent;
            BorderColor = Color.Gray;
        }

        internal virtual void Draw (SpriteBatch spriteBatch, GameTime time) {
            DrawPrimitives.DrawRect(Rectangle, spriteBatch, 1, ActualBorderColor, ActualFillColor);
        }

        public override bool Contains (object position) {
            return Rectangle.Contains((Vector2)position);
        }

        public override bool Contains (float X, float Y) {
            return Rectangle.Contains(X, Y);
        }
    }
    public enum Align { Left, Center, Right, Top, Bottom }
    public class Label: Control {
        public StringBuilder Text { get; set; }
        public Color TextColor { get; internal set; }
        public Align TextAlign { get; set; } = Align.Center;
        public Label (int x, int y, int w, int h, string text) : base(new Rectangle(x, y, w, h)) {
            Text = new StringBuilder(text);
            TextColor = Color.Black;
        }

        protected virtual void TextDraw (SpriteBatch spriteBatch, string text) {
            Vector2 textSize = Renderer.Font.MeasureString(text);
            Vector2 panSize = Rectangle.Size.ToVector2();
            Vector2 alignVector = TextAlign == Align.Left ? new Vector2(5, (panSize.Y - textSize.Y) / 2) : (panSize - textSize) / 2;
            Vector2 textLocation = Rectangle.Location.ToVector2() + alignVector;// + (panSize - textSize) / 2;
            spriteBatch.DrawString(Renderer.Font, text, textLocation, TextColor);
        }

        internal override void Draw (SpriteBatch spriteBatch, GameTime time) {
            base.Draw(spriteBatch, time);
            TextDraw(spriteBatch, Text.ToString());
        }
    }
    public class TextBox: Label {
        int caret = 0;
        public TextBox (int x, int y, int w, int h, string text) : base(x, y, w, h, text) {
            TextAlign = Align.Left;
            KeyPress += TextBox_KeyPress;
            caret = text.Length;
            int c = 'a';
            for (int i = 65; i <= 90; i++) {
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


        protected override void TextDraw (SpriteBatch spriteBatch, string text) {

            if (IsSelected) {
                text = text.ToString().Insert(caret, "|");
            }
            base.TextDraw(spriteBatch, text.ToUpper());

        }

        private void TextBox_KeyPress (object key, EventArgs e) {
            int k = (int)key;
            switch (k) {
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
            if (k >= 65 && k <= 90) {
                AppendText(k);
                caret++;
            }
            if (caret > Text.Length)
                caret = Text.Length;
            if (caret < 0)
                caret = 0;
            System.Diagnostics.Debug.WriteLine(key);
        }



        Dictionary<int, char> letters = new Dictionary<int, char>();

        private void AppendText (int k) {
            Text.Insert(caret, letters[k]);
        }
        void RemoveCurrentSymbol () {
            if (caret > 0) {
                Text.Remove(caret - 1, 1);
                caret--;
            }
        }
        void DeleteCurrentSymbol () {
            if (caret < Text.Length) {
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
                return IsSelected ? Color.Black : BorderColor;
            }
        }
        public override Color ActualFillColor {
            get {
                return IsHighlighted ? Color.DarkGray : IsSelected ? Color.LightGray : FillColor;
            }
        }
        public object Tag { get; set; }
        public Button (int x, int y, int w, int h, string text) : base(x, y, w, h, text) {
        }

        public event EventHandler ButtonClick;

        public override void RaiseMouseClick (object tag) {
            ButtonClick?.Invoke(this, EventArgs.Empty);
            base.RaiseMouseClick(tag);
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
                if (!value)
                    for (int i = 0; i < buttons.Count; i++)
                        buttons[i].IsHighlighted = false;
                base.IsHighlighted = value;
            }
        }

        public ListBox (Point location, params object[] objects) : base(new Rectangle()) {

            this.location = location;
            Update(objects);
            this.Click += ListBox_Click;


        }

        public event EventHandler ItemClick;

        private void ListBox_Click (object position, EventArgs e) {
            for (int i = 0; i < buttons.Count; i++)
                if (buttons[i].Contains((Point)position))
                    if (ItemClick != null)
                        ItemClick(buttons[i], EventArgs.Empty);
        }

        internal void Update (object[] objects) {
            int w = 0;
            int h = 0;
            int hStep = 0;
            for (int i = 0; i < objects.Length; i++) {
                Vector2 size = Renderer.Font.MeasureString(objects[i].ToString());
                w = Math.Max((int)size.X + 10, w);
                hStep = Math.Max((int)size.Y + 10, hStep);
                h += (int)size.Y + 10;
                Rectangle = new Rectangle(location, new Point(w, h));
            }
            buttons.Clear();
            for (int i = 0; i < objects.Length; i++) {
                Button b = new Button(location.X, location.Y + hStep * i, w, hStep, objects[i].ToString()) { Tag = objects[i] };
                buttons.Add(b);
            }
        }

        internal override void Draw (SpriteBatch spriteBatch, GameTime time) {
            base.Draw(spriteBatch, time);
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].Draw(spriteBatch, time);
        }

        public override bool Contains (object position) {
            for (int i = 0; i < buttons.Count; i++)
                if (buttons[i].Contains((Point)position))
                    return true;
            return false;
        }
    }

    public class Slider: Control {

        int min = 0, max = 1500;
        int current = 50;
        int sliderWidth = 10;
        int sliderHeight = 20;
        bool pressed = false;
        int Current {
            get { return current; }
            set { current = Math.Min(Math.Max(min, value), max); }
        }

        public Slider (Rectangle rect, int min, int max) : base(rect) {
            Hover += Slider_Hover;
            Down += Slider_Down;
            Up += Slider_Up;
            sliderHeight = rect.Height;
            this.min = min;
            this.max = max;
        }


        private void Slider_Up (object sender, EventArgs e) {
            // color = Color.Green; 
            pressed = false;
        }

        private void Slider_Down (object sender, EventArgs e) {
            //     color = Color.Red;
            pressed = true;
        }

        Color color = Color.Red;


        private void Slider_Hover (object sender, EventArgs e) {
            //color = Color.Yellow;
            if (pressed) {
                MouseActionInfo info = (MouseActionInfo)sender;

                var value = (info.X - Rectangle.Left) / Rectangle.Width;

                var result = min +  (max - min) * value  ;

                Current = (int)Math.Ceiling( result);
            }
        }

        internal override void Draw (SpriteBatch spriteBatch, GameTime time) {
            
            var value = (Rectangle.Width - sliderWidth) * 1.0 / (max - min);
            int x = sliderWidth/2+ (int)(value * current - value*min    + Rectangle.X);
            int w = (int)(value * current-value * min);
            var rect = new Rectangle(x - sliderWidth / 2, Rectangle.Center.Y - sliderHeight / 2, sliderWidth, sliderHeight);
            var rect2 = new Rectangle(Rectangle.Left, Rectangle.Center.Y - sliderHeight / 2, w, sliderHeight);


            DrawPrimitives.DrawRect(rect2, spriteBatch, 1, Color.GreenYellow, Color.GreenYellow);
            base.Draw(spriteBatch, time);
            DrawPrimitives.DrawRect(rect, spriteBatch, 1, Color.Black, color);
            spriteBatch.DrawString(Renderer.Font, current.ToString(), Rectangle.Location.ToVector2(), Color.Black);

        }
    }

    //public class ImageBox: Control {
    //    public ImageBox(Rectangle rect) : base(rect) {
    //        sprite = new Sprite(new SpriteInfo(), rect);

    //    }
    //    Sprite sprite;
    //    public void SetImage(SpriteInfo info) {
    //        sprite = new Sprite(info, Rectangle);
    //    }
    //    internal override void Draw(SpriteBatch spriteBatch, GameTime time) {
    //        base.Draw(spriteBatch, time);
    //        sprite.Draw(spriteBatch, time, true);
    //    }
    //}
}
