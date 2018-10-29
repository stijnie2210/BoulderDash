using System;

namespace BoulderDash.Models {
    public class LevelReader {

        public string[] ReadLevel(string fileLocation)
        {
            String temp = this.getLocation;
            return System.IO.File.ReadAllLines(temp);
        }

        public String getLocation()
        {
            String location = "../Levels/BD_level1.txt";
            return location;
        }

    }
}
