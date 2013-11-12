using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
namespace AHGame
{
    public class SFXController
    {
        Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        ContentManager Content;
        public SFXController(ContentManager Content)
        {
            this.Content = Content;
            SoundEffect.MasterVolume = 0.2f;
        }

        public int addSound(String name, String path)
        {
            SoundEffect s = Content.Load<SoundEffect>(path);
            sounds.Add(name, s);
            return 0;
        }
        public void playSound(String name)
        {
            try
            {
                sounds[name].Play();
            }
            catch { }
        }
        //has to be between 0.0(silence) to 1.0(max)
        public void setMasterSFXVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;
        }


    }
}
