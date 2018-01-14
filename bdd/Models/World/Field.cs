using System;
using System.Collections.Generic;
using BoulderDash.Models.Characters;

namespace BoulderDash.Models.World {

    public abstract class Field {


        public Dictionary<Direction, Field> Next;
        public abstract bool IsUnbreakable { get; set; }
        public abstract bool IsDestroyed { get; set; }
        public bool CanStand { get; set; }
        public Character Character { get; set; } 
        public Objects.Object Object { get; set; }
        public abstract bool IsOccupied();
        public abstract override string ToString();

    }
}
