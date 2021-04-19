using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroDesktopCharacter.Data
{
    public class RetroDesktopCharacterOptions
    {
        public float BottomOffset { get; set; } = 20;
        
        public List<GifItem> Items { get; set; }
    }

    public class GifItem
    {
        public int Location { get; set; }
        public Behaviour Behaviour { get; set; }
        public bool CanCollide { get; set; }

    }

    public enum Behaviour
    {
        StayStill,
        BottomScroll,
        TopScroll,
        LeftScroll,
        RightScroll,
        Bounce
    }
}
