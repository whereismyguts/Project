using Microsoft.Xna.Framework;
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
        void ActByKey (ActorKeyPair pair, bool clickedOnce);
        void ActByMouse (MouseActionInfo pair, bool clickedOnce);
    }
    public class MenuBehavior: ICommandsBehavior {

        public void ActByKey (ActorKeyPair pair, bool clickedOnce) {
            if (clickedOnce)
                switch (pair.Action) {
                    case PlayerAction.Down: MainCore.Instance.CurrentState.Select(true, pair.Actor); break;
                    case PlayerAction.Up: MainCore.Instance.CurrentState.Select(false, pair.Actor); break;
                    //case PlayerAction.Tab: MainCore.SwitchState(); break;
                    case PlayerAction.Yes: MainCore.Instance.CurrentState.DoSelected(pair.Actor); break;
                }
        }

        public void ActByMouse (MouseActionInfo eventInfo, bool clickedOnce) {

            //MainCore.Instance.CurrentState.DoMouseAction(eventInfo, true);

        }
    }
    //public class InventoryBehavior: ICommandsBehavior {
    //}
    public class GameBehavior: ICommandsBehavior {
        public void ActByKey (ActorKeyPair pair, bool clickedOnce) {

            PlayerController.ExecuteKey(pair, clickedOnce);
            return;
            //switch(pair.Action) {
            //    case PlayerAction.Down: MainCore.Instance.CurrentState.Select(true, pair.Actor); break;
            //    case PlayerAction.Up: MainCore.Instance.CurrentState.Select(false, pair.Actor); break;
            //    case PlayerAction.Tab: MainCore.SwitchState(); break;
            //    case PlayerAction.Yes: MainCore.Instance.CurrentState.DoSelected(pair.Actor); break;
            //}
        }

        public void ActByMouse (MouseActionInfo mouseInfo, bool clickedOnce) {
            PlayerController.ExecuteCursor(mouseInfo, clickedOnce);
        }
    }


    public enum UIStates {
        Menu = 0,
        Game = 1
    }

    public abstract class UIState {
        public abstract int Id { get; }

        public bool InGame { get { return StateId == UIStates.Game; } }
        protected ICommandsBehavior Behavior { get; set; }
        public abstract List<IControl> Controls { get; }
        public abstract UIStates StateId { get; }

        public void DoAction (ActorKeyPair pair, bool clickedOnce) {
            Behavior.ActByKey(pair, clickedOnce);
        }

        public abstract void AddControl (IControl control);

        //public abstract void SelectPrev(int actor);
        //public abstract void SelectNext(int actor);
        public abstract void Select (bool next, int actor);
        public abstract void DoSelected (int actor);

        public virtual void DoMouseAction (MouseActionInfo eventInfo) {
            foreach (IControl control in Controls) {

                // fire everytime, not just on the control:
                if (eventInfo.Action == MouseAction.Up)
                    control.RaiseMouseUp(eventInfo);
                if (eventInfo.Action == MouseAction.Move) {
                    control.RaiseMouseMove(eventInfo);
                    //Behavior.ActByMouse(eventInfo, true);
                }

                if (control.Contains(eventInfo.X, eventInfo.Y)) {
                    if (eventInfo.Action == MouseAction.Down) {
                        control.RaiseMouseDown(eventInfo);
                        Behavior.ActByMouse(eventInfo, true);
                    }
                }
            }
        }

        protected void SelectBase (List<IControl> list, bool next, ref int selectedIndex) {
            if (next) {
                if (selectedIndex < list.Count - 1)
                    selectedIndex++;
                else selectedIndex = 0;
            }
            else {
                if (selectedIndex > 0)
                    selectedIndex--;
                else selectedIndex = list.Count - 1;
            }
        }
    }

    public class MenuState: UIState {
        public MenuState () {
            Behavior = new MenuBehavior();
            menuControls = new List<IControl>(); // set menu items
        }
        
        List<IControl> menuControls = new List<IControl>();
        int selectedIndex = 0;

        public override int Id { get { return 0; } }

        public override List<IControl> Controls {
            get {

                return menuControls;
            }
        }

        public override UIStates StateId { get { return UIStates.Menu; } }

        public override void AddControl (IControl control) {
            menuControls.Add(control);
        }

        public override void Select (bool next, int actor) {
            // anybody can select menu items
            SelectBase(menuControls, next, ref selectedIndex);

            for (int i = 0; i < menuControls.Count; i++) {
                menuControls[i].IsSelected = selectedIndex == i;
            }
        }

        public override void DoSelected (int actor) {
            menuControls[selectedIndex].RaiseMouseUp(actor);
        }
    }

    public class GameState: UIState {
        public GameState () {
            Behavior = new GameBehavior();
        }

        public override List<IControl> Controls {
            get {
                return controls;
            }
        }
        List<IControl> controls = new List<IControl>();

        public override int Id { get { return 1; } }

        public override UIStates StateId { get { return UIStates.Game; } }

        public override void AddControl (IControl control) {
            controls.Add(control);
        }

        public override void Select (bool next, int actor) {
        }

        public override void DoSelected (int actor) {
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

    public struct MouseActionInfo {

        public float X { get; set; }
        public float Y { get; set; }
        public MouseAction Action { get; set; }

        public MouseActionInfo (float x, float y, MouseAction action) {
            X = x; Y = y; Action = action;
        }
    }

    public enum MouseAction {
        Up = 0,
        Down = 1,
        Move = 2
    }

    public struct ActorKeyPair {
        //        public int Key { get; set; }
        public int Actor { get; set; }
        public PlayerAction Action { get; set; }

        public ActorKeyPair (int actor, PlayerAction action) {
            Actor = actor; Action = action;
        }
    }

    public interface IControl {
        bool IsSelected { get; set; }
        PlayerAction Tag { get; set; }

        bool Contains (object position);
        bool Contains (float X, float Y);
        void RaiseMouseUp (object tag);
        void RaiseMouseDown (object tag);
        void RaiseMouseMove (object tag);

    }

    //public class InventoryItemControl: IControl {
    //    Item item;

    //    public InventoryItemControl (Item item) {
    //        this.item = item;
    //    }

    //    public bool IsSelected { get; set; }

    //    public bool Contains (object position) {
    //        throw new NotImplementedException();
    //    }

    //    public void DoClick (object tag) {
    //        throw new NotImplementedException();
    //    }
    //}
}
