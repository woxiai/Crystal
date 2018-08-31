using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crystal.Astar
{
    public class AstarMap
    {
        public AstarCell[,] mapCells;

        public int width;

        public int height;

        public AstarCell startCell;

        public AstarCell endCell;

        public AstarMap(AstarCell[,] mapCells, int width, int height, AstarCell startCell, AstarCell endCell)
        {
            this.mapCells = mapCells;
            this.width = width;
            this.height = height;
            this.startCell = startCell;
            this.endCell = endCell;
        }
    }
}
