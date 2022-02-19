// See https://aka.ms/new-console-template for more information
internal class Cell
{
    public Cell()
    {
        //Data = data;
    }

    public List<Cell> Neighbours { get; private set; }

    /// <summary>
    /// Used during route-building to determine whether this cell is part of the current route or not.
    /// If 'Next' is null, the cell is not yet part of the current route.
    /// </summary>
    public Cell Next { get; set; }
    /// <summary>
    /// Used to iterate over the route once we have generated it.
    /// Iteration is backwards if you consider 'forwards' to be the order the route is recursed.
    /// As the inverse of this route will also be present in the results the order doesn't matter.
    /// </summary>
    public Cell Previous { get; set; }
    public byte Data { get; set; }

    internal void SetNeighbours(List<Cell> neighbours)
    {
        Neighbours = neighbours;
    }
}

