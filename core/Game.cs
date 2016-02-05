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
        World World { get; } = new World(1000, 1000);
        public Viewport Viewport { get { return World.Viewport; } }

        public void SetViewport(Viewport viewport) {
            World.Viewport = viewport;
        }
    }
}
