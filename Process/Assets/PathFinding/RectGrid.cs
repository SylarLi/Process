class RectGrid
{
    public int lenx;

    public int leny;

    public RectGrid(int lenx, int leny)
    {
        this.lenx = lenx;
        this.leny = leny;
    }

    public bool CanCarry(CuboidPathStep step)
    {
        bool result = false;
        switch (step.s)
        {
            case CuboidState.Y:
                {
                    result = step.x >= 0 &&
                        step.x < lenx &&
                        step.y >= 0 &&
                        step.y < leny;
                    break;
                }
            case CuboidState.X:
                {
                    result = step.x >= 0 &&
                        step.x < lenx - 1 &&
                        step.y >= 0 &&
                        step.y < leny;
                    break;
                }
            case CuboidState.Z:
                {
                    result = step.x >= 0 &&
                        step.x < lenx &&
                        step.y >= 0 &&
                        step.y < leny - 1;
                    break;
                }
        }
        return result;
    }
}