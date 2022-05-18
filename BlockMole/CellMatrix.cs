// See https://aka.ms/new-console-template for more information
internal class CellMatrix
{
    public CellMatrix(int width, int height)
    {
        Width = width;
        Height = height;

        var cellList = new List<Cell>();

        // Create (width x height) cells where each cell contains a unit of data and store them in cellList ...
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                cellList.Add(new Cell());

        // Tell each cell it's neighbours ...
        int index = 0;
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                // Give the current cell a list of its neighbours ...
                var neighbours = new List<Cell>();

                if (y > 0)
                    neighbours.Add(cellList[index - Width]);  // Up one row

                if (x > 0)
                    neighbours.Add(cellList[index - 1]);    // Left one

                if (y < Height - 1)
                    neighbours.Add(cellList[index + Width]);  // Down one

                if (x < Width - 1)
                    neighbours.Add(cellList[index + 1]);      // Right one

                cellList[index].Neighbours = neighbours;

                index++;
            }

        CellList = cellList;
    }

    public void ApplyData(byte[] data)
    {
        if (Width * Height != data.Length)
            throw new InvalidOperationException("data length must equal Width*Height");

        int cellIndex = 0;
        foreach (Cell cell in CellList)
            cell.Data = data[cellIndex++];
    }

    public int Width { get; }
    public int Height { get; }
    public IEnumerable<Cell> CellList { get; }
}