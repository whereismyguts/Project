﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    //class VirtualObject: GameObject {
    //    #region implement
    //    public override Bounds ObjectBounds { get { throw new NotImplementedException(); } }
    //    protected internal override float Rotation { get { throw new NotImplementedException(); } }
    //    public override IEnumerable<Item> GetItems() { throw new NotImplementedException(); }
    //    #endregion
    //    public bool IsDead = false;
    //    public VirtualObject(float mass, Vector2 position, Vector2 velosity) {
    //        Location = position;
    //        Mass = mass;
    //        Velosity = velosity;
    //    }
    //    protected internal override void Step() {
    //        Velosity += PhysicsHelper.GetSummaryAttractingForce(CurrentSystem.Objects, this);
    //        foreach(SpaceBody b in CurrentSystem.Objects)
    //            if(Vector2.Distance(b.Location, Location) <= b.Radius)
    //                IsDead = true;
    //        base.Step();
    //    }

    //    protected override string GetName() {
    //        throw new NotImplementedException();
    //    }
    //}
    //public class TrajectoryCalculator {
    //    VirtualObject virtObj;
    //    GameObject realObj;
    //    public List<Vector2> Path { get; private set; } = new List<Vector2>();
    //    public TrajectoryCalculator(GameObject obj) {
    //        this.virtObj = new VirtualObject(obj.Mass, obj.Location, obj.Velosity);
    //        realObj = obj;
    //    }
    //    public List<Vector2> CalculateStep() {

    //        Path.Clear();
    //        virtObj.IsDead = false;
    //        Path.Add(virtObj.Location);
    //        for(int i = 0; i < 100; i++) {
    //            for(int j = 0; j < 20; j++) {
    //                virtObj.Step();
    //                if(virtObj.IsDead)
    //                    return Path;
    //            }
    //            Path.Add(virtObj.Location.Clone());
    //        }
    //        return Path;
    //    }
    //    internal void Update() {
    //        virtObj.Location = realObj.Location;
    //        virtObj.Velosity = realObj.Velosity;
    //    }
    //}
}
