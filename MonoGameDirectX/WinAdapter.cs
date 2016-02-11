using Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDirectX {
    public static class WinAdapter {
        public static Rectangle ToRectangle(Bounds bounds) {
            return new Rectangle((int)bounds.LeftTop.X, (int)bounds.LeftTop.Y, (int)bounds.Width, (int)bounds.Height);
        }
    }
}
