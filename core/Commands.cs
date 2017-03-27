using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {

    public interface ICommandsBehavior {
        void Up(int actor);
    }

    public class MenuBehavior: ICommandsBehavior {
        public void Up(int actor) {
            InterfaceController.SelectPrev(actor);
        }
    }
    public class InventoryBehavior: ICommandsBehavior {
        public void Up(int actor) {
            InterfaceController.SelectPrev(actor);
        }
    }
    public class GameBehavior: ICommandsBehavior {
        public void Up(int actor) {
            MainCore.Instance.Ships[actor].Accselerate();
        }
    }


    public enum PlayerAction { Up, Down, Left, Right, Yes, Tab };
}
