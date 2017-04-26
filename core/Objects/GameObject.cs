using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.TextureTools;
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

        public Vector2 Velosity { get { return Body.LinearVelocity; } }


        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
        protected internal virtual float Rotation { get { return Body.Rotation; } }

        public static StarSystem CurrentSystem { get { return MainCore.Instance.System; } }

        public Vector2 Direction { get { return new Vector2(0, -1).GetRotated(Body.Rotation); } }

        public bool ToRemove { get; set; } = false;



        public float Radius { get; }
        public virtual string Name { get { return string.Empty; } }
        public virtual Vector2 Location { get { return Body.WorldCenter; } }
        public virtual Bounds ObjectBounds { get { return new Bounds(Location.Substract(Radius), Location.Add(Radius)); } }

        public float Mass {
            get { return body != null ? body.Mass : 0; }
        }


        public Bounds ScreenBounds {
            get {
                return Viewport.World2ScreenBounds(ObjectBounds);
            }
        }

        Body body;

        protected virtual internal Body Body {
            get { return body; }
            set { body = value; }
        }

        public World World { get; }

        public GameObject(World world, Vector2 location, float radius = 1) {
            MainCore.Instance.System.Add(this);
            Radius = radius;
            World = world;
            CreateBody(radius, location);
            if(body != null) {
                Body.UserData = this;
            }
        }

        protected Geometry Body2PolygonShape(Body body) {
            var pol = (Body.FixtureList[0].Shape as PolygonShape);
            List<Vector2> res = new List<Vector2>();

            foreach(var v in pol.Vertices) {
                res.Add(v.GetRotated(Rotation));
            }
            return new WorldShape(Location + (-pol.MassData.Centroid).GetRotated(Rotation), res);
        }

        public static Vector2 GetNewLocation(GameObject thisObject) {
            bool correctPosition = false;
            Vector2 location = Vector2.Zero;
            while(!correctPosition) {
                location = new Vector2(Rnd.Get(-600, 600), Rnd.Get(-600, 600));
                correctPosition = true;
                foreach(GameObject obj in CurrentSystem.Objects(false))
                    if(obj != thisObject && obj.ObjectBounds.Contains(location)) {
                        correctPosition = false;
                        break;
                    }
            }
            return location;
        }

        protected virtual void CreateBody(float radius, Vector2 location) {

            //Vertices vertices;
            //vertices = TextureConverter.DetectVertices(texData, tex.Width);


            Body = BodyFactory.CreateCircle(World, radius, 1f, location, this);
            Body.BodyType = BodyType.Dynamic;
        }
        internal void ApplyLinearImpulse(Vector2 imp) {
            if(Body != null)
                Body.ApplyLinearImpulse(imp);
        }
        internal void ApplyForce(Vector2 vector2) {
            if(Body != null)
                Body.ApplyForce(vector2);
        }
        internal void ApplyForce(Vector2 vector2, Vector2 location) {
            if(Body != null)
                Body.ApplyForce(vector2, location);
        }

        protected internal virtual void Step() {
            //if(IsDynamic)
            //    Velosity = Velosity * 0.9999f + PhysicsHelper.GetSummaryAttractingForce(CurrentSystem.Objects, this) * 5;

            if(body == null)
                return;

            var attraction = Vector2.Zero;

            foreach(var obj in CurrentSystem.Objects(false))
                if(this != obj) {
                    attraction += PhysicsHelper.GravitationForceVector(obj, this);

                    //   Circle.ApplyLinearImpulse(-attraction);

                    //  Circle.ApplyAngularImpulse(10);
                }
            if(attraction.Length() != 0)
                ApplyForce(attraction);
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


            if(Body != null) {
                foreach(var fix in Body.FixtureList)
                    if(fix.Shape is PolygonShape)
                        yield return Body2PolygonShape(Body);
            }
            
                yield return new WorldGeometry(Location, new Vector2(Radius * 2, Radius * 2), true);
                yield return new Line(Location, Location + new Vector2(0, Radius).GetRotated(Rotation));
            
            if(MainCore.Instance.HookedObject == this)
                yield return new Line(Location, MainCore.Instance.Cursor);
        }

        protected internal virtual void GetDamage(int v) {
            // base: do nothing
        }
    }
    public static class CollideProcessing {
        public static bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            //    return true;
            ProjectileBase projectile = fixtureA.Body.UserData as ProjectileBase;
            //Ship ship = fixtureB.Body.UserData as Ship;
            GameObject obj = fixtureB.Body.UserData as GameObject;

            if(projectile != null && !projectile.ToRemove) {

                if(obj == projectile.Owner)
                    return false;

                var relativeVelosity = projectile.Velosity.Substract(obj.Velosity).Length();
                if(relativeVelosity <= projectile.Resistance) {
                    return true;
                }

                if(fixtureB.Body.UserData is ProjectileBase) {
                    (fixtureB.Body.UserData as ProjectileBase).ToRemove = true;
                }
                Vector2 point = projectile.Location + projectile.Body.ContactList.Contact.Manifold.Points[0].LocalPoint.GetRotated(projectile.Rotation);
                new Explosion(projectile.World, point);
                projectile.ToRemove = true;
                obj.GetDamage((int)(projectile.Damage * relativeVelosity / 200));
            }
            return true;
        }
    }
}
