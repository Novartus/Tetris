using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Blocks
{
    public class QueueBlock
    {
        private readonly Block[] blocks = new Block[]
        {
            new I(),
            new J(),
            new L(),
            new O(),
            new S(),
            new T(),
            new Z(),
        };
        public QueueBlock()
        {
            NextBlock = RandomBlockGenerator();
        }

        private readonly Random random = new Random();

        public Block NextBlock { get; private set; }

        public Block FetchAndUpdate()
        {
            Block block = NextBlock;
            // To prevent from getting same block twice in the row
            do { NextBlock = RandomBlockGenerator(); } while (block.ID == NextBlock.ID);

            return block;
        }

        private Block RandomBlockGenerator()
        {
            return blocks[random.Next(blocks.Length)];
        }

    }
}
