using System;
using System.Collections.Generic;
using BoulderDash.Models.Objects;
using BoulderDash.Models.World;

namespace BoulderDash.Models.Characters {
    public class Enemy : Character {

        public override int BlastRadius { get; set; }
        public override bool IsAlive { get; set; }
        public override Field CurrentField { get; set; }
        public Direction MoveDirection;

        public List<Direction> Directions;

        public override void Move(Direction direction) {

            if(IsAlive) {
                Field nextField;
                CurrentField.Next.TryGetValue(direction, out nextField);
                CurrentField.CanStand = true;
                CurrentField.Character = null;
                nextField.CanStand = false;
                nextField.Character = this;
                CurrentField = nextField;  
            }
        }

        public Direction CheckDirections() {

            SetDirections(MoveDirection);
            CheckSurroundings();

            foreach(Direction d in Directions) {
                Field nextField;
                CurrentField.Next.TryGetValue(d, out nextField);
                if (nextField.CanStand && nextField.Object == null) {
                    if (nextField is Mud) {
                        if (((Mud)nextField).IsDestroyed) {
                            MoveDirection = d;
                            return d;
                        }
                    }

                    else {
                        MoveDirection = d;
                        return d;
                    }
                }

                else if(nextField.Object != null) {

                    if(nextField.Object.IsFalling) {
                        IsAlive = false;
                        return d;   
                    }
                }
            }

            return Direction.INVALID;

        }

        public void SetDirections(Direction direction) {
            
            switch(direction) {
                case Direction.UP :
                    Directions = new List<Direction>() { Direction.LEFT, Direction.UP, Direction.RIGHT, Direction.DOWN };
                    break;
                case Direction.LEFT :
                    Directions = new List<Direction>() { Direction.DOWN, Direction.LEFT, Direction.UP, Direction.RIGHT };
                    break;
                case Direction.DOWN :
                    Directions = new List<Direction>() { Direction.RIGHT, Direction.DOWN, Direction.LEFT, Direction.UP };
                    break;
                case Direction.RIGHT :
                    Directions = new List<Direction>() { Direction.UP, Direction.RIGHT, Direction.DOWN, Direction.LEFT };
                    break;
            }
        }

        public override string ToString() {

            if(!IsAlive) {
                return " ";
            }

            return "ƒ";
        }

        public void CheckSurroundings() {

            foreach (Direction d in Directions) {

                Field nextField;
                CurrentField.Next.TryGetValue(d, out nextField);

                if (nextField.Character != null) {
                    if (nextField.Character is Hero) {
                        SelfDestruct();
                        nextField.Character.SelfDestruct();
                        return;
                    }
                }

            }

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

            foreach(Field field in blastZone) {
                if(!field.IsUnbreakable) {
                    if(field.Object != null) {
                        field.CanStand = true;
                        field.IsDestroyed = true;
                        field.Object.IsDestroyed = true;
                        field.Object = null;
                    }

                    else {
                        field.CanStand = true;
                        field.IsDestroyed = true;
                        field.Object = null;
                    }
                }
            }

            IsAlive = false;
        }
    }
}
