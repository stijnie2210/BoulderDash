using System;

namespace BoulderDash.Models {
    public class LevelReader {

        public string[] ReadLevel(string fileLocation) {
            return System.IO.File.ReadAllLines(@fileLocation);
        }

    }
}
