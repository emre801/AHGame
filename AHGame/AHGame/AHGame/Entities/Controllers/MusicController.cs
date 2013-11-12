using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace AHGame
{
    public class MusicController
    {
        Dictionary<string, Song> music = new Dictionary<string, Song>();
        ArrayList musicKeys = new ArrayList();
        ContentManager Content;
        Random r;

        public MusicController(ContentManager Content)
        {
            this.Content = Content;
            r = new Random();
            MediaPlayer.Volume = 0.02f;
        }
        public int addMusic(String name, String path)
        {
            try
            {
                Song song = Content.Load<Song>(path);
                music.Add(name, song);
                musicKeys.Add(name);
            }
            catch { }
            return 0;
        }
        public void playMusic(String name)
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                Song song = music[name];
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = false;
            }
        }

        public void pauseMusic()
        {
            MediaPlayer.Pause();
        }
        public void resumeMusic()
        {
            MediaPlayer.Resume();
        }

        public void playRandomMusic()
        {
            int count = musicKeys.Count;
            String randomKey = (String)musicKeys[r.Next(count)];
            playMusic(randomKey);
        }

        public void setVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }
    }
}
