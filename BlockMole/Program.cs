// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Hello, World!");

bool TestForSequence(byte[] data)
{
    //int d = 0;
    //for (int c = 1; c < data.Length; c++)
    //    if (data[c] != data[d++] + 1)
    //        return false;

    //return true;


    byte lastData = data[0];
    for (int c = 1; c < data.Length; c++)
    {
        if (data[c] != lastData + 1)
            return false;
        lastData = data[c];
    }

    return true;


    //var iterator = ((IEnumerable<byte>)data).GetEnumerator();

    //iterator.MoveNext();
    //var previous = iterator.Current;

    //while (iterator.MoveNext())
    //{
    //    var next = iterator.Current;
    //    if (previous != next - 1)
    //        return false;
    //    previous = next;
    //}
    //return true;
}

bool TestForSequence2(byte[] data, int count)
{
    byte lastData = data[0];
    for (int c = 1; c < count; c++)
    {
        if (data[c] != lastData + 1)
            return false;
        lastData = data[c];
    }

    return true;
}

bool TestForSequence3(IEnumerator<byte> enumerator)
{
    enumerator.MoveNext();
    var lastData = enumerator.Current;

    while(enumerator.MoveNext())
    {
        var nextData = enumerator.Current;
        if (lastData + 1 != nextData)
            return false;

        lastData = nextData;
    }
    return true;

    //byte lastData = data[0];
    //for (int c = 1; c < count; c++)
    //{
    //    if (data[c] != lastData + 1)
    //        return false;
    //    lastData = data[c];
    //}

    //return true;
}

//
// Build a matrix of cells where each cell knows its neighbours.
// Ideally the width and height and data will be command-line parameters
// and data will be a byte stream rather than a string.
//

//var cellMatrix = new CellMatrix(3, 2);
//var cellMatrix = new CellMatrix(4, 3);
var cellMatrix = new CellMatrix(6, 5);
//var cellMatrix = new CellMatrix(3, 2, "ABCDEF");
//var cellMatrix = new CellMatrix(5, 4, "ABCDEFGHIJKLMNOPQRST");
//var cellMatrix = new CellMatrix(6, 5, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123");

//
// Build a list of routes through the cell matrix. 
//
var routeList = GenerateRouteList(cellMatrix);



// NOTES: Every route is a 'route' plus a 'cell', assuming a route can be empty.
// Use this to massively optimise any search performance done during route-generation.
// Use this to massively optimise any memory pressure when storing 'all' routes.
   






// NOTES: We now have all the routes through the matrix, from any starting point.
// This is independent of the actual data, so can be precalculated for each permissible matrix dimensions.
// Each route is a linked list of cells, so it can be queried for any sort of match
// e.g. a fixed pattern such as 'LEET' or a sequence such as ((next == previous+1) AND patternlength > 5)


//cellMatrix.ApplyData("ABCDEF");
//cellMatrix.ApplyData("XBERECYQXNOP");
cellMatrix.ApplyData(Encoding.UTF8.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123"));


//
// Print each cell's data along with the data of its neighbours.
//
foreach (var cell in cellMatrix.CellList)
{
    Console.Write($"{cell.Data}:");

    foreach (var neighbour in cell.Neighbours)
        Console.Write($"{neighbour.Data}");

    Console.WriteLine();
}

//
// Print the number of routes.
//
Console.WriteLine($"Count : {routeList.Count}");

// Print all the routes. Impractical for more than about 3x2

//foreach (Route route in routeList)
//{
//    Console.WriteLine(route.ToString());
//}





//
// Now find something
//

// Look for a 'function based' pattern. run > 3 and index[n+1] = index[n] + 1, i.e. (1, 2, 3, 4 ...), (2, 3, 4, 5 ...), (3, 4, 5, 6 ...)

//var rules = new List<Func<Route, bool>>();

//rules.Add(SequenceRule);

var results = new List<Route>();

Console.WriteLine($"Looking for n, n+1, n+2 ... ");


#if old
foreach (var route in routeList)
{
    if(route.Length > 4)
    {
        byte[] data = route.GetRouteData();

        if (TestForSequence(data) == true)
            results.Add(route);
    }
}
#elif old

byte[] buffer = new byte[30];

foreach (var route in routeList)
{
    if (route.Length > 4)
    {
        int count = route.GetRouteData2(buffer);

        if (TestForSequence2(buffer, count) == true)
            results.Add(route);
    }
}

#else

foreach (var route in routeList)
{
    if (route.Length > 4)
    {
        var iterator = route.GetRouteData3();

        if (TestForSequence3(iterator) == true)
            results.Add(route);
    }
}


#endif

foreach (Route route in results)
{
    Console.WriteLine(Encoding.UTF8.GetString(route.GetRouteData().ToArray()));
}

 Console.WriteLine($"Looking for BEYONCE ... ");

foreach (var route in routeList)
{
    byte[] matchData = Encoding.UTF8.GetBytes("CDJPONHBAGM");

    if (route.Length == matchData.Length)
    {
        byte[] data = route.GetRouteData();

        if (TestForMatch(data, matchData) == true)
            results.Add(route);
    }
}

bool TestForMatch(byte[] data, byte[] matchData)
{
    if (data.Length != matchData.Length)
        return false;

    for (int i = 0; i < data.Length; i++)
        if (data[i] != matchData[i])
            return false;

    return true;
}

foreach (Route route in results)
{
    Console.WriteLine(Encoding.UTF8.GetString(route.GetRouteData().ToArray()));
}






//
// For each Cell in a matrix
//
List<Route> GenerateRouteList(CellMatrix matrix)
{
    var retval = new List<Route>();

    foreach (var cell in matrix.CellList)
        GetRoutes(null, cell, retval);

    return retval;
}

//
// 
//
void GetRoutes(Route route, Cell cell, List<Route> routeList)
{
    var thisRoute = new Route(route, cell);
    // Add this cell's route to the results.
    routeList.Add(thisRoute);

    foreach (var nextCellCandidate in cell.Neighbours)
    {
        // If the cell isn't already part of a route ...
        if (nextCellCandidate.IsInCurrentRoute == false)
        {
            cell.IsInCurrentRoute = true;
            GetRoutes(thisRoute, nextCellCandidate, routeList);
            cell.IsInCurrentRoute = false;
        }
    }
}