﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutputMapGrid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor)
        {
            AstarPath path = GetComponent<AstarPath>();
            if (path != null)
            {
                Transform startPoint = GameObject.Find("StartPoint").transform;
                var node = path.GetNearest(startPoint.position).node;
                List<Pathfinding.GraphNode> reachableNodes = Pathfinding.PathUtilities.GetReachableNodes(node);

                Scene scene = SceneManager.GetActiveScene();

                using (BinaryWriter writer = new BinaryWriter(File.Open(@".\Maps\" + scene.name + ".umap", FileMode.Create)))
                {
                    writer.Write(path.data.gridGraph.width);
                    writer.Write(path.data.gridGraph.depth);
                    for (int y = path.data.gridGraph.depth - 1; y >= 0; y--)
                        for (int x = 0; x < path.data.gridGraph.width; x++)
                        {
                            bool walkable = path.data.gridGraph.nodes[path.data.gridGraph.width * y + x].Walkable && reachableNodes.Contains(path.data.gridGraph.nodes[path.data.gridGraph.width * y + x]);

                            writer.Write(walkable);
                            if (walkable)
                            {
                                writer.Write(path.data.gridGraph.nodes[path.data.gridGraph.width * y + x].position.x / 1000f);
                                writer.Write(path.data.gridGraph.nodes[path.data.gridGraph.width * y + x].position.y / 1000f);
                                writer.Write(path.data.gridGraph.nodes[path.data.gridGraph.width * y + x].position.z / 1000f);
                                //Debug.Log("saving x: " + path.data.gridGraph.nodes[path.data.gridGraph.width * y + x].position.x / 1000f + ", y: " + path.data.gridGraph.nodes[path.data.gridGraph.width * y + x].position.y / 1000f + ", z: " + path.data.gridGraph.nodes[path.data.gridGraph.width * y + x].position.z / 1000f);
                            }
                        }
                }
            }

        }
    }
}
