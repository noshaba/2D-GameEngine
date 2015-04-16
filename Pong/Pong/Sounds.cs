using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;

namespace Pong {
    public static class Sounds {
        public static Sound scoreSound = new Sound(new SoundBuffer("../Content/score.ogg"));
        public static Sound hitSound = new Sound(new SoundBuffer("../Content/Hit.wav"));
    }
}
