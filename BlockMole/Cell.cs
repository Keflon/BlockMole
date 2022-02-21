// See https://aka.ms/new-console-template for more information
internal class Cell
{
    public List<Cell> Neighbours { get; set; }

    /// <summary>
    /// Used during route-building to determine whether this cell is part of the current route or not.
    /// If 'Next' is null, the cell is not yet part of the current route.
    /// </summary>
    public bool IsInCurrentRoute { get; set; }

    public byte Data { get; set; }
}

