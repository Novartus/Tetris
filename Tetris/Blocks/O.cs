using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Blocks
{
    public class O : Block
    {
        // Rotatation will be the same for all sides
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }
        };   
      
        public override int ID { get { return 4; } }

        protected override Position StartOffSet { get { return new Position(0, 4); } }
        protected override Position[][] Tiles { get { return tiles; } }
    }
}
