using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public interface IRenderableObject {
     
        event RenderObjectChangedEventHandler Changed;

        IEnumerable<Item> GetItems();
        IEnumerable<Geometry> GetPrimitives();
    }

    public delegate void RenderObjectChangedEventHandler(RenderObjectChangedEventArgs args);
    public class RenderObjectChangedEventArgs : EventArgs {
        IRenderableObject renderObject;
        public IRenderableObject RenderObject { get { return renderObject; } }
        public RenderObjectChangedEventArgs(IRenderableObject renderObject) {
            this.renderObject = renderObject;
        }
    }
}
