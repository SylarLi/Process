using System;
using System.Collections;
using System.Collections.Generic;

class PathFinding
{
    public const int StepCost = 1;

    private RectGrid mRectGrid;

    private CuboidPathStep mStart;

    private CuboidPathStep mEnd;

    private bool mArrived;

    private CuboidPath mPath;

    private int mRecursion;

    private double mTimeCost;

    public PathFinding(RectGrid rectGrid)
    {
        mRectGrid = rectGrid;
        mPath = new CuboidPath();
    }

    public bool arrived
    {
        get
        {
            return mArrived;
        }
    }

    public CuboidPath path
    {
        get
        {
            return mPath;
        }
    }

    public int stepNums
    {
        get
        {
            return mPath.Count - 1;
        }
    }

    public int recursion
    {
        get
        {
            return mRecursion;
        }
    }

    public double timeCost
    {
        get
        {
            return mTimeCost;
        }
    }

    public void Search(CuboidPathStep start, CuboidPathStep end)
    {
        if (!mRectGrid.CanCarry(start) || !mRectGrid.CanCarry(end))
        {
            throw new InvalidOperationException();
        }
        if (start == end)
        {
            throw new InvalidOperationException();
        }
        if (end.s != CuboidState.Y)
        {
            throw new InvalidOperationException();
        }
        mStart = start;
        mEnd = end;
        mArrived = false;
        mRecursion = 0;
        mPath.Clear();
        DateTime startTime = DateTime.Now;
        Dijkstra();
        mTimeCost = (DateTime.Now - startTime).TotalMilliseconds;
    }

    private void Dijkstra()
    {
        Dictionary<CuboidPathStep, CuboidPath> paths = new Dictionary<CuboidPathStep, CuboidPath>();
        Queue<CuboidPathStep> open = new Queue<CuboidPathStep>();
        HashSet<CuboidPathStep> close = new HashSet<CuboidPathStep>();

        paths[mStart] = new CuboidPath();
        paths[mStart].Enqueue(mStart);
        open.Enqueue(mStart);
        while(open.Count > 0)
        {
            mRecursion += 1;
            CuboidPathStep current = open.Dequeue();
            close.Add(current);
            CuboidPathStep[] dir4 = new CuboidPathStep[]
            {
                current.Left(),
                current.Right(),
                current.Forward(),
                current.Backward()
            };
            int dirStepNums = paths[current].Count + 1;
            foreach (CuboidPathStep dir in dir4)
            {
                if (mRectGrid.CanCarry(dir) &&
                    !open.Contains(dir) &&
                    !close.Contains(dir))
                {
                    open.Enqueue(dir);
                    if (!paths.ContainsKey(dir) ||
                        dirStepNums < paths[dir].Count)
                    {
                        paths[dir] = new CuboidPath(paths[current].ToArray());
                        paths[dir].Enqueue(dir);
                    }
                    if (dir == mEnd)
                    {
                        mArrived = true;
                        mPath = paths[mEnd];
                        return;
                    }
                }
            }
        }
    }

    private int CalcHeuristic(CuboidPathStep current)
    {
        int estimate = int.MaxValue;
        switch (current.s)
        {
            case CuboidState.Y:
                {

                    break;
                }
            case CuboidState.X:
                {
                    break;
                }
            case CuboidState.Z:
                {

                    break;
                }
        }
        return estimate;
    }
}