namespace GameCore {
    public class RenderObjectCore {
        public string ContentString { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }
        public Bounds MiniMapBounds { get; set; }

        internal RenderObjectCore(Bounds bounds,Bounds miniMapBounds, string content, float rotation) {
            ScreenBounds = bounds;
            MiniMapBounds = miniMapBounds;
            ContentString = content;
            Rotation = rotation;
        }
        public override string ToString() {
            return ContentString;
        }
    }
}
