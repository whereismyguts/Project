using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public enum PlayerAction { Up, Down, Left, Right, Yes, Tab, None };

    public interface ICommandsBehavior {
        //void Up(int actor);
        //void Tab(int actor);
        void Act(ActorKeyPair pair);
    }
    public class MenuBehavior: ICommandsBehavior {


        TimeSpan lastPressed = new TimeSpan();
        public void Act(ActorKeyPair pair) {

            TimeSpan now = DateTime.Now.TimeOfDay;
            if((now - lastPressed).TotalMilliseconds < 100)
                return;
            lastPressed = now;

            switch(pair.Action) {
                case PlayerAction.Down: MainCore.Instance.CurrentState.Select(true, pair.Actor); break;
                case PlayerAction.Up: MainCore.Instance.CurrentState.Select(false, pair.Actor); break;
                case PlayerAction.Tab: MainCore.SwitchState(); break;
                case PlayerAction.Yes: MainCore.Instance.CurrentState.DoSelected(pair.Actor); break;
            }
        }


    }
    //public class InventoryBehavior: ICommandsBehavior {
    //}
    public class GameBehavior: ICommandsBehavior {
        public void Act(ActorKeyPair pair) {

            PlayerController.Execute(pair);
            return;
            switch(pair.Action) {
                case PlayerAction.Down: MainCore.Instance.CurrentState.Select(true, pair.Actor); break;
                case PlayerAction.Up: MainCore.Instance.CurrentState.Select(false, pair.Actor); break;
                case PlayerAction.Tab: MainCore.SwitchState(); break;
                case PlayerAction.Yes: MainCore.Instance.CurrentState.DoSelected(pair.Actor); break;
            }
        }
    }


    public abstract class UIState {
        public abstract int Id { get; }

        public virtual bool InGame { get { return true; } }
        protected ICommandsBehavior Behavior { get; set; }
        public abstract List<IControl> Controls { get; }

        public void DoAction(ActorKeyPair pair) {
            Behavior.Act(pair);
        }

        public abstract void AddControl(IControl control, int actor);

        //public abstract void SelectPrev(int actor);
        //public abstract void SelectNext(int actor);
        public abstract void Select(bool next, int actor);
        public abstract void DoSelected(int actor);

        protected void SelectBase(List<IControl> list, bool next, ref int selectedIndex) {
            if(next) {
                if(selectedIndex < list.Count - 1)
                    selectedIndex++;
                else selectedIndex = 0;
            }
            else {
                if(selectedIndex > 0)
                    selectedIndex--;
                else selectedIndex = list.Count - 1;
            }
        }
    }

    public class MenuState: UIState {
        public MenuState() {
            Behavior = new MenuBehavior();
            menuControls = new List<IControl>(); // set menu items
        }
        public override bool InGame {
            get {
                return false;
            }
        }
        List<IControl> menuControls = new List<IControl>();
        int selectedIndex = 0;

        public override int Id { get { return 0; } }

        public override List<IControl> Controls {
            get {
                for(int i = 0; i < menuControls.Count; i++) {
                    menuControls[i].IsSelected = selectedIndex == i;
                }
                return menuControls;
            }
        }


        public override void AddControl(IControl control, int actor) {
            menuControls.Add(control);
        }

        public override void Select(bool next, int actor) {
            // anybody can select menu items
            SelectBase(menuControls, next, ref selectedIndex);
        }



        public override void DoSelected(int actor) {
            menuControls[selectedIndex].DoClick(actor);
        }
    }

    public class GameState: UIState {
        public GameState() {
            Behavior = new GameBehavior();
        }

        public override List<IControl> Controls {
            get {
                return p1Controls.Union(p2Controls).Union(commonControls).ToList();
            }
        }

        List<IControl> p1Controls = new List<IControl>();
        List<IControl> p2Controls = new List<IControl>();
        List<IControl> commonControls = new List<IControl>();
        int p1SelectIndex = 0;
        int p2SelectIndex = 0;
        int commonSelectIndex = 0;

        public override int Id { get { return 1; } }

        public override void AddControl(IControl control, int actor) {
            switch(actor) {
                case 1:
                    p1Controls.Add(control);
                    break;
                case 2:
                    p2Controls.Add(control);
                    break;
                default:
                    commonControls.Add(control);
                    break;
            }
        }

        public override void DoSelected(int actor) {
            switch(actor) {
                case 1:
                    p1Controls[p1SelectIndex].DoClick(actor);
                    break;
                case 2:
                    p2Controls[p2SelectIndex].DoClick(actor);
                    break;
                default:
                    commonControls[commonSelectIndex].DoClick(actor);
                    break;
            }
        }

        public override void Select(bool next, int actor) {
            switch(actor) {
                case 1:
                    SelectBase(p1Controls, next, ref p1SelectIndex);
                    break;
                case 2:
                    SelectBase(p2Controls, next, ref p2SelectIndex);
                    break;
                default:
                    SelectBase(commonControls, next, ref commonSelectIndex);
                    break;
            }
        }
    }

    //public class InventoryState: UIState {
    //    public InventoryState() {
    //        Behavior = new InventoryBehavior();
    //        inventoryControls = new Dictionary<int, List<CoreControl>>();// set controls in inventory; must be a link to updatable inventory
    //        selectedControlsIndexes = new Dictionary<int, int>();
    //    }

    //    Dictionary<int, List<CoreControl>> inventoryControls;
    //    Dictionary<int, int> selectedControlsIndexes;



    //    public override void SelectPrev(int actor) {
    //        if(selectedControlsIndexes[actor] > 0)
    //            selectedControlsIndexes[actor]--;
    //        else selectedControlsIndexes[actor] = inventoryControls[actor].Count - 1;
    //    }
    //}

    public struct ActorKeyPair {
        //        public int Key { get; set; }
        public int Actor { get; set; }
        public PlayerAction Action { get; set; }

        public ActorKeyPair(int actor, PlayerAction action) {
            Actor = actor; Action = action;
        }
    }

    public interface IControl {
        bool IsSelected { get; set; }
        void DoClick(object tag);
    }

    public class InventoryItemControl: IControl {
        Item item;

        public InventoryItemControl(Item item) {
            this.item = item;
        }

        public bool IsSelected { get; set; }

        public void DoClick(object tag) {

        }
    }
}
