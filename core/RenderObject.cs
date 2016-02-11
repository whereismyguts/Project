namespace Core {
    public class RenderObject {
        public Bounds ScreenBounds { get; }
        public string ContentString { get; }
        public float Rotation { get; }
        internal RenderObject(Bounds bounds, string content, float rotation) {
            ScreenBounds = bounds;
            ContentString = content;
            Rotation = rotation;
        }
    }
}