using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Blocks;

namespace Tetris
{
    public class State
    {
        private Block currentSelectedBlock;
        public Block CurrentSelectedBlock {
            get { return currentSelectedBlock; }
            private set
            {
                currentSelectedBlock = value; 
                currentSelectedBlock.Reset(); 
            } 
        }

        public GameGrid GameGrid { get;  }
        public QueueBlock BlockQueue { get; }

        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public int ScoreLine { get; private set; }
        public Block BlockOnHold { get; private set; }
        public bool BlockCanHold { get; private set; }


        public State()
        {
            GameGrid = new GameGrid(22, 10); // Game Grid Size
            BlockQueue = new QueueBlock();
            CurrentSelectedBlock = BlockQueue.FetchAndUpdate();
            BlockCanHold = true;
        }

        private bool CanFitBlock()
        {
            foreach (Position position in CurrentSelectedBlock.TilesPosition())
            {
                // If any block is overlapping or is outside the grid then it will return false
                if (!GameGrid.CheckEmpty(position.Row, position.Column))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckGameOver()
        {
            return !(GameGrid.CheckRowEmpty(0) && GameGrid.CheckRowEmpty(1));
        }

        public void RotateBlockClockWise()
        {
            CurrentSelectedBlock.ClockWiseRotation();
            if (!CanFitBlock())
            {
                CurrentSelectedBlock.CounterClockWiseRotation();
            }
        }

        public void RotateBlockCounterClockWise()
        {
            CurrentSelectedBlock.CounterClockWiseRotation();
            if (!CanFitBlock())
            {
                CurrentSelectedBlock.ClockWiseRotation();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentSelectedBlock.Mover(0, -1);
            if (!CanFitBlock())
            {
                CurrentSelectedBlock.Mover(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentSelectedBlock.Mover(0, 1);
            if (!CanFitBlock())
            {
                CurrentSelectedBlock.Mover(0, -1);
            }
        }


        private void BlockPlacer()
        {
            foreach (Position position in CurrentSelectedBlock.TilesPosition())
            {
                GameGrid[position.Row, position.Column] = CurrentSelectedBlock.ID;
            }

            ScoreLine = ScoreLine + GameGrid.ClearFullRow();

            Score = Score +  20 + (3 * ScoreLine);
            if (CheckGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentSelectedBlock = BlockQueue.FetchAndUpdate();
                BlockCanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            CurrentSelectedBlock.Mover(1, 0);
            if (!CanFitBlock())
            {
                CurrentSelectedBlock.Mover(-1, 0);
                BlockPlacer();
            }
        }

        public void HoldBlock()
        {
            if (!BlockCanHold) { return; }
            if (BlockOnHold == null)
            {
                BlockOnHold = CurrentSelectedBlock;
                CurrentSelectedBlock = BlockQueue.FetchAndUpdate();
            }
            else
            {
                Block tmp = CurrentSelectedBlock;
                CurrentSelectedBlock = BlockOnHold;
                BlockOnHold = tmp;
            }
            BlockCanHold = false;
        }

        private int TileDropDistance(Position position)
        {
            int dropDistance = 0;
            while (GameGrid.CheckEmpty(position.Row + dropDistance + 1, position.Column))
            {
                dropDistance++;
            }
            return dropDistance;
        }

        public int BlockDropDistance()
        {
            int dropDistance = GameGrid.Rows;
            foreach (Position position in CurrentSelectedBlock.TilesPosition())
            {
                dropDistance = System.Math.Min(dropDistance, TileDropDistance(position));
            }
            return dropDistance;
        }

        public void BlockDropper()
        {
            CurrentSelectedBlock.Mover(BlockDropDistance(), 0);
            BlockPlacer();
        }

    }
}
