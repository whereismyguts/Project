using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class GameCore {
        static GameCore instance;
        List<GameObject> objects;
        List<RenderObject> renderObjects;
        Random rnd = new Random();
        Character ship;

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
        public Character Ship {
            get {
                return ship;
            }
        }
        public Viewport Viewport { get; set; }

        GameCore() {
            World world = new World(new Vector2(0f, 9.82f));
            Body myBody = new Body(world, new Vector2(0, 0));

            myBody.BodyType = BodyType.Dynamic;
            CircleShape circleShape = new CircleShape(0.5f, 1f);


            Fixture fixture = myBody.CreateFixture(circleShape);


            Viewport = new Viewport(300, 300, 0, 0);
            LoadGameObjects();
        }

        float GetRandomT() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }
        void LoadGameObjects() {
            objects = new List<GameObject>();
            var bodies = new List<AttractingObject>();
            //TODO Data Driven Factory
            var sun = new AttractingObject(new CoordPoint(300, 300), 100, Viewport, "planet" + rnd.Next(1, 5));
            bodies.Add(new Planet(new CoordPoint(10, 10), 50, Viewport, GetRandomT(), sun, "planet" + rnd.Next(1, 5), true));
            bodies.Add(new Planet(new CoordPoint(10, 100), 40, Viewport, GetRandomT(), sun, "planet" + rnd.Next(1, 5), false));
            bodies.Add(new Planet(new CoordPoint(100, 10), 00, Viewport, GetRandomT(), sun, "planet" + rnd.Next(1, 5), true));
            bodies.Add(sun);
            ship = new Character(Viewport, bodies, new CoordPoint(0, 0));
            objects.Add(ship);
            objects.AddRange(bodies);

            for (var i = 0; i < 1000; i++)
                objects.Add(new Star(Viewport, GetCoord()));
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

        internal CoordPoint GetCoord() {
            return new CoordPoint(GetFloat(), GetFloat());
        }
        internal float GetFloat() {
            return (float)rnd.Next(-1000, 1000);
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
