using System;
namespace BoulderDash.Models.World {
    public class Wall : Field {

        public override bool IsUnbreakable { get; set; }
        public override bool IsDestroyed { get; set; }

        public override bool IsOccupied() {
            return true;
        }

        public override string ToString() {

            if (IsDestroyed) {
                return " ";
            }

            else {
                return "│";    
            }

        }
    }
}
