using System;
namespace BoulderDash.Models.World {
    public class Void : Field {

        public override bool IsUnbreakable { get; set; }
        public override bool IsDestroyed { get; set; }

        public override bool IsOccupied() {
            if (this.Character != null || this.Object != null) {
                return true;
            }

            return false;
        }

        public override string ToString() {
            return " ";
        }
    }
}
