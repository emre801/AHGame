using System;
using System.Diagnostics;
namespace AHGame
{
    public class Ticker
    {
        float tickBeat;
        Stopwatch stopwatch;

        public Ticker(float tickBeat)
        {
            this.tickBeat = tickBeat;
            this.stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void setTickBeat(float tickBeat)
        {
            if (tickBeat > 0)
                this.tickBeat = tickBeat;
        }

        public void pauseUnpause()
        {
            if (stopwatch.IsRunning)
                stopwatch.Stop();
            else
                stopwatch.Start();
        }
        public bool hasTicked()
        {
            stopwatch.Stop();
            int time = (int)stopwatch.ElapsedMilliseconds;
            stopwatch.Start();
            if (time >= tickBeat)
            {
                stopwatch.Restart();
                return true;
            }
            return false;
        }

    }
}

