using System;

class CuboidPathStep
{
    /// <summary>
    /// 位置 x
    /// </summary>
    public int x;

    /// <summary>
    /// 位置 y
    /// </summary>
    public int y;

    /// <summary>
    /// 状态 s
    /// </summary>
    public CuboidState s;

    public CuboidPathStep(int x, int y, CuboidState s)
    {
        this.x = x;
        this.y = y;
        this.s = s;
    }

    public CuboidPathStep Left()
    {
        switch (s)
        {
            case CuboidState.Y:
                {
                    return new CuboidPathStep(x - 2, y, CuboidState.X);
                }
            case CuboidState.X:
                {
                    return new CuboidPathStep(x - 1, y, CuboidState.Y);
                }
            case CuboidState.Z:
                {
                    return new CuboidPathStep(x - 1, y, CuboidState.Z);
                }
        }
        throw new InvalidOperationException();
    }

    public CuboidPathStep Right()
    {
        switch (s)
        {
            case CuboidState.Y:
                {
                    return new CuboidPathStep(x + 1, y, CuboidState.X);
                }
            case CuboidState.X:
                {
                    return new CuboidPathStep(x + 2, y, CuboidState.Y);
                }
            case CuboidState.Z:
                {
                    return new CuboidPathStep(x + 1, y, CuboidState.Z);
                }
        }
        throw new InvalidOperationException();
    }

    public CuboidPathStep Forward()
    {
        switch (s)
        {
            case CuboidState.Y:
                {
                    return new CuboidPathStep(x, y + 1, CuboidState.Z);
                }
            case CuboidState.X:
                {
                    return new CuboidPathStep(x, y + 1, CuboidState.X);
                }
            case CuboidState.Z:
                {
                    return new CuboidPathStep(x, y + 2, CuboidState.Y);
                }
        }
        throw new InvalidOperationException();
    }

    public CuboidPathStep Backward()
    {
        switch (s)
        {
            case CuboidState.Y:
                {
                    return new CuboidPathStep(x, y - 2, CuboidState.Z);
                }
            case CuboidState.X:
                {
                    return new CuboidPathStep(x, y - 1, CuboidState.X);
                }
            case CuboidState.Z:
                {
                    return new CuboidPathStep(x, y - 1, CuboidState.Y);
                }
        }
        throw new InvalidOperationException();
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", x, y, s);
    }

    public override bool Equals(object obj)
    {
        return GetHashCode() == obj.GetHashCode();
    }

    /// <summary>
    /// 为了缩小哈希表的分布范围，这里假设x，y不超过8位，也就是最多划分256X256个格子
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return (x << 11) + (y << 2) + (int)s;
    }

    public static bool operator ==(CuboidPathStep c1, CuboidPathStep c2)
    {
        if (object.Equals(c1, null) || object.Equals(c2, null))
        {
            return object.Equals(c1, c2);
        }
        return c1.x == c2.x &&
            c1.y == c2.y &&
            c1.s == c2.s;
    }

    public static bool operator !=(CuboidPathStep c1, CuboidPathStep c2)
    {
        return !(c1 == c2);
    }
}
