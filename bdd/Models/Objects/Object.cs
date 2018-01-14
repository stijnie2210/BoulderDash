using System;
using BoulderDash.Models.World;

namespace BoulderDash.Models.Objects {
    public abstract class Object {

        public abstract bool IsFalling { get; set; }
        public abstract bool HasCollided { get; set; }
        public abstract bool IsDestroyed { get; set; }
        public abstract Field CurrentField { get; set; }
        public abstract void Fall();
        public abstract void Slide(Direction direction);
        public abstract bool CanFall();
        public abstract bool CanSlideLeft();
        public abstract bool CanSlideRight();

    }
}
