using System.Threading.Tasks;
using System.Timers;
using BoulderDash.Models.Characters;
using BoulderDash.Models.Objects;
using BoulderDash.Models.World;

namespace BoulderDash.Models {
    public class Game {

        private string[] _levels = new string[4];
        private static Timer _timer;
        public bool IsPlaying { get; set; }
        public int DiamondsLeft { get; set; }
        public int Score { get; set; }
        public int TimeLimit { get; set; }
        public Level CurrentLevel;

        public Game() {
            _levels[0] = "./Levels/BD_level1.txt";
            _levels[1] = "./Levels/BD_level2.txt";
            _levels[2] = "./Levels/BD_level3.txt";
            _levels[3] = "./Levels/level1.txt";
            CurrentLevel = new Level(_levels[0], 150, new Level(_levels[1], 150, new Level(_levels[2], 999, null)));
            TimeLimit = CurrentLevel.TimeLimit;
            IsPlaying = true;
            _timer = new Timer(1000);
            _timer.Elapsed += UpdateTimer;
            _timer.Start();

        }

        public void MoveHero(Direction direction) {

            if(CurrentLevel.Hero.IsAlive) {
                CurrentLevel.Hero.Move(direction);
                CheckObjects();
                if (CurrentLevel.Hero.CurrentField is Exit) {
                    if (((Exit)CurrentLevel.Hero.CurrentField).IsFinished()) {
                        CurrentLevel = CurrentLevel.Next;
                        TimeLimit = CurrentLevel.TimeLimit;
                        Score += (CurrentLevel.TimeLimit) * 10;   
                    }
                }
            }

        }

        public void CheckHero() {

            if(!CurrentLevel.Hero.IsAlive) {
                IsPlaying = false;
            }

        }

        public void MoveEnemies() {

            foreach (Enemy enemy in CurrentLevel.Enemies.ToArray()) {

                if (!enemy.IsAlive) {
                    CurrentLevel.Enemies.Remove(enemy);
                    CheckHero();
                }

                enemy.Move(enemy.CheckDirections());

            }
        }

        public void CheckDiamonds() {

            DiamondsLeft = 0;

            foreach (Objects.Object obj in CurrentLevel.Objects.ToArray()) {

                if(obj is Diamond) {
                    if(((Diamond)obj).IsCollected) {
                        DiamondsLeft--;
                        CollectDiamond(obj);
                    }

                    DiamondsLeft++;
                }

                if (DiamondsLeft == 0) {
                    foreach (Field field in CurrentLevel.Fields) {
                        if (field is Exit) {
                            ((Exit)field).IsHidden = false;
                        }
                    }
                }
            
            }                
        }

        public void CheckObjects() {

            CheckDiamonds();

            foreach(Objects.Object obj in CurrentLevel.Objects.ToArray()) {

                if (obj.IsDestroyed) {
                    CurrentLevel.Objects.Remove(obj);
                }
                
                if (obj.CanFall()) {
                    obj.Fall();
                }

                else {
                    if(obj.CanSlideLeft()) {
                        obj.Slide(Direction.LEFT);
                    }

                    else if (obj.CanSlideRight()){
                        obj.Slide(Direction.RIGHT);
                    }
                }

                if (obj.HasCollided) {
                    
                    Field characterField;
                    obj.CurrentField.Next.TryGetValue(Direction.DOWN, out characterField);

                    if(characterField.Character is Hero) {
                        characterField.Character.SelfDestruct();
                        CurrentLevel.Objects.Remove(obj);
                        IsPlaying = false;    
                        return;

                    }

                    if(characterField.Character is Enemy) {

                        characterField.Character.SelfDestruct();
                        CurrentLevel.Objects.Remove(obj);
                        CurrentLevel.Enemies.Remove(((Enemy)characterField.Character));
                        characterField.Character = null;
                        Score += 250;
                    }
                }
            }
        }

        public void CollectDiamond(Objects.Object obj) {

            if (obj.CurrentField.Character is Hero) {
                CurrentLevel.Hero.CollectDiamond(obj.CurrentField);
                CurrentLevel.Objects.Remove(obj);
                Score += 10;
            }   
        }

        private void UpdateTimer(object source, ElapsedEventArgs e) {
            TimeLimit--;
            if(TimeLimit == 0 && !(CurrentLevel.Hero.CurrentField is Exit)) {
                IsPlaying = false;
            }
        }

    }
}