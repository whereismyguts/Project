using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core {
    public class GameCore {
        public Viewport Viewport { get; set; }
        List<GameObject> objects;
        List<RenderObject> renderObjects;
        static GameCore instance;
        Random rnd = new Random();
        public static GameCore Instance { get { if(instance == null) instance = new GameCore(); return instance; } }
        public List<RenderObject> RenderObjects { get { return renderObjects; } }
        GameCore() {
            SetViewport(new Viewport(300,300,0,0));
            LoadGameObjects();
        }
        void SetViewport(Viewport viewport) {
            this.Viewport = viewport;
        }
        
        void LoadGameObjects() {
            objects = new List<GameObject>();
            List<AttractingObject> bodies = new List<AttractingObject>();
            //TODO Data Driven Factory
            AttractingObject sun = new AttractingObject(new CoordPoint(300, 300), 100, Viewport, "planet" + rnd.Next(1, 5));
            bodies.Add(new Planet(new CoordPoint(10, 10), 50, Viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun, "planet" + rnd.Next(1, 5)));
            bodies.Add(new Planet(new CoordPoint(10, 100), 40, Viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun, "planet" + rnd.Next(1, 5)));
            bodies.Add(new Planet(new CoordPoint(100, 10), 30, Viewport, (float)(rnd.NextDouble() * Math.PI * 2), sun, "planet" + rnd.Next(1, 5)));
            bodies.Add(sun);

            objects.Add( new Character(Viewport, bodies, new CoordPoint(0, 0)));
            objects.AddRange(bodies);
        }

        public void Update() {
            MoveObjects();
            //Viewport.Centerpoint = objects.First().Location;
            //Viewport.SetScale(5f / (objects.First() as Character).Speed);
            //System.Diagnostics.Debug.WriteLine((objects.First() as Character).Speed.ToString());
            UpdateRenderObjects();
        }

        void UpdateRenderObjects() {
            renderObjects = new List<RenderObject>();
            foreach(GameObject obj in objects)
                renderObjects.Add(new RenderObject(obj.GetScreenBounds(), obj.ContentString, obj.GetRotation()));
        }

        void MoveObjects() {
            foreach(GameObject obj in objects)
                obj.Move();
        }
    }
}
