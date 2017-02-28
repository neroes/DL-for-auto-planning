using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace RPGModel
{
    //Map related logic. Extends ObservableCollection
    public class Map : ObservableCollection<Tile>
    {
        public int nCols { get; set; }
        public int nRows { get; set; }



        public Map(int[] Mapst)
        {
            loadMap(Mapst);
            nCols = 12;
            nRows = 12;
        }

        //Load the map
        public void loadMap(int[] Mapst)
        {
            
            
            int[] bittable = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };
            for (int x = 0; x < 12; x++)
            {
                BitVector32 vec = new BitVector32 (Mapst[x]);
                for (int y = 0; y < 12; y++)
                {
                    
                    switch (vec[bittable[y]])
                    {
                        case true:
                            this.Add(new Tile("floor", true, y , x ));
                            break;
                        case false:
                            this.Add(new Tile("wall", false, y , x ));
                            break;
                    }
                }
            }           
        }
        

        //Check if the given coordinates are within the bounds of the map
        private bool isWithinBounds(int x, int y)
        {
            if (x >= 0 && x < nCols && y >= 0 && y < nRows)
                return true;
            else
                return false;
        }

        private bool isWall(int x, int y)
        {
            if (this[x+y*nCols].Walkable)
                return true;
            else
                return false;
        }


        public bool isWalkable(int x, int y)
        {
            return (isWithinBounds(x, y) && isWall(x, y));
        }
    }
}
