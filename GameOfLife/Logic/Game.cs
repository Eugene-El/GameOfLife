using GameOfLife.Logic.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife.Logic
{
    public class Game
    {
        public Game(int height, int width, int startCount, Random random)
        {
            Height = height;
            Width = width;
            AliveCount = startCount;

            world = new bool[Height, Width];

            GenerateWorld(startCount, random);
        }

        public int Width { get; }
        public int Height { get; }

        private bool[,] world;
        private bool[,] previousStepWorld;

        public int AliveCount { get; private set; }
        public bool IsWorldStable { get; private set; }
        public bool IsRunning { get; private set; }


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
            IsRunning = true;
            IsWorldStable = false;
            while (AliveCount > 0 && !IsWorldStable)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                UpdateWorld();
                stopwatch.Stop();
                Thread.Sleep(1000 - (int)stopwatch.ElapsedMilliseconds);
            }
            IsRunning = false;
        }

        private void UpdateWorld()
        {
            lock (world)
            {
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

                AliveCount = world.Cast<bool>().Where(b => b).Count();
                IsWorldStable = previousStepWorld.Cast<bool>().SequenceEqual(world.Cast<bool>());
            }
        }

        public string GetVisualWorld()
        {
            lock (world)
            {
                string image = "";
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                        image += world[y, x] ? InformationManager.LifeCell : InformationManager.DeadCell;
                    //image += '\n';
                }
                return image;
            }
        }
    }
}
