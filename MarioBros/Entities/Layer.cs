using System.Collections.Generic;

namespace MarioBros.Entities
{
    public class Layer
    {
        #region Properties

        public List<int> Data { get; set; }
        public int Height { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Opacity { get; set; }
        public string Type { get; set; }
        public bool Visible { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string DrawOrder { get; set; }
        public List<BaseObject> Objects { get; set; }

        #endregion
    }
}
