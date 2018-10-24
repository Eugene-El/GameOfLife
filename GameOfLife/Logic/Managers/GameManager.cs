using GameOfLife.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Logic.Managers
{
    public static class GameManager
    {
        private static List<Game> games = new List<Game>();
        private static Random random = new Random();

        public static string BackupFileName = "GameOfLife.wrld";

        public static int GameCount { get { return games.Count; } }
        public static int RunningGamesCount { get { return games.Where(g => g.IsRunning).Count(); } }
        public static int TotalLiveformCount { get { return games.Sum(g => g.AliveCount); } }
        public static double AverageLiveformCount { get { return TotalLiveformCount / (double)GameCount; } }

        public static int WorldWidth { get; private set; }
        public static int WorldHeight { get; private set; }

        static GameManager()
        {
            ExitManager.ApplicationExitEvent += GameManagerDestructor;
        }

        private static void GameManagerDestructor(object sender, EventArgs e)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(BackupFileName, FileMode.Create))
                {
                    lock (games)
                    {
                        formatter.Serialize(fs, games);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw ex;
            }
        }

        public static void Start(int gameCount, int height, int width, int startCount)
        {
            if (gameCount <= 0)
                throw new Exception("Game count can't be less than 1!");
            if (height <= 2 || width <= 2)
                throw new Exception("Width and height can't be less than 3");
            if (startCount <= 3)
                throw new Exception("Start count can't be less than 3");

            WorldHeight = height;
            WorldWidth = width;

            for (int i = 0; i < gameCount; i++)
            {
                games.Add(new Game(height, width, startCount, random));
                if (i < 9) InformationManager.WorldIndexes.Add(i);
            }

            games.ForEach(g => g.Start());
        }

        public static void ResumePreviousGames()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(BackupFileName, FileMode.Open))
                {
                    games = (List<Game>)formatter.Deserialize(fs);
                }
                WorldWidth = games.FirstOrDefault()?.Width ?? 0;
                WorldHeight = games.FirstOrDefault()?.Height ?? 0;

                for(int i = 0; i < (9 < GameCount? 9 : GameCount); i++)
                    InformationManager.WorldIndexes.Add(i);

                games.ForEach(g => g.Start());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw ex;
            }
        }


        public static void Stop()
        {
            lock (games)
                games.ForEach(g => g.Stop());
        }

        public static void Continue()
        {
            lock (games)
                games.ForEach(g => g.Continue());
        }

        public static List<string> GetVisualWorlds(List<int> indexes)
        {
            List<string> visualWorlds = new List<string>();
            for (int i = 0; i < (indexes.Count < 9 ? indexes.Count : 9); i++)
                visualWorlds.Add(games[indexes[i]].GetVisualWorld());
            return visualWorlds;
        }
    }
}
