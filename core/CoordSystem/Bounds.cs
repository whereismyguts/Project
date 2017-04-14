using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameCore {
    public class Bounds {
        public Vector2 Center { get { return (LeftTop + RightBottom) / 2f; } }
        public float Height { get { return Math.Abs(RightBottom.Y - LeftTop.Y); } }
        public Vector2 Size { get { return new Vector2(Width, Height); } }
        public float Width { get { return Math.Abs(RightBottom.X - LeftTop.X); } }

        public Bounds() { }

        public Bounds(Vector2 lt, Vector2 rb) {
            LeftTop = lt;
            RightBottom = rb;
        }

        public Bounds(float x, float y, float w, float h) {
            LeftTop = new Vector2(x, y);
            RightBottom = new Vector2(x + w, y + h);
        }

        public static Bounds operator -(Bounds p1, Bounds p2) {
            return new Bounds(p1.LeftTop - p2.LeftTop, p1.RightBottom - p2.RightBottom);
        }
        public static Bounds operator -(Bounds p1, Vector2 p2) {
            return new Bounds(p1.LeftTop - p2, p1.RightBottom - p2);
        }
        public static Bounds operator *(Bounds p1, float k) {
            return new Bounds(p1.LeftTop * k, p1.RightBottom * k);
        }
        public static Bounds operator /(Bounds p1, float k) {
            return new Bounds(p1.LeftTop / k, p1.RightBottom / k);
        }
        public static Bounds operator +(Bounds p1, Bounds p2) {
            return new Bounds(p1.LeftTop + p2.LeftTop, p1.RightBottom + p2.RightBottom);
        }
        public static Bounds operator +(Bounds p1, Vector2 p2) {
            return new Bounds(p1.LeftTop + p2, p1.RightBottom + p2);
        }

        //public  bool isIntersect(Bounds obj) {
        //    return (Math.Abs(LeftTop.X - obj.LeftTop.X) * 2 < (Width + obj.Width)) &&
        //    (Math.Abs(LeftTop.Y - obj.LeftTop.Y) * 2 < (Height + obj.Height));
        //}
        public bool Contains(Vector2 p) {
            return this.LeftTop.X <= p.X && LeftTop.Y <= p.Y && RightBottom.X >= p.X && RightBottom.Y > p.Y;
        }
        public Vector2[] GetPoints() {
            return new Vector2[] {
                LeftTop,
                LeftTop + new Vector2(Width, 0),
                RightBottom,
                RightBottom + new Vector2(0, Height) };
        }
        public bool Intersect(Bounds bounds) {
            return Vector2.Distance(Center, bounds.Center) <= (Width + Height) / 4f + (bounds.Width + bounds.Height) / 4f;
        }
        public override string ToString() {
            return LeftTop + " : " + RightBottom;
        }
        public Vector2 LeftTop = new Vector2();
        public Vector2 RightBottom = new Vector2();

    }
}
