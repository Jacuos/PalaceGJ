﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    // pacz w ustawienia tekstury!
    public Texture2D mapBlueprint;
    public bool spawnInLocalCoordinates;
    public MapEntry[] mapInput;

    private Dictionary<string, GameObject> mapDict;
    private Rect mapBounds;

    void Start()
    {
        GenerateDict();
        GenerateMap();
        mapBounds = new Rect(0, 0, mapBlueprint.width, mapBlueprint.height);
    }

    private void GenerateDict()
    {
        mapDict = new Dictionary<string, GameObject>();
        foreach (MapEntry entry in mapInput)
            mapDict.Add(toColorString(entry.color), entry.prefab);
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapBlueprint.width; x++)
            for (int y = 0; y < mapBlueprint.height; y++)
                spawnCube(x, y);
    }

    private void spawnCube(int x, int y)
    {
        Color cubeCode = mapBlueprint.GetPixel(x, y);
        
        if (mapDict.ContainsKey(toColorString(cubeCode)))
        {
            Vector3 position = new Vector3(x, y,0);
            GameObject tile = Instantiate(mapDict[toColorString(cubeCode)], this.transform.localPosition + position, Quaternion.identity, this.transform);
            tile.name = string.Format("[{0}:{1}] - {2}", x, y, tile.name);
        }
    }

    private string toColorString(Color color)
    {
        return color.r.ToString("F") + color.g.ToString("F") + color.b.ToString("F");
    }


    public bool canPass(Vector3 point)
    {
        if (mapBounds.Contains(point))
            return mapBlueprint.GetPixel((int)point.x, (int)point.y).r >= 0.5;
        Debug.Log(string.Format("there is nowhere to go anymore [{0}:{1}]", (int)point.x, (int)point.y));
        return false;
    }


	public bool canSee(Vector3 point)
    {
        if (mapBounds.Contains(point))
            return mapBlueprint.GetPixel((int)point.x, (int)point.y).g.Equals(1f);
        Debug.Log(string.Format("there is nothing to see anymore [{0}:{1}]", (int)point.x, (int)point.y));
        return false;
    }

}
