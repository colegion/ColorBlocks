using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Utilities
{
    public static class JsonReader
    {
        public static LevelInfo ReadJSon(string fileName)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        
            if (jsonFile != null)
            {
                string jsonData = jsonFile.text;
                LevelInfo levelInfo = JsonConvert.DeserializeObject<LevelInfo>(jsonData);
                LogLevelAttributes(levelInfo);
                return levelInfo;
            }
            else
            {
                Debug.LogError("Failed to load JSON file.");
            }

            return new LevelInfo();
        }

        private static void LogLevelAttributes(LevelInfo levelInfo)
        {
            Debug.Log("MoveLimit: " + levelInfo.moveLimit);
            Debug.Log("RowCount: " + levelInfo.rowCount);
            Debug.Log("ColCount: " + levelInfo.columnCount);
                
            foreach (var cell in levelInfo.cellInfo)
            {
                Debug.Log("Cell - Row: " + cell.row + ", Col: " + cell.column);
            }

            foreach (var movable in levelInfo.movableInfo)
            {
                Debug.Log("Movable - Row: " + movable.row +
                          ", Col: " + movable.column + 
                          "\n Color: " + movable.color +
                          "\n Directions: " + movable.directions[0] +
                          "\n Length" + movable.length);
            }
                
            foreach (var exit in levelInfo.exitInfo)
            {
                Debug.Log("Movable - Row: " + exit.row +
                          ", Col: " + exit.column + 
                          "\n Color: " + exit.color +
                          "\n Directions: " + exit.direction);
            }
        }
    }
}
