using System;
using System.Collections.Generic;
using System.Linq;


namespace GameCore {

    public static class InteractionController {
        public static List<int> KeysUp { get; private set; } = new List<int>();
        public static List<int> KeysPressed { get; private set; } = new List<int>();

        public static void SetPressedKeys(IEnumerable<int> newKeys) {
            KeysUp.Clear();
            foreach(var key in newKeys)
                if(!KeysPressed.Contains(key))
                    KeysUp.Add(key);

            KeysPressed = newKeys.ToList();
        }
    }
}
