using Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDirectX {
    public static class WinAdapter {
        public static Rectangle ToRectangle(Bounds bounds) {
            return new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
        }
    }
}
