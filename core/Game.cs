using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core {
    public enum GameState { Menu, InGame }
    public class GameCore {
        GameState state = GameState.Menu;
        Viewport viewport;
        List<GameObjectBase> objects;

        public GameState State { get { return state; } }
        public Character Character { get; }
        public List<GameObjectBase> Objects { get { return objects; } }
        //World World { get; } = new World(1000, 1000);
        public Viewport Viewport { get { return viewport; } }
        public GameCore() {
            SetViewport(new Viewport(0, 0));
        }
        public void SetViewport(Viewport viewport) {
            this.viewport = viewport;
            objects = LoadGameObjects();
        }
        Random rnd = new Random();
        List<GameObjectBase> LoadGameObjects() {
            objects = new List<GameObjectBase>();
            //TODO Data Driven Factory
            Sun sun = new Sun(new CoordPoint(300, 300), 100, viewport);
            objects.Add(new Planet(new CoordPoint(10, 10), 50, viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun));
            objects.Add(new Planet(new CoordPoint(10, 100), 40, viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun));
            objects.Add(new Planet(new CoordPoint(100, 10), 30, viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun));
            objects.Add(sun);

            return objects;
        }
    }
}
