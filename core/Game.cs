using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public enum GameState { Menu, InGame}
    public class GameCore
    {
        GameState state = GameState.Menu;
        public GameState State { get { return state; } }
        Character Character { get; }
        public World World { get; } = new World(100, 100);
        public Viewport Viewport { get { return World.Viewport; } }
    }
}
