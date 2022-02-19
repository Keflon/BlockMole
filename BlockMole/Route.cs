// See https://aka.ms/new-console-template for more information
using System.Text;

internal class Route
{
    public Route(Route head, Cell tail)
    {
        Head = head;
        Tail = tail;

        Length = (head?.Length ?? 0) + 1;
    }

    public int Length { get; }

    public Route Head { get; }
    public Cell Tail { get; }

    public byte[] GetRouteData()
    {
        var result = new byte[Length];

        int index = 0;

        var current = this;

        while(current != null)
        {
            result[index++] = current.Tail.Data;
            current = current.Head;
        }
        return result;
    }
}