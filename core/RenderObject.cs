namespace Core {
    public class RenderObject {
        public string ContentString { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }

        internal RenderObject(Bounds bounds, string content, float rotation) {
            ScreenBounds = bounds;
            ContentString = content;
            Rotation = rotation;
        }
    }
}
