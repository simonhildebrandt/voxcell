using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;


public class LevelManager : MonoBehaviour {

    [Serializable]
    public struct Tile {
        public Vector3Int position;

        public Tile(Vector3Int p) {
            position = p;
        }
    }

    public struct Face {
        public Vector3Int position;
        public Vector3Int direction;

        public Face(Vector3Int p, Vector3Int d) {
            position = p;
            direction = d;
        }

        public Vector3 centre() {
            return (Vector3)position + ((Vector3)direction / 2);
        }

        public Quaternion rotation() {
            return Quaternion.FromToRotation(Vector3.forward, direction);
        }
    }

    public Vector3Int[] directions = {
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 0, 1)
    };

    [SerializeField]
    public List<Tile> tiles = new List<Tile>();

    public void AddBlock(Vector3Int position) {
        Tile tile = new Tile(position);
        tiles.Add(tile);
        //tileLookup[position] = tile;
    }

    public void RemoveBlock(Vector3Int position) {
        int index = tiles.FindIndex(i => i.position == position);
        if (index >= 0) {
            tiles.RemoveAt(index);
        }
    }

    public void EnsureBlock() {
        Debug.Log("Adding block");
        if (tiles.Count == 0) {
            AddBlock(Vector3Int.zero);
        }
    }

    public IEnumerable<Face> getFaces() {
        Dictionary<Vector3Int, Tile> tileLookup = new Dictionary<Vector3Int, Tile>();

        foreach(Tile tile in tiles) {
            tileLookup[tile.position] = tile;
        }

        foreach(Tile tile in tiles) {
            foreach(Vector3Int direction in directions) {
                if (!tileLookup.ContainsKey(tile.position + direction)) {
                    yield return new Face(tile.position, direction);
                }
            }
        }
    }

    void OnDrawGizmos() {
        // if (ToolManager.activeToolType == typeof(LevelTool)) {
            Gizmos.color = new Color(.5f, .5f, .5f, .5f);
            Gizmos.matrix = transform.localToWorldMatrix;

            foreach (Tile tile in tiles) {
              Gizmos.DrawCube(tile.position, Vector3.one * .9f);
            }
        // }
    }
}
