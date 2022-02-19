// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

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
cellMatrix.ApplyData("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123");


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


// Print all the routes. Impractical for more than about 3x2

foreach (Route route in routeList)
{
    foreach (var routeCell in route)
    {
        Console.Write(routeCell.Data);
    }
    Console.WriteLine();
}

//
// Print the number of routes.
//
Console.WriteLine($"Count : {routeList.Count}");



//
// Now find something
//

// Look for a 'function based' pattern. run > 3 and index[n+1] = index[n] + 1, i.e. (1, 2, 3, 4 ...), (2, 3, 4, 5 ...), (3, 4, 5, 6 ...)

//var rules = new List<Func<Route, bool>>();

//rules.Add(SequenceRule);

var results = new List<Route>();

Console.WriteLine($"Looking for n, n+1, n+2 ... ");

foreach (var route in routeList)
{
    if (SequenceRule
        (
        route,
        (route) => route.Count > 5,
        (route, index) => index == 0 || route[index-1].Data == route[index].Data - 1
))
        results.Add(route);
}


foreach (Route route in results)
{
    foreach (var routeCell in route)
    {
        Console.Write(routeCell.Data);
    }
    Console.WriteLine();
}

Console.WriteLine($"Looking for BEYONCE ... ");

foreach (var route in routeList)
{
    if (SequenceRule
        (
        route,
        (route) => route.Count == 7,
        (route, index) => route[index].Data == "BEYONCE"[index]
))
        results.Add(route);
}


foreach (Route route in results)
{
    foreach (var routeCell in route)
    {
        Console.Write(routeCell.Data);
    }
    Console.WriteLine();
}




// Search for 'Beyonce'. If found, sell it to her.

bool SequenceRule(Route route, Func<Route, bool> routePredicate, Func<Route, int, bool> sequenceBuilderPredicate)
{
    // routePredicate:
    // Given a route, can we trivially reject it?
    // e.g. Route.Length == 5
    // e.g. Route.Length < 5

    // sequenceBuilderPredicate(route, index):
    // Decides if the cell at 'index' satisfies the requirements for the sequence we're looking for,
    // where sequenceBuilderPredicate may *not* retain state.
    // ... is this limiting?

    if (routePredicate(route) == false)
        return false;

    for (int index = 0; index < route.Count; index++)
        if (sequenceBuilderPredicate(route, index) == false)
            return false;

    // We have a match!
    return true;
}



//
// For each Cell in a matrix
//
List<Route> GenerateRouteList(CellMatrix matrix)
{
    var retval = new List<Route>();

    foreach (var cell in matrix.CellList)
        GetRoutes(cell, matrix, retval);

    return retval;
}

//
// 
//
void GetRoutes(Cell cell, CellMatrix matrix, List<Route> routeList)
{
    // Add this cell's route to the results.
    routeList.Add(GetCellRoute(cell));

    foreach (var nextCellCandidate in cell.Neighbours)
    {
        // If the cell isn't already part of a route ...
        if (nextCellCandidate.Next == null)
        {
            cell.Next = nextCellCandidate;
            nextCellCandidate.Previous = cell;
            GetRoutes(nextCellCandidate, matrix, routeList);
            cell.Next = null;
            nextCellCandidate.Previous = null;
        }
    }
}

Route GetCellRoute(Cell cell)
{
    var route = new Route();

    var nextCell = cell;
    while (nextCell != null)
    {
        route.Add(nextCell);
        nextCell = nextCell.Previous;
    }
    return route;
}