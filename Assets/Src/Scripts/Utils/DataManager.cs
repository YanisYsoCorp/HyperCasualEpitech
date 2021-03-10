using UnityEngine;

namespace YsoCorp {

    [DefaultExecutionOrder(-1)]
    public class DataManager : ADataManager {

        private static string PSEUDO = "PSEUDO";
        private static string LEVEL = "LEVEL";
        private static string NUMCHARACTER = "NUMCHARACTER";

        private static int DEFAULT_LEVEL = 1;

        /***** CUSTOM  *****/

        // LEVEL
        public int GetLevel() {
            return this.GetInt(LEVEL, DEFAULT_LEVEL);
        }
        public int NextLevel() {
            int level = this.GetLevel() + 1;
            this.SetInt(LEVEL, this.GetLevel() + 1);
            return level;
        }
        public int PrevLevel() {
            int level = Mathf.Max(this.GetLevel() - 1, DEFAULT_LEVEL);
            this.SetInt(LEVEL, level);
            return level;
        }

        //PLAYER NAME
        public string GetPseudo() {
            return this.GetString(PSEUDO, "Player");
        }
        public void SetPseudo(string pseudo) {
            this.SetString(PSEUDO, pseudo);
        }

        // NUM CHARACTER
        public int GetNumCharacter() {
            return this.GetInt(NUMCHARACTER, -1);
        }
        public void SetNumCharacter(int num) {
            this.SetInt(NUMCHARACTER, num);
        }
        public void UnlockNumCharacter(int num) {
            this.SetInt(NUMCHARACTER + num, 1);
        }
        public bool IsUnlockNumCharacter(int num) {
            this.UnlockNumCharacter(0);
            return this.GetInt(NUMCHARACTER + num, 0) == 1;
        }

    }
}