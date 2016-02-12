using Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDirectX {
    public static class WinAdapter {
        internal static Rectangle ToRectangle(Bounds bounds) {
            return new Rectangle((int)bounds.LeftTop.X, (int)bounds.LeftTop.Y, (int)bounds.Width, (int)bounds.Height);
        }
        internal static Vector2 ToVector2(Bounds bounds) {
            return new Vector2(bounds.LeftTop.X, bounds.LeftTop.Y);
        }
    }
}
