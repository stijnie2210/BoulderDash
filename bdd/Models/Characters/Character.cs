using System;
using BoulderDash.Models.World;

namespace BoulderDash.Models.Characters {
    public abstract class Character {

        public abstract void Move(Direction direction);
        public abstract void SelfDestruct();
        public abstract bool IsAlive { get; set; }
        public abstract int BlastRadius { get; set; }
        public abstract Field CurrentField { get; set; }
    }
}
