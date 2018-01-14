using System;
using BoulderDash.Models.Characters;

namespace BoulderDash.Models.World {
    public class Exit : Field {

        public bool IsHidden;
        public override bool IsUnbreakable { get; set; }
        public override bool IsDestroyed { get; set; }

        public override bool IsOccupied() {

            if(this.Character != null || this.Object != null) {
                return true;
            }

            return false;
        }

        public bool IsFinished() {
            if (this.Character is Hero && !this.IsHidden){
                return true;
            }

            return false;
        }

        public override string ToString() {

            if (IsHidden) {
                return " ";
            }

            else {
                return "E";   
            }
        }
    }
}
