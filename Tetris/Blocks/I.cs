using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Blocks
{
    public class I : Block
    {
        private readonly Position[][] tiles = new Position[][]
       {
           // ClockWise Rotation
            new Position[] { new(1,0), new(1,1), new(1,2), new(1,3) },
            new Position[] { new(0,2), new(1,2), new(2,2), new(3,2) },
            new Position[] { new(2,0), new(2,1), new(2,2), new(2,3) },
            new Position[] { new(0,1), new(1,1), new(2,1), new(3,1) }
       };

        public override int ID { get { return 1; } }

        // Spawn in the middle of top row
        protected override Position StartOffSet { get {return  new Position(-1, 3); } } 
        protected override Position[][] Tiles { get { return tiles; } }
    }
}
