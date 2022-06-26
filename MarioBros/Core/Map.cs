using System.Collections.Generic;

namespace MarioBros.Core
{
    public class Map
    {
        #region Properties

        public string BackgroundColor { get; set; }
        public int Height { get; set; }
        public bool Infinite { get; set; }
        public List<Layer> Layers { get; set; }
        public int NextLayerId { get; set; }
        public int NextObjectId { get; set; }
        public string Orientation { get; set; }
        public string RenderOrder { get; set; }
        public string TiledVersion { get; set; }
        public int TileHeight { get; set; }
        public List<TileSet> TileSets { get; set; }
        public int TileWidth { get; set; }
        public string Type { get; set; }
        public double Version { get; set; }
        public int Width { get; set; }

        #endregion
    }
}
