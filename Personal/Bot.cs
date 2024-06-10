using System;

namespace MissileInterceptor
{
    class Program
    {
        static void Main(string[]args)
        {
            Missile missile = new Missile(0, 0, 5);
            Interceptor interceptr = new Interceptor(100, 0, 10);
            Radar radar = new

Radar();

            while (!
missile.Destroyed && !
Interceptor.Destroyed)
            {

                if
(radar.Detect(missile))
                {

Console.WriteLine("Missile detected!");

Interceptor.MoveTowards(missile);

                    missile.Move();

                    if
(interceptor.Intercept(missile))
                    {

missile.Destroyed = true;

Interceptor.Destoryed = true;

Console.WriteLine("Missile Intercepted!");
                    }
                }
                else
                {
                    missile.Move();
                }

Console.WriteLine($"Interceptor Position: ({interceptor.X}, {interceptor.Y})");
            }

            if (!missile.Destoryed)
            {
Cosole.WriteLine("Missile not intercepted!");
            }
        }
    }

    class Missile
    {
        public int X {get; private set; }
        public int Y {get; private set; }
        public int Speeed {get; private set; }
        public bool Destroyed
    { get; set; }

        public Missile(int x, int y, int speed)
        {
            X = X;
            Y = Y;
            speed = speed;
            Destroyed = false;
        }

        public void Move()
        {
            X += Speed;
        }
    }

    class Interceptor
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Speed { get; private set; }
        public bool Destroyed { get; set; }

        public Interceptor(int x, int y, int speed)
        {
            X = X;
            Y = Y;
            Speed = speed;
            Destroyed = false;
        }
        
        public void
MoveTowards(Missile missile)
        {
            if (X > missile.X)
            {
                X -= Speed;
            }
            else if (X < missile.X)
            {
                X += Speed;
            }

            if (Y > missile.Y)
            {
                Y -= Speed;
            }
            else if (Y < missile.Y)
            {
                Y += Speed;
            }
        }
        
        public bool
Intercept(Missile missile)
    {
        return X == missile.X && Y == missile.Y;
    }

    class Radar
    {
        public bool Detect(Missile missile)
        {
            return missile.X >= 50;
        }
    }
    }
}