// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text;

Console.WriteLine("Hello, World!");
int _theCount = 0;

bool TestForSequence(IEnumerator<byte> enumerator)
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
}

//
// Build a matrix of cells where each cell knows its neighbours.
// Ideally the width and height and data will be command-line parameters
// and data will be a byte stream rather than a string.
//

var cellMatrix = new CellMatrix(6, 5);

var sw = new Stopwatch();
sw.Start();
//
// Build a list of routes through the cell matrix. 
//
var routeList = GenerateRouteList(cellMatrix);

sw.Stop();
Console.WriteLine($"Time taken: {sw.Elapsed.TotalSeconds}, Count: {_theCount}");

// NOTES: Every route is a 'route' plus a 'cell', assuming a route can be empty.
// Use this to massively optimise any search performance done during route-generation.
// Use this to massively optimise any memory pressure when storing 'all' routes.


// NOTES: We now have all the routes through the matrix, from any starting point.
// This is independent of the actual data, so can be precalculated for each permissible matrix dimensions.
// Each route is a linked list of cells, so it can be queried for any sort of match
// e.g. a fixed pattern such as 'LEET' or a sequence such as ((next == previous+1) AND patternlength > 5)

cellMatrix.ApplyData(Encoding.UTF8.GetBytes(
    "ABCDEF" +
    "GHIJKL" +
    "MNOPQR" +
    "STUVWX" +
    "YZ0123"));

//
// Print each cell's data along with the data of its neighbours.
//
foreach (var cell in cellMatrix.CellList)
{
    Console.Write($"{(char)cell.Data} has neighbours:");

    foreach (var neighbour in cell.Neighbours)
        Console.Write($"{(char)neighbour.Data}");

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

// Look for a 'function based' pattern. run > 4 and index[n+1] = index[n] + 1, i.e. (1, 2, 3, 4, 5 ...), (2, 3, 4, 5, 6 ...), (3, 4, 5, 6, 7 ...)

//var rules = new List<Func<Route, bool>>();

//rules.Add(SequenceRule);

var results = new List<Route>();

Console.WriteLine($"Looking for n, n+1, n+2 where sequence length > 4 ... ");

foreach (var route in routeList)
{
    if (route.Length > 4)
    {
        var iterator = route.GetRouteDataEnumerator();

        if (TestForSequence(iterator) == true)
            results.Add(route);
    }
}

foreach (Route route in results)
{
    Console.WriteLine(Encoding.UTF8.GetString(route.GetRouteData().ToArray()));
}

 Console.WriteLine($"Looking for BEYONCE ... ");

cellMatrix.ApplyData(Encoding.UTF8.GetBytes(
    "xBExxx" +
    "xxYxxx" +
    "CNOxxx" +
    "Exxxxx" +
    "xxxxxx"));

results.Clear();

byte[] matchData = Encoding.UTF8.GetBytes("BEYONCE");

foreach (var route in routeList)
{
    if (route.Length == matchData.Length)
    {
        var iterator = route.GetRouteDataEnumerator();

        if (TestForMatch(iterator, matchData) == true)
            results.Add(route);
    }
}

bool TestForMatch(IEnumerator<byte> iterator, byte[] matchData)
{
    int index = 0;

    while (iterator.MoveNext())
    {
        if (iterator.Current != matchData[index++])
            return false;
    }
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

    // Instead of adding the route, test the route against our predicates.
    // We can quickly know:
    // cell data
    // last cell data
    // cell index
    // last cell index (obviously)
    // Predicates can maintain state, meaning
    // we can test for a string match by iterating over the match-string and comparing to cell[data]

    _theCount++;
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