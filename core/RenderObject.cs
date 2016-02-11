namespace Core {
    public class RenderObject {
        internal RenderObject(Bounds bounds, string content, float rotation) {
            ScreenBounds = bounds;
            ContentString = content;
            Rotation = rotation;
        }

        public string ContentString { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }
    }
}
