using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public static class Debugger {

        public static event EventHandler LineAdded;

        public static void AddLine(string line) {
            Lines.Add(line);
            if(LineAdded != null)
                LineAdded(Lines, EventArgs.Empty);
        }

        static List<string> Lines = new List<string>() { "Start..." };

        public static int LinesCount { get { return Lines.Count; } }

        public static string GetLine(int line) {
            return Lines[line];
        }
    }
}
