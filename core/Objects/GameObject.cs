using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public abstract class GameObject: IRenderableObject {
        static IEnumerable<Item> itemsEmpty = new Item[] { };
        static IEnumerable<Geometry> geometryEmpty = new Geometry[] { };


        public event RenderObjectChangedEventHandler Changed;

        public Vector2 Velosity { get { return Circle.LinearVelocity; } }

        protected string Image { get; set; }
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
        protected internal float Rotation { get { return Circle.Rotation; } }

        public static StarSystem CurrentSystem { get { return MainCore.Instance.System; } }

        public bool ToRemove { get; set; } = false;



        public float Radius { get; }
        public virtual string Name { get { return string.Empty; } }
        public virtual Vector2 Location { get { return Circle.WorldCenter; } }
        public virtual Bounds ObjectBounds { get { return new Bounds(Location.Substract(Radius), Location.Add(Radius)); } }

        public float Mass {
            get { return Circle.Mass; }
        }

        public Body Circle { get; private set; }

        public World World { get; }

        public GameObject(World world, Vector2 location, float radius = 1) {
            MainCore.Instance.System.Add(this);
            Radius = radius;
            World = world;
            CreateCircle(radius, location);
        }

        public static Vector2 GetNewLocation(GameObject thisObject) {
            bool correctPosition = false;
            Vector2 location = Vector2.Zero;
            while(!correctPosition) {

                location = new Vector2(Rnd.Get(-300, 300), Rnd.Get(-300, 300));

                correctPosition = true;
                foreach(GameObject obj in CurrentSystem.Objects)
                    if(obj != thisObject && obj.ObjectBounds.Contains(location)) {
                        correctPosition = false;
                        break;
                    }
            }
            return location;
        }

        protected void CreateCircle(float radius, Vector2 location) {
            if(Circle != null && World.BodyList.Contains(Circle))
                World.RemoveBody(Circle);
            Circle = BodyFactory.CreateCircle(World, radius, 1f, location);

            Circle.BodyType = BodyType.Dynamic;

            Circle.OnCollision += Circle_OnCollision;
        }

        private bool Circle_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            return true;
        }

        protected void ApplyForce(Vector2 vector2) {
            Circle.ApplyForce(vector2);
        }

        protected internal virtual void Step() {
            //if(IsDynamic)
            //    Velosity = Velosity * 0.9999f + PhysicsHelper.GetSummaryAttractingForce(CurrentSystem.Objects, this) * 5;

            foreach(var obj in CurrentSystem.Objects)
                if(this != obj) {
                    var attraction = PhysicsHelper.GravitationForceVector(obj, this);

                    //   Circle.ApplyLinearImpulse(-attraction);
                    ApplyForce(attraction);
                    //  Circle.ApplyAngularImpulse(10);
                }

            //foreach(GameObject obj in MainCore.Instance.Objects)
            //    if(obj != this) {
            //        var nextPos = Location + Velosity;
            //        if(CoordPoint.Distance(obj.Location, nextPos) <= Radius + obj.Radius) {
            //            Location -= Velosity.UnaryVector;
            //            return;
            //        }
            //    }
            //if(MainCore.Instance.Objects.FirstOrDefault(ob => CoordPoint.Distance(ob.Location, Location) < Radius + ob.Radius && ob == this) != null)

            //   Location += Velosity;
        }
        protected abstract string GetName();

        public override string ToString() {
            return GetName();
        }
        /// <summary>foreach in all iternal items (weapons, effects, clouds, engines)</summary>
        public virtual IEnumerable<Item> GetItems() {
            return itemsEmpty;
        }
        public virtual IEnumerable<Geometry> GetPrimitives() {
            return geometryEmpty;
        }

        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(ObjectBounds);
        }
    }
}
