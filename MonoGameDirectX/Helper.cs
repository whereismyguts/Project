using Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDirectX {
    public static class Helper {
        static int pxlTileSize;
        public static int TileSize
        {
            get
            {
                if(pxlTileSize != 0)
                    return pxlTileSize;
                throw new Exception("tile size is 0");
            }
        }
        public static void CalculateTileSize(Rectangle windowRect, Viewport viewport) {
            double pxlSizeX = windowRect.Width * 1.0 / viewport.Width;
            double pxlSizeY = windowRect.Height * 1.0 / viewport.Height;
            pxlTileSize = (int)Math.Min(pxlSizeX, pxlSizeY);
        }

        internal static Vector2 ToVector2(CoordPoint location) {
            return new Vector2(location.X, location.Y);
        }
    }
}
