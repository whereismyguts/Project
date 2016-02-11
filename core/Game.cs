using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class GameCore {
        static GameCore instance;
        List<GameObject> objects;
        List<RenderObject> renderObjects;
        Random rnd = new Random();

        GameCore() {
            Viewport = new Viewport(300, 300, 0, 0);
            LoadGameObjects();
        }

        public static GameCore Instance {
            get {
                if (instance == null)
                    instance = new GameCore();
                return instance;
            }
        }
        public List<RenderObject> RenderObjects {
            get {
                return renderObjects;
            }
        }
        public Viewport Viewport { get; set; }

        float GetRandomT() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }
        void LoadGameObjects() {
            objects = new List<GameObject>();
            var bodies = new List<AttractingObject>();
            //TODO Data Driven Factory
            //TODO Data Driven Factory
            var sun = new AttractingObject(new CoordPoint(300, 300), 100, Viewport, "planet" + rnd.Next(1, 5));
            bodies.Add(new Planet(new CoordPoint(10, 10), 50, Viewport, GetRandomT(), sun, "planet" + rnd.Next(1, 5), true));
            bodies.Add(new Planet(new CoordPoint(10, 100), 40, Viewport, GetRandomT(), sun, "planet" + rnd.Next(1, 5), false));
            var planet_with_moon = new Planet(new CoordPoint(100, 10), 30, Viewport, GetRandomT(), sun, "planet" + rnd.Next(1, 5), true);
            var moon = new Planet(planet_with_moon.Location + new CoordPoint(20, 20), 10, Viewport, GetRandomT(), planet_with_moon, "planet3", false);
            bodies.Add(planet_with_moon);
            bodies.Add(moon);
            bodies.Add(sun);

            objects.Add(new Character(Viewport, bodies, new CoordPoint(0, 0)));
            objects.AddRange(bodies);
        }
        void MoveObjects() {
            foreach (GameObject obj in objects)
                obj.Move();
        }
        void UpdateRenderObjects() {
            renderObjects = new List<RenderObject>();
            foreach (GameObject obj in objects)
                renderObjects.Add(new RenderObject(obj.GetScreenBounds(), obj.ContentString, obj.GetRotation()));
        }

        public void Update() {
            MoveObjects();
            Viewport.Centerpoint = objects.First().Location;
            //Viewport.SetScale(5f / (objects.First() as Character).Speed);
            //System.Diagnostics.Debug.WriteLine((objects.First() as Character).Speed.ToString());
            UpdateRenderObjects();
        }
    }
}
