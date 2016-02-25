using System.Collections.Generic;

class CuboidPath : Queue<CuboidPathStep>
{
    public CuboidPath() : base()
    {

    }

    public CuboidPath(IEnumerable<CuboidPathStep> collection) : base(collection)
    {

    }
}