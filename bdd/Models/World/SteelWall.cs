using System;
namespace BoulderDash.Models.World {
    public class SteelWall : Field {

        public override bool IsUnbreakable { get; set; }
        public override bool IsDestroyed { get; set; }

        public override bool IsOccupied() {
            return true;
        }

        public override string ToString() {
            
            return "█";
        }
    }
}
