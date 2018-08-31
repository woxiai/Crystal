using System;

namespace Crystal.Astar
{
    public class AstarCell : IComparable<AstarCell>
    {
        public int x;

        public int y;

        public float g;

        public float h;

        public AstarCell parent;

        public virtual bool walkable
        {
            private set;
            get;
        }

        public AstarCell()
        {
        }

        public void Reset()
        {
            g = 0;
            h = 0;
            parent = null;
        }

        public AstarCell(int x, int y, bool walkable)
        {
            this.x = x;
            this.y = y;
            this.walkable = walkable;
        }

        public AstarCell(int x, int y, int g, int h, AstarCell parent, bool walkable) : this(x, y, walkable)
        {
            this.parent = parent;
        }

        public int CompareTo(AstarCell other)
        {
            if (other == null)
            {
                return -1;
            }
            var delta = (this.g + this.h) - (other.g + other.h);
            if (Math.Abs(delta) < 0.01F)
            {
                return 0;
            } else if (delta > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}

