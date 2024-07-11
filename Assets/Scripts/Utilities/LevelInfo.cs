using Newtonsoft.Json;

namespace Utilities
{
    [System.Serializable]
    public class LevelInfo
    {
        [JsonProperty("MoveLimit")] public int moveLimit;
        [JsonProperty("RowCount")] public int rowCount;
        [JsonProperty("ColCount")] public int columnCount;
        [JsonProperty("CellInfo")] public CellAttributes[] cellInfo;
        [JsonProperty("MovableInfo")] public MovableAttributes[] movableInfo;
        [JsonProperty("ExitInfo")] public ExitAttributes[] exitInfo;
    }

    [System.Serializable]
    public class CellAttributes
    {
        [JsonProperty("Row")] public int row;
        [JsonProperty("Col")] public int column;

        public CellAttributes(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }

    [System.Serializable]
    public class MovableAttributes
    {
        [JsonProperty("Row")] public int row;
        [JsonProperty("Col")] public int column;
        [JsonProperty("Direction")] public int[] directions;
        [JsonProperty("Length")] public int length;
        [JsonProperty("Colors")] public int color;
    }

    [System.Serializable]
    public class ExitAttributes
    {
        [JsonProperty("Row")] public int row;
        [JsonProperty("Col")] public int column;
        [JsonProperty("Direction")] public int direction;
        [JsonProperty("Colors")] public int color;
    }
}