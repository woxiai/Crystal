using System;
using System.Collections;
using System.Collections.Generic;
using Crystal.Collections;

namespace Crystal.Astar
{
    public abstract class AbsAstar
    {
        protected static readonly float DIRECT_VALUE = 1f;

        protected PriorityQueue<AstarCell> openQueue;

        protected List<AstarCell> closeList;

        public AbsAstar()
        {
            openQueue = new PriorityQueue<AstarCell>();
            closeList = new List<AstarCell>();
        }

        public AbsAstar(int collectionsCapacity)
        {
            openQueue = new PriorityQueue<AstarCell>(collectionsCapacity);
            closeList = new List<AstarCell>(collectionsCapacity);
        }

        public bool Find(AstarMap map)
        {
            if (map == null || map.mapCells == null || map.startCell == null || map.endCell == null)
            {
                return false;
            }
            openQueue.Clear();
            closeList.Clear();
            openQueue.Enqueue(map.startCell);
            while (openQueue.Count > 0)
            {
                if (closeList.Contains(map.endCell))
                {
                    return true;
                }
                var cell = openQueue.Dequeue();
                closeList.Add(cell);
                AddNeighborsToOpenQueue(map, cell);
            }
            return false;
        }

        void AddNeighborsToOpenQueue(AstarMap map, AstarCell currentCell)
        {
            var x = currentCell.x;
            var y = currentCell.y;

            AddNeighborToOpenQueue(map, currentCell, x - 1, y, DIRECT_VALUE);
            AddNeighborToOpenQueue(map, currentCell, x + 1, y, DIRECT_VALUE);

            AddNeighborToOpenQueue(map, currentCell, x, y - 1, DIRECT_VALUE);
            AddNeighborToOpenQueue(map, currentCell, x, y + 1, DIRECT_VALUE);

            AddNeighborToOpenQueue(map, currentCell, x - 1, y - 1, AstarDistanceUtility.SQRT2);
            AddNeighborToOpenQueue(map, currentCell, x - 1, y + 1, AstarDistanceUtility.SQRT2);

            AddNeighborToOpenQueue(map, currentCell, x + 1, y - 1, AstarDistanceUtility.SQRT2);
            AddNeighborToOpenQueue(map, currentCell, x + 1, y + 1, AstarDistanceUtility.SQRT2);
        }

        void AddNeighborToOpenQueue(AstarMap map, AstarCell currentCell, int x, int y, float value)
        {
            //超出
            if (x < 0 || x >= map.width || y < 0 || y >= map.height)
            {
                return;
            }
            var neighbor = map.mapCells[x, y];

            if (!neighbor.walkable)
            {
                return;
            }
            if (closeList.Contains(neighbor))
            {
                return;
            }
            var endCell = map.endCell;
            var cg = currentCell.g + value;
            var child = openQueue.Find((findCell) =>
            {
                return findCell == null ? false : (findCell.x == x && findCell.y == y);
            });
            //未找到
            if (child == null)
            {
                var h = CalculateHeuristic(neighbor, endCell);
                neighbor.parent = currentCell;
                neighbor.g = cg;
                neighbor.h = h;
                openQueue.Enqueue(neighbor);
            }
            else
            {
                if (child.g > cg)
                {
                    child.g = cg;
                    child.parent = currentCell;
                    openQueue.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// 是否是结束点
        /// </summary>
        /// <param name="endCell"></param>
        /// <param name="checkCell"></param>
        /// <returns></returns>
        bool IsEndCell(AstarCell endCell, AstarCell checkCell)
        {
            if (endCell == null || checkCell == null)
            {
                return false;
            }
            return endCell.Equals(checkCell);
        }

        protected virtual float CalculateHeuristic(AstarCell currentCell, AstarCell endCell)
        {
            return AstarDistanceUtility.MahattanDistance(currentCell, endCell);
        }

        public void PrintMap(AstarCell cell)
        {
            var parent = cell.parent;
            while (parent != null)
            {
                UnityEngine.Debug.LogFormat("X = {0}, Y = {1}", parent.x, parent.y);
                parent = parent.parent;
            }
        }
    }
}

