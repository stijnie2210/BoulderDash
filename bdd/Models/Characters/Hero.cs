using System;
using BoulderDash.Models.Objects;
using BoulderDash.Models.World;

namespace BoulderDash.Models.Characters {
    public class Hero : Character {

        public override int BlastRadius { get; set; }
        public override bool IsAlive { get; set; }
        public override Field CurrentField { get; set; }

        public override void Move(Direction direction) {

            Field nextField;
            CurrentField.Next.TryGetValue(direction, out nextField);

            if(nextField.Object == null && nextField.Character == null && CanMoveTo(nextField)) {
                CleanMud(CurrentField);
                CurrentField.CanStand = true;
                CurrentField.Character = null;
                nextField.Character = this;
                nextField.CanStand = false;
                CurrentField = nextField;
            }

            else if(nextField.IsOccupied() && nextField.Object is Diamond) {
                CurrentField.Character = null;
                CurrentField.CanStand = true;
                CleanMud(CurrentField);
                CollectDiamond(nextField);

            }

            else if (nextField.Object is Boulder){
                CleanMud(CurrentField);
                TryPushBoulder(direction, nextField);
            }
        }

        private bool CanMoveTo(Field field) {
            

            return field.CanStand;
        }

        public void CollectDiamond(Field field) {

            if (field.Object is Diamond) {
                ((Diamond)field.Object).IsCollected = true;
                field.Object = null;
                field.Character = this;
                field.CanStand = false;
                CurrentField = field;
            }

        }

        private void CleanMud(Field field) {

            if (field is Mud && !((Mud)field).IsDestroyed) {
                ((Mud)field).IsDestroyed = true;
            }

        }

        public void TryPushBoulder(Direction direction, Field field) {

            Field nextField;
            field.Next.TryGetValue(direction, out nextField);

            if(direction == Direction.UP) { return; }

            if(!nextField.IsOccupied() && !nextField.IsUnbreakable && nextField.CanStand) {
                if(nextField is Mud) {
                    if(((Mud)nextField).IsDestroyed) {
                        nextField.Object = field.Object;
                        nextField.Object.CurrentField = nextField;
                        nextField.CanStand = false;
                        field.Object = null;
                        field.CanStand = true;      
                    }
                }

                else {
                    nextField.Object = field.Object;
                    nextField.Object.CurrentField = nextField;
                    nextField.CanStand = false;
                    field.Object = null;
                    field.CanStand = true; 
                }
            }
        }

        public override string ToString() {

            if (!IsAlive) {
                return "✝";
            }
                
            return "®";
        }

        public Field[] GetBlastZone() {

            Field fieldUp;
            Field fieldRight;
            Field fieldDown;
            Field fieldLeft;
            Field topLeft;
            Field topRight;
            Field bottomLeft;
            Field bottomRight;

            CurrentField.Next.TryGetValue(Direction.UP, out fieldUp);
            CurrentField.Next.TryGetValue(Direction.RIGHT, out fieldRight);
            CurrentField.Next.TryGetValue(Direction.DOWN, out fieldDown);
            CurrentField.Next.TryGetValue(Direction.LEFT, out fieldLeft);
            fieldUp.Next.TryGetValue(Direction.LEFT, out topLeft);
            fieldUp.Next.TryGetValue(Direction.RIGHT, out topRight);
            fieldDown.Next.TryGetValue(Direction.LEFT, out bottomLeft);
            fieldDown.Next.TryGetValue(Direction.RIGHT, out bottomRight);


            return new Field[] { topLeft, fieldUp, topRight, fieldLeft, CurrentField, fieldRight, bottomLeft, fieldDown, bottomRight };
        }

        public override void SelfDestruct() {

            Field[] blastZone = GetBlastZone();

            foreach (Field field in blastZone) {
                if (!field.IsUnbreakable) {
                    if (field.Object != null) {
                        field.CanStand = true;
                        field.IsDestroyed = true;
                        field.Object.IsDestroyed = true;
                        field.Object = null;
                    }

                    else {
                        field.CanStand = true;
                        field.IsDestroyed = true;
                    }
                }
            }

            IsAlive = false;
        }
    }
}
