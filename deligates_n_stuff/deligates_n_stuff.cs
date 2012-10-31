using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUD
{
    enum Direction { North, South, East, West };
    delegate void Push(Direction dir);

    interface IController 
    {
        void Add(Push controllee);
        void Work();
    }

    interface IControllee
    {
        // void Move(Direction dir);
    }

    class RandomController : IController
    {
        Random rand = new Random();
        public Push who;
        public void Work()
        {
            Direction dir = Direction.North;
            switch (rand.Next(4))
            {
                case 0: dir = Direction.North; break;
                case 1: dir = Direction.South; break;
                case 2: dir = Direction.East;  break;
                case 3: dir = Direction.West;  break;
            }
            who(dir);
        }
        public void Add(Push pushed) 
        {
           who += pushed;
        }
    }

    class StraightController : IController
    {
        public Push who;
        public Direction dir;
        public StraightController(Direction _dir) 
        { 
          dir = _dir; 
        }

        public void Work()
        {
            who(dir);
        }
        public void Add(Push pushed)
        {
            who += pushed;
        }
    }

    class Gnome : IControllee
    {
        public void Stumble(Direction dir)
        {
            Console.WriteLine("Stumbling {0}", dir);
        }

    }

    class Troll : IControllee
    {
        public void Stomp(Direction dir)
        {
            Console.WriteLine("Stomping {0}", dir);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var troll = new Troll();
            var gnome = new Gnome();
            var rand = new RandomController();
            var north = new StraightController(Direction.North);

            rand.Add(gnome.Stumble);
            north.Add(troll.Stomp);

            rand.Work();
            north.Work();
        }
    }
}
