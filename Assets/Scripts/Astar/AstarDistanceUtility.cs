using System;

namespace Crystal.Astar
{
    public class AstarDistanceUtility
    {
        /// <summary>
        /// Math.Sqrt(2)
        /// </summary>
        public static readonly float SQRT2 = 1.4142F;

        /// <summary>
        /// 曼哈顿距离
        /// </summary>
        /// <param name="currentCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static float MahattanDistance(AstarCell currentCell, AstarCell endCell)
        {
            return Math.Abs(currentCell.x - endCell.x) + Math.Abs(currentCell.y - endCell.y);
        }

        /// <summary>
        /// 对角线距离
        /// </summary>
        /// <param name="currentCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static float DiagonalDistance(AstarCell currentCell, AstarCell endCell)
        {
            var absX = Math.Abs(currentCell.x - endCell.x);
            var absY = Math.Abs(currentCell.y - endCell.y);
            var min = Math.Min(absX, absY);
            return (SQRT2 - 2) * min + absX + absY;
        }

        /// <summary>
        /// 欧几里得距离
        /// </summary>
        /// <param name="currentCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static float EuclidDistance(AstarCell currentCell, AstarCell endCell)
        {
            var xMinus = currentCell.x - endCell.x;
            var yMinus = currentCell.y - endCell.y;
            return (float)Math.Sqrt(xMinus * xMinus + yMinus * yMinus);
        }
    }
}
