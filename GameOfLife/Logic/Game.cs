using GameOfLife.Logic.Managers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace GameOfLife.Logic
{
    [Serializable]
    public class Game
    {
        public Game(int height, int width, int startCount, Random random)
        {
            Height = height;
            Width = width;
            aliveCount = startCount;

            world = new bool[Height, Width];

            GenerateWorld(startCount, random);

            isRunning = true;
            isWorldStable = false;
        }

        public int Width { get; }
        public int Height { get; }

        private bool[,] world;
        private bool[,] previousStepWorld;
        private bool isStopped;
        private int aliveCount;
        private bool isWorldStable;
        private bool isRunning;

        public int AliveCount { get { return aliveCount; } }
        public bool IsWorldStable { get { return isWorldStable; } }
        public bool IsRunning { get { return isRunning; } }

        [NonSerialized]
        private Mutex mutex;

        private void GenerateWorld(int startCount, Random random)
        {
            if (Width * Height < startCount)
                throw new Exception("Start count can't be more than world size!");

            while (startCount > 0)
            {
                int x = random.Next(0, Width), y = random.Next(0, Height);
                if (!world[y, x])
                {
                    world[y, x] = true;
                    startCount--;
                }
            }
        }

        public void Start()
        {
            new Thread(Life).Start();
        }

        private void Life()
        {
            mutex = new Mutex();

            while (AliveCount > 0 && !IsWorldStable)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                if (!isStopped)
                    UpdateWorld();
                stopwatch.Stop();
                Thread.Sleep(1000 - (int)stopwatch.ElapsedMilliseconds > 0 ? 1000 - (int)stopwatch.ElapsedMilliseconds : 0);
            }
            isRunning = false;
        }

        public void Stop()
        {
            isStopped = true;
        }

        public void Continue()
        {
            isStopped = false;
        }

        private void UpdateWorld()
        {
            mutex.WaitOne();

            previousStepWorld = (bool[,])world.Clone();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int neighbourCount = 0;

                    for (int j = -1; j <= 1; j++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            if (y + j >= 0 && y + j < Height && x + i >= 0 && x + i < Width && !(j == 0 && i == 0) && world[y + j, x + i])
                                neighbourCount++;
                        }
                    }

                    if (neighbourCount == 3)
                        world[y, x] = true;
                    else if (neighbourCount != 2 && world[y, x])
                        world[y, x] = false;

                }
            }

            aliveCount = world.Cast<bool>().Where(b => b).Count();
            isWorldStable = previousStepWorld.Cast<bool>().SequenceEqual(world.Cast<bool>());

            mutex.ReleaseMutex();
        }

        public string GetVisualWorld()
        {
            mutex.WaitOne();

            string image = "";
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    image += world[y, x] ? InformationManager.LifeCell : InformationManager.DeadCell;
                //image += '\n';
            }
            
            mutex.ReleaseMutex();

            return image;

        }
    }
}
