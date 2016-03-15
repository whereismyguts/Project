namespace Core {
    public class RenderObjectCore {
        public string ContentString { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }

        internal RenderObjectCore(Bounds bounds, string content, float rotation) {
            ScreenBounds = bounds;
            ContentString = content;
            Rotation = rotation;
        }
    }
}
