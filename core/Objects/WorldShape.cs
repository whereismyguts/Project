using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameCore {
    public class WorldShape: Geometry {
         public List<Vector2> Points { get; } = new List<Vector2>();

        public WorldShape(Vector2 location, List<Vector2> vlist) : base(location, Vector2.One, false) {
            foreach(Vector2 v in vlist) {
                Points.Add( Viewport.World2ScreenPoint( v + location));
            }
        }
        
    }
}