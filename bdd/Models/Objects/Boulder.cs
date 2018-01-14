using System;
using BoulderDash.Models.Characters;
using BoulderDash.Models.World;

namespace BoulderDash.Models.Objects {
    public class Boulder : Object {
        
        public override bool IsFalling { get; set; }
        public override bool IsDestroyed { get; set; }
        public override bool HasCollided { get; set; }
        public override Field CurrentField { get; set; }


        public override void Fall() {

            IsFalling = true;

            Field nextField;
            CurrentField.Next.TryGetValue(Direction.DOWN, out nextField);

            CurrentField.Object = null;
            CurrentField.CanStand = true;
            nextField.Object = this;
            nextField.CanStand = false;
            CurrentField = nextField;
        }

        public override bool CanFall() {

            Field nextField;
            CurrentField.Next.TryGetValue(Direction.DOWN, out nextField);

            if(nextField.IsOccupied()) {
                if(IsFalling && nextField.Character != null) {
                    HasCollided = true;
                    return false;
                }
            }

            else {
                
                return true;

            }

            IsFalling = false;
            return false;

        }

        public override void Slide(Direction direction) {

            Field directionField;
            Field nextField;
            CurrentField.Next.TryGetValue(direction, out directionField);
            directionField.Next.TryGetValue(Direction.DOWN, out nextField);
            CurrentField.Object = null;
            CurrentField.CanStand = true;
            nextField.Object = this;
            nextField.CanStand = false;
            CurrentField = nextField;
            
        }

        public override bool CanSlideLeft() {

            Field leftField;
            Field holdingField;
            Field checkField;
            CurrentField.Next.TryGetValue(Direction.DOWN, out holdingField);
            CurrentField.Next.TryGetValue(Direction.LEFT, out leftField);
            leftField.Next.TryGetValue(Direction.DOWN, out checkField);

            if(holdingField.IsOccupied() && holdingField.Object != null && !IsFalling) {
                if (!leftField.IsOccupied()) {
                    if (!checkField.IsOccupied()) {
                        IsFalling = true;
                        return true;
                    }

                    return false;
                }   
            }

            return false;
        }

        public override bool CanSlideRight() {

            Field rightField;
            Field holdingField;
            Field checkField;
            CurrentField.Next.TryGetValue(Direction.DOWN, out holdingField);
            CurrentField.Next.TryGetValue(Direction.RIGHT, out rightField);
            rightField.Next.TryGetValue(Direction.DOWN, out checkField);

            if (holdingField.IsOccupied() && holdingField.Object != null && !IsFalling) {
                if (!rightField.IsOccupied()) {
                    if (!checkField.IsOccupied()) {
                        IsFalling = true;
                        return true;
                    }

                    return false;
                }
            }

            return false;

        }

        public override string ToString() {
            return "O";
        }
    }
}
