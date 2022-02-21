using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffSet { get; }
        public abstract int ID { get; }

        private int rotationState;
        private Position offSet;

        public Block()
        {
             offSet = new Position(StartOffSet.Row, StartOffSet.Column);
        }

        public IEnumerable<Position> TilesPosition()
        {
            foreach (Position position in Tiles[rotationState])
            {
                yield return new Position(position.Row + offSet.Row, position.Column + offSet.Column);
            }
        }

        public void ClockWiseRotation()
        {
            rotationState = (rotationState + 1) % Tiles.Length;

        }

        public void CounterClockWiseRotation()
        {
          if(rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Mover(int rows, int columns)
        {
            offSet.Row = offSet.Row + rows;
            offSet.Column = offSet.Column + columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offSet.Row = StartOffSet.Row;
            offSet.Column = StartOffSet.Column;
        }
    }
}
