using System;
namespace BoulderDash.Models.World {
    public class Mud : Field {

        public override bool IsDestroyed { get; set; }
        public override bool IsUnbreakable { get; set; }

        public override bool IsOccupied() {
            if(this.Character != null) {
                return true;
            }

            if(this.Object == null) {
                if(this.IsDestroyed) {
                    return false;
                }

                return true;
            }

            return true;
        }

        public override string ToString() {

            if (IsDestroyed) {
                return " ";
            }

            else {
                return "▒";   
            }
        }
    }
}
