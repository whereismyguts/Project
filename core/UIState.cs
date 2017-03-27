using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public abstract class UIState {
        public virtual bool InGame { get { return true; } }
        protected ICommandsBehavior Behavior { get; set; }

        public void DoAction(ActorKeyPair pair) {
            switch(pair.Action) {
                case PlayerAction.Yes: break;
                case PlayerAction.Up: Behavior.Up(pair.Actor); break;
                case PlayerAction.Left: break;
                case PlayerAction.Right: break;
                case PlayerAction.Tab:
                    //InterfaceController.Switch(new GameState());
                    break;
            }
        }
        public abstract void SelectPrev(int actor);
    }

    public class MenuState: UIState {
        public MenuState() {
            Behavior = new MenuBehavior();
            menuControls = new List<CoreControl>(); // set menu items
        }

        List<CoreControl> menuControls = new List<CoreControl>();
        int selectedIndex = 0;





        public override void SelectPrev(int actor) {
            // anybody can select menu items
            if(selectedIndex > 0)
                selectedIndex--;
            else selectedIndex = menuControls.Count - 1;
        }
    }

    public class GameState: UIState {
        public GameState() {
            Behavior = new GameBehavior();
        }

        public override void SelectPrev(int actor) {
            // do nothing, but it may be a target selection
        }
    }

    public class InventoryState: UIState {
        public InventoryState() {
            Behavior = new InventoryBehavior();
            inventoryControls = new Dictionary<int, List<CoreControl>>();// set controls in inventory; must be a link to updatable inventory
            selectedControlsIndexes = new Dictionary<int, int>();
        }

        Dictionary<int, List<CoreControl>> inventoryControls;
        Dictionary<int, int> selectedControlsIndexes;



        public override void SelectPrev(int actor) {
            if(selectedControlsIndexes[actor] > 0)
                selectedControlsIndexes[actor]--;
            else selectedControlsIndexes[actor] = inventoryControls[actor].Count - 1;
        }
    }

    public class CoreControl {
        //todo internal impl of ui controls
    }
}
