using System;
using System.Collections.Generic;
using BoulderDash.Models.Characters;
using BoulderDash.Models.Objects;

namespace BoulderDash.Models.World {
    public class Level {

        private string[] _level;
        public bool IsCompleted { get; set; }
        public Level Next { get; set; }
        public int TimeLimit { get; set; }
        public Field[,] Fields { get; set; }
        public List<Objects.Object> Objects = new List<Objects.Object>();
        public List<Enemy> Enemies = new List<Enemy>();
        public Hero Hero { get; set; }

        public Level(string level, int timeLimit, Level next) {
            this._level = new LevelReader().ReadLevel(level);
            this.TimeLimit = timeLimit;
            this.Next = next;
            this.IsCompleted = false;
            CreateLevel();
            LinkFields();
        }

        public void CreateLevel() {
            
            for (int y = 0; y < _level.Length; y++) {
                for (int x = 0; x < _level[y].Length; x++) {

                    if(Fields == null) {
                        Fields = new Field[_level[y].Length, _level.Length];
                    }

                    switch (_level[y].Substring(x, 1)) {
                        case "S":
                            Fields[x, y] = new SteelWall { CanStand = false, IsUnbreakable = true, Character = null };
                            break;
                        case " ":
                            Fields[x, y] = new Void { CanStand = true, IsUnbreakable = false, Character = null };
                            break;
                        case "W":
                            Fields[x, y] = new Wall { CanStand = false, IsUnbreakable = false, Character = null };
                            break;
                        case "M":
                            Fields[x, y] = new Mud { CanStand = true, IsUnbreakable = false, Character = null };
                            break;
                        case "E":
                            Fields[x, y] = new Exit { IsHidden = true, IsUnbreakable = true, CanStand = true };
                            break;
                        case "R":
                            Field heroField = new Void { CanStand = false };
                            Hero hero = new Hero { BlastRadius = 1, CurrentField = heroField, IsAlive = true };
                            heroField.Character = hero;
                            Hero = hero;
                            Fields[x, y] = heroField;
                            break;
                        case "F":
                            Field enemyField = new Void { CanStand = false };
                            Enemy enemy = new Enemy { BlastRadius = 1, MoveDirection = Direction.RIGHT, CurrentField = enemyField, IsAlive = true };
                            enemyField.Character = enemy;
                            Fields[x, y] = enemyField;
                            Enemies.Add(enemy);
                            break;
                        case "B":
                            Field boulderField = new Void { CanStand = false, Character = null };
                            Boulder boulder = new Boulder { CurrentField = boulderField, IsFalling = false };
                            boulderField.Object = boulder;
                            Fields[x, y] = boulderField;
                            Objects.Add(boulder);
                            break;
                        case "D":
                            Field diamondField = new Void { CanStand = true, Character = null };
                            Diamond diamond = new Diamond { CurrentField = diamondField, IsFalling = false, IsCollected = false };
                            diamondField.Object = diamond;
                            Fields[x, y] = diamondField;
                            Objects.Add(diamond);
                            break;
                    }
                }
            }
        }

        public void LinkFields() {
            for (int y = 0; y < _level.Length; y++) {
                for (int x = 0; x < _level[y].Length; x++) {

                    Direction direction = InvalidDirection(x, y);


                    if(IsCorner(x, y)) {
                        if (x == 0 && y == 0) {
                            Fields[x, y].Next = new Dictionary<Direction, Field>();
                            Fields[x, y].Next.Add(Direction.UP, null);
                            Fields[x, y].Next.Add(Direction.RIGHT, Fields[x + 1, y]);
                            Fields[x, y].Next.Add(Direction.DOWN, Fields[x, y + 1]);
                            Fields[x, y].Next.Add(Direction.LEFT, null);
                        }

                        if (x == (_level[y].Length - 1) && y == 0) {
                            Fields[x, y].Next = new Dictionary<Direction, Field>();
                            Fields[x, y].Next.Add(Direction.UP, null);
                            Fields[x, y].Next.Add(Direction.RIGHT, null);
                            Fields[x, y].Next.Add(Direction.DOWN, Fields[x, y + 1]);
                            Fields[x, y].Next.Add(Direction.LEFT, Fields[x - 1, y]);
                        }

                        if (x == 0 && y == (_level.Length - 1)) {
                            Fields[x, y].Next = new Dictionary<Direction, Field>();
                            Fields[x, y].Next.Add(Direction.UP, Fields[x, y - 1]);
                            Fields[x, y].Next.Add(Direction.RIGHT, Fields[x + 1, y]);
                            Fields[x, y].Next.Add(Direction.DOWN, null);
                            Fields[x, y].Next.Add(Direction.LEFT, null);
                        }

                        if (x == (_level[y].Length - 1) && y == (_level.Length - 1)) {
                            Fields[x, y].Next = new Dictionary<Direction, Field>();
                            Fields[x, y].Next.Add(Direction.UP, Fields[x, y - 1]);
                            Fields[x, y].Next.Add(Direction.RIGHT, null);
                            Fields[x, y].Next.Add(Direction.DOWN, null);
                            Fields[x, y].Next.Add(Direction.LEFT, Fields[x - 1, y]);
                        }
                    }

                    else if(direction != Direction.INVALID) {
                        switch(direction) {
                            case Direction.UP : 
                                Fields[x, y].Next = new Dictionary<Direction, Field>();
                                Fields[x, y].Next.Add(Direction.UP, null);
                                Fields[x, y].Next.Add(Direction.RIGHT, Fields[x + 1, y]);
                                Fields[x, y].Next.Add(Direction.DOWN, Fields[x, y + 1]);
                                Fields[x, y].Next.Add(Direction.LEFT, Fields[x - 1, y]);
                                break;

                            case Direction.RIGHT:
                                Fields[x, y].Next = new Dictionary<Direction, Field>();
                                Fields[x, y].Next.Add(Direction.UP, Fields[x, y - 1]);
                                Fields[x, y].Next.Add(Direction.RIGHT, null);
                                Fields[x, y].Next.Add(Direction.DOWN, Fields[x, y + 1]);
                                Fields[x, y].Next.Add(Direction.LEFT, Fields[x - 1, y]);
                                break;

                            case Direction.DOWN:
                                Fields[x, y].Next = new Dictionary<Direction, Field>();
                                Fields[x, y].Next.Add(Direction.UP, Fields[x, y - 1]);
                                Fields[x, y].Next.Add(Direction.RIGHT, Fields[x + 1, y]);
                                Fields[x, y].Next.Add(Direction.DOWN, null);
                                Fields[x, y].Next.Add(Direction.LEFT, Fields[x - 1, y]);
                                break;

                            case Direction.LEFT:
                                Fields[x, y].Next = new Dictionary<Direction, Field>();
                                Fields[x, y].Next.Add(Direction.UP, Fields[x, y - 1]);
                                Fields[x, y].Next.Add(Direction.RIGHT, Fields[x + 1, y]);
                                Fields[x, y].Next.Add(Direction.DOWN, Fields[x, y + 1]);
                                Fields[x, y].Next.Add(Direction.LEFT, null);
                                break;
                        }
                    }

                    else {
                        Fields[x, y].Next = new Dictionary<Direction, Field>();
                        Fields[x, y].Next.Add(Direction.UP, Fields[x, y - 1]);
                        Fields[x, y].Next.Add(Direction.RIGHT, Fields[x + 1, y]);
                        Fields[x, y].Next.Add(Direction.DOWN, Fields[x, y + 1]);
                        Fields[x, y].Next.Add(Direction.LEFT, Fields[x - 1, y]); 
                    }
                }
            }

        }

        public bool IsCorner(int x, int y) {

            if(x == 0 && y == 0) {
                return true;
            }

            if(x == (_level[y].Length - 1) && y == 0) {
                return true;
            }

            if(x == 0 && y == (_level.Length - 1)) {
                return true;
            }

            if(x == (_level[y].Length - 1) && y == (_level.Length - 1)) {
                return true;
            }

            return false;

        }

        public Direction InvalidDirection(int x, int y) {

            if (y == 0) {
                return Direction.UP;
            }

            if(x == 0) {
                return Direction.LEFT;
            }

            if(x == (_level[y].Length - 1)) {
                return Direction.RIGHT;
            }

            if(y == (_level.Length - 1)) {
                return Direction.DOWN;
            }

            return Direction.INVALID;
            
        }

        // TODO: Update fields if destroyed or cleaned. Mud => Void || Field => Void (if breakable)
        public void UpdateFields() {
            
        }
    }
}
