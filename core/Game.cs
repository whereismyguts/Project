using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core {
    public enum GameState { Menu, InGame }
    public class GameCore {
        GameState state = GameState.Menu;
        Viewport viewport;
        Character character;
        List<AttractingObject> objects;

        public GameState State { get { return state; } }
        public Character Character { get { return character; } }
        public List<AttractingObject> Objects { get { return objects; } }
        //World World { get; } = new World(1000, 1000);
        public Viewport Viewport { get { return viewport; } }
        public GameCore() {
            SetViewport(new Viewport(0, 0));
            LoadGameObjects();
        }
        void SetViewport(Viewport viewport) {
            this.viewport = viewport;
        }
        Random rnd = new Random();
        void LoadGameObjects() {
            objects = new List<AttractingObject>();
            //TODO Data Driven Factory
            AttractingObject sun = new AttractingObject(new CoordPoint(300, 300), 100, viewport);
            objects.Add(new Planet(new CoordPoint(10, 10), 50, viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun));
            objects.Add(new Planet(new CoordPoint(10, 100), 40, viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun));
            objects.Add(new Planet(new CoordPoint(100, 10), 30, viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun));
            objects.Add(sun);
            character = new Character(viewport, objects, new CoordPoint(0,0));
        }
    }
}
