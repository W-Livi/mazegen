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
    private GameObject cell_start;
    private GameObject cell_end;

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
        punch_holes();
        ConstructMaze();
        place_ends();
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

        mazegen_xsplit(0, maze_x_size - 1, 0, maze_z_size - 1);
    }

    private void punch_holes() {
        rem_wall(0, 0, SOUTH);
        rem_wall(maze_x_size - 1, 0, SOUTH);
    }

    private void place_ends() {
        cell_start = Instantiate(cell_prefabs[EAST|SOUTH|WEST]) as GameObject;
        cell_start.transform.SetParent(maze_parent.transform);
        Vector3 pos = new Vector3(0 * cell_x_offset, 0, -1 * cell_z_offset);
        cell_start.transform.position = pos;

        cell_end = Instantiate(cell_prefabs[EAST|SOUTH|WEST]) as GameObject;
        cell_end.transform.SetParent(maze_parent.transform);
        pos = new Vector3((maze_x_size-1) * cell_x_offset, 0, -1 * cell_z_offset);
        cell_end.transform.position = pos;
    }    

    private void mazegen_xsplit(int xmin, int xmax, int zmin, int zmax) {
        if (xmin >= xmax || zmin >= zmax) return;

        int xsplit = Random.Range(xmin, xmax - 1);
        int zgap = Random.Range(zmin, zmax - 1);

        for (int z = zmin; z <= zmax; ++z) {
            if (z == zgap) continue;
            add_wall(xsplit, z, EAST);
            add_wall(xsplit + 1, z, WEST);
        }

        mazegen_zsplit(xmin, xsplit, zmin, zmax);
        mazegen_zsplit(xsplit + 1, xmax, zmin, zmax);
    }

    private void mazegen_zsplit(int xmin, int xmax, int zmin, int zmax) {
        if (xmin >= xmax || zmin >= zmax) return;

        int zsplit = Random.Range(zmin, zmax);
        int xgap = Random.Range(xmin, xmax);

        for (int x = xmin; x <= xmax; ++x) {
            if (x == xgap) continue;
            add_wall(x, zsplit, NORTH);
            add_wall(x, zsplit+1, SOUTH);
        }

        mazegen_xsplit(xmin, xmax, zmin, zsplit);
        mazegen_xsplit(xmin, xmax, zsplit+1, zmax);
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
