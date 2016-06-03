using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class StationGenerator {
        Random rnd = new Random();
        const int a = 400;
        int[,] cells = new int[a, a];

        List<IntBounds> rooms = new List<IntBounds>();

        public int[,] Generate() {
            for(int i = 0; i < a; i++)
                for(int j = 0; j < a; j++)
                    cells[i, j] = 0;


            for(int i = 0; i < a * 10; i++) {
                IntBounds room = new IntBounds(rnd.Next(0, a), rnd.Next(0, a), rnd.Next(5, 30), rnd.Next(5, 30));
                if(room.LeftTop.X < 0 || room.LeftTop.Y < 0 || room.LeftTop.X >= a || room.LeftTop.Y >= a ||
                    room.RightBottom.X < 0 || room.RightBottom.Y < 0 || room.RightBottom.X >= a || room.RightBottom.Y >= a)
                    continue;
                if(!IsIntersectWithOtherRooms(room))
                    rooms.Add(room);
            }


            foreach(IntBounds room in rooms) {

                for(int i = room.LeftTop.X; i < room.RightBottom.X + 1; i++)
                    for(int j = room.LeftTop.Y; j < room.RightBottom.Y + 1; j++)
                        cells[i, j] = 2;

                for(int i = room.LeftTop.X; i < room.RightBottom.X + 1; i++) {
                    cells[i, room.LeftTop.Y] = 1;
                    cells[i, room.RightBottom.Y] = 1;
                }
                for(int j = room.LeftTop.Y; j < room.RightBottom.Y + 1; j++) {
                    cells[room.LeftTop.X, j] = 1;
                    cells[room.RightBottom.X, j] = 1;
                }
            }

            List<IntPoint> hdoors = new List<IntPoint>();
            List<IntPoint> vdoors = new List<IntPoint>();


            for(int i = 0; i < a; i++)
                for(int j = 0; j < a; j++)
                    if(cells[i, j] == 0)
                        if(i - 1 >= 0 && i + 1 < a && j - 1 >= 0 && j + 1 < a)
                            if(cells[i - 1, j - 1] == 1 && cells[i + 1, j + 1] == 1 && cells[i + 1, j - 1] == 1 && cells[i - 1, j + 1] == 1) {

                                if(cells[i - 1, j] == 1 && cells[i + 1, j] == 1)
                                    hdoors.Add(new IntPoint(i, j));

                                if(cells[i, j - 1] == 1 && cells[i, j + 1] == 1)
                                    vdoors.Add(new IntPoint(i, j));



                            }
            //cells[i, j] = 2;


            for(int i = 0; i < hdoors.Count / 5; i++) {
                int k = rnd.Next(0, hdoors.Count - 1);
                cells[hdoors[k].X, hdoors[k].Y - 1] = 1;
                cells[hdoors[k].X, hdoors[k].Y + 1] = 1;
                cells[hdoors[k].X - 1, hdoors[k].Y] = 2;
                cells[hdoors[k].X + 1, hdoors[k].Y] = 2;
                cells[hdoors[k].X, hdoors[k].Y] = 2;
                hdoors.RemoveAt(k);
            }
            for(int i = 0; i < vdoors.Count / 5; i++) {
                int k = rnd.Next(0, vdoors.Count - 1);

                cells[vdoors[k].X, vdoors[k].Y - 1] = 2;
                cells[vdoors[k].X, vdoors[k].Y + 1] = 2;
                cells[vdoors[k].X - 1, vdoors[k].Y] = 1;
                cells[vdoors[k].X + 1, vdoors[k].Y] = 1;
                cells[vdoors[k].X, vdoors[k].Y] = 2;
                vdoors.RemoveAt(k);
            }


            return cells;

        }

        private bool IsIntersectWithOtherRooms(IntBounds room) {
            foreach(var r in rooms)
                if(room.Intersect(r))
                    return true;
            return false;
        }
    }


    public class IntBounds {

        public int Height { get { return (int)Math.Abs(RightBottom.Y - LeftTop.Y); } }
        public IntPoint Size { get { return new IntPoint(Width, Height); } }
        public int Width { get { return (int)Math.Abs(RightBottom.X - LeftTop.X); } }

        public IntBounds(IntPoint lt, IntPoint rb) {
            LeftTop = lt;
            RightBottom = rb;
        }

        public IntBounds(int x, int y, int w, int h) {
            LeftTop = new IntPoint(x, y);
            RightBottom = new IntPoint(x + w, y + h);
        }



        public bool Contains(IntPoint p) {
            return this.LeftTop.X <= p.X && LeftTop.Y <= p.Y && RightBottom.X >= p.X && RightBottom.Y > p.Y;
        }
        //public IntPoint[] GetIntPoints() {
        //    return new IntPoint[] {
        //        LeftTop,
        //        LeftTop + new IntPoint(Width, 0),
        //        RightBottom,
        //        RightBottom + new IntPoint(0, Height) };
        //}
        public bool Intersect(IntBounds bounds) {

            return !(RightBottom.X < bounds.LeftTop.X || bounds.RightBottom.X < LeftTop.X || RightBottom.Y < bounds.LeftTop.Y || bounds.RightBottom.Y < LeftTop.Y);
            //var IntPoints = bounds.GetIntPoints();
            //for(int i = 0; i < IntPoints.Length; i++)
            //    if(bounds.Contains(IntPoints[i]))
            //        return true;
            //return false;
        }
        public override string ToString() {
            return LeftTop + " : " + RightBottom;
        }
        public IntPoint LeftTop;
        public IntPoint RightBottom;

    }

    public class IntPoint {


        public int X { get; set; }
        public int Y { get; set; }


        public static IntPoint operator +(IntPoint p1, IntPoint p2) {
            if(p2 == null)
                return p1;
            if(p1 == null)
                return p2;
            return new IntPoint(p1.X + p2.X, p1.Y + p2.Y);
        }


        public IntPoint(int x, int y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return string.Format("X:{0}, Y:{1}", X, Y);
        }
    }
}
