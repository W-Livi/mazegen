using UnityEngine;
using System.Collections;

public class MazeGen : MonoBehaviour {

    public GameObject maze_parent; // so the cells can live somewhere organized
    public GameObject[] cell_prefabs; // our building blocks
    public int cell_x_offset;
    public int cell_z_offset;
    public int maze_x_size;
    public int maze_z_size;

    private int[,] map; // where we design the maze layout
        
    private GameObject[,] cell_instances; // in case we wish to address a cell-object by its coordinates *shrug*

    private static int NORTH = 1;
    private static int EAST = 2;
    private static int SOUTH = 4;
    private static int WEST = 8;


    void Awake()
    {
        map = new int[maze_x_size, maze_z_size];
        cell_instances = new GameObject[maze_x_size, maze_z_size];
        initmap();
        DesignMaze();
        ConstructMaze();
    }

    private int add_wall(int cell, int wall)
    {
        int newcell = cell | wall;
        return newcell;
    }

    private int rem_wall(int cell, int wall)
    {
        int mask = ~wall;
        int newcell = cell & mask;
        return newcell;
    }

    private void add_wall(int x, int z, int wall)
    {
        map[x, z] = add_wall(map[x, z], wall);
    }

    private void rem_wall(int x, int z, int wall)
    {
        map[x, z] = rem_wall(map[x, z], wall);
    }

    private void initmap()
    {
        for (int x = 0; x < maze_x_size; ++x)
        {
            for (int z = 0; z < maze_z_size; ++z)
            {
                map[x, z] = 0;
            }
        }
    }

    private void DesignMaze()
    {
        // zero-fill our map
        initmap();

        // fill in the outer walls
        for (int x = 0; x < maze_x_size; ++x)
        {
            add_wall(x, 0, SOUTH);
            add_wall(x, maze_z_size-1, NORTH);
        }
        for (int z = 0; z < maze_z_size; ++z)
        {
            add_wall(0, z, WEST);
            add_wall(maze_x_size-1, z, EAST);
        }

        // TODO implement maze generation algorithm
    }

    private void ConstructMaze()
    {
        for (int x = 0; x < maze_x_size; ++x)
        {
            for (int z = 0; z < maze_z_size; ++z)
            {
                GameObject go = Instantiate(cell_prefabs[map[x, z]]) as GameObject;
                cell_instances[x, z] = go;
                go.transform.SetParent(maze_parent.transform);

                Vector3 pos = new Vector3(x * cell_x_offset, 0, z * cell_z_offset);

                go.transform.position = pos;
            }
        }
    }



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
