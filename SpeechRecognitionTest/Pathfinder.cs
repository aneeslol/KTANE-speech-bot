using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Tile Parent { get; set; }

        //The distance is essentially the estimated distance, ignoring walls to our target. 
        //So how many tiles left and right, up and down, ignoring walls, to get there. 
        public void SetDistance(int targetX, int targetY)
        {
            this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }
    }

    public class Pathfinder
    {
        private static List<Tile> GetWalkableTiles(List<string> map, Tile currentTile, Tile targetTile)
        {
            var adjacentTiles = new List<Tile>()
            {
                new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            var possibleTiles = new List<Tile>()
            {
                new Tile { X = currentTile.X, Y = currentTile.Y - 2, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X, Y = currentTile.Y + 2, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X - 2, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X + 2, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            var maxX = map.First().Length - 1;
            var maxY = map.Count - 1;

            for (int i = 0; i < 4; i++)
            {
                var adjTile = adjacentTiles[i];
                if (adjTile.Y >= 0 && adjTile.Y <= maxY && adjTile.X >= 0 && adjTile.X <= maxX && map[adjTile.Y][adjTile.X] == 'X')
                    possibleTiles[i].Cost = 9999;
            }

            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

            return possibleTiles
                    .Where(tile => tile.X >= 0 && tile.X <= maxX)
                    .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
                    .Where(tile => map[tile.Y][tile.X] == '*' || map[tile.Y][tile.X] == 'o')
                    .Where(tile => tile.Cost != 9999)
                    .ToList();
        }

        public List<string> FindPath(List<string> map, int startX, int startY, int endX, int endY)
        {
            var start = new Tile();
            start.Y = (startY - 1) * 2;
            start.X = (startX - 1) * 2;

            var finish = new Tile();
            finish.Y = (endY - 1) * 2;
            finish.X = (endX - 1) * 2;

            start.SetDistance(finish.X, finish.Y);

            var activeTiles = new List<Tile>();
            activeTiles.Add(start);
            var visitedTiles = new List<Tile>();

            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    //We found the destination and we can be sure (Because the the OrderBy above)
                    //That it's the most low cost option. 
                    var tile = checkTile;
                    Console.WriteLine("Retracing steps backwards...");
                    var tileList = new List<Tile>();
                    while (true)
                    {
                        tileList.Add(tile);
                        //Console.WriteLine($"{tile.X} : {tile.Y}");
                        //if (map[tile.Y][tile.X] == ' ')
                        //{
                        //    var newMapRow = map[tile.Y].ToCharArray();
                        //    newMapRow[tile.X] = '*';
                        //    map[tile.Y] = new string(newMapRow);
                        //}
                        tile = tile.Parent;
                        if (tile == null)
                        {
                            //Console.WriteLine("Map looks like :");
                            //map.ForEach(x => Console.WriteLine(x));
                            //Console.WriteLine("Done!");
                            tileList.Reverse();
                            var directions = new List<string>();
                            for(var i = 0; i < tileList.Count() - 1; i++)
                            {
                                var current = tileList[i];
                                var next = tileList[i + 1];

                                if (next.Y > current.Y)
                                    directions.Add("down");
                                else if (next.Y < current.Y)
                                    directions.Add("up");
                                else if (next.X > current.X)
                                    directions.Add("right");
                                else if (next.X < current.X)
                                    directions.Add("left");
                            }
                            return directions;
                        }
                    }
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    {
                        var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            return new List<string>();
        }
    }
}
