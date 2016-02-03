using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    enum GameState { Menu, InGame}
    public class Game
    {
        GameState State = GameState.Menu;
        Character Character { get; }
        public World World { get; } = new World(100, 100);
        public Viewport Viewport { get { return World.Viewport; } }
    }
}
