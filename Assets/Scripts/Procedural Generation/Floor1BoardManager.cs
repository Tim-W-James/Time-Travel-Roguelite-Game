using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;      //tells Random to use the Unity Engine random number generator

public class Floor1BoardManager : MonoBehaviour {
    
    //public paramater variables
    public int totalColumns = 8;                                        //number of columns in our game board.
    public int totalRows = 8;                                           //number of rows in our game board.
    public int totalRoomsMin = 8;                                       //min amount of rooms to spawn
    public int totalRoomsMax = 20;                                      //max amount of rooms to spawn
    public GameObject grid;                                             //grid reference for rooms to spawn on
    
    //room type prefabs
    public GameObject emptySpaceRoom;   //fills in blank spaces
    public GameObject exitRoom;         //player objective
    public GameObject[] startRoom;        //where to player spawns
    public GameObject[] corridorHoriRoom;
    public GameObject[] corridorVertRoom;
    public GameObject[] cornerBottomLeftRoom;
    public GameObject[] cornerBottomRightRoom;
    public GameObject[] cornerTopLeftRoom;
    public GameObject[] cornerTopRightRoom;
    public GameObject[] endRightRoom;
    public GameObject[] endLeftRoom;
    public GameObject[] endBottomRoom;
    public GameObject[] endTopRoom;
    public GameObject[] tLeftRoom;
    public GameObject[] tRightRoom;
    public GameObject[] tUpRoom;
    public GameObject[] tDownRoom;
    public GameObject[] xRoom;
    public GameObject[] treasureRoom;   //must be at least 1 in every level
    public GameObject[] bossRoom;       //must be at least 1 in every level, when beaten leads to exitRoom

    private GameObject[, ] roomArray;   //array room all rooms, used for deletion
    
    private string[, ] gridTypes;       //array of all room types
    private Vector3 offset = new Vector3(0, -0, 0); //offset for placing rooms
    private GameObject tempRoom;    //temp room reference
    private string prevRoom = "";   //tracking for previous room, empty when no rooms created
    private int totalRooms = 0;     //tracks total rooms placed, not including empty rooms
    private bool hasBossRoomSpawned = false;    //only one boss room should spawn
    private bool hasTreasureRoomSpawned = false;//only one treasure room should spawn
    //private Random.State seed;    //tracks the unique seed for the level

    //Clears our list gridPositions and prepares it to generate a new board.
    void RefreshGridTypes()
    {
        //loop through x axis (columns)
        for (int x = 0; x < totalColumns; x++)
        {
            //within each column, loop through y axis (rows)
            for (int y = 0; y < totalRows; y++)
            {
                gridTypes[x, y] = ""; //set grid type to empty for all spaces initially
            }
        }
    }

    string TestForType(int col, int row) //returns type of room at position (col, row)
    {
        string tempString = "";

        tempString = gridTypes[col, row];

        return tempString;
    }

    int TestForConnection(string direction, int col, int row) //tests for a connection in direction of position (col, row) and returns the following values: 0 = not generated, 1 = connection, 2 = blocked
    {
        int connection = 2; //blocked by default
        string tempString = "";

        if (direction == "up") //checks upper connection
        {
            if (row == totalRows) //if at edge limt of grid, return blocked
                connection = 2;
            else if (gridTypes[col, row + 1] == "") //if space empty, return not generated
                connection = 0;
            else
            {
                tempString = TestForType(col, row + 1);

                //if room above has a lower connection, return connected
                if (tempString == "bossRoom" || tempString == "endTopRoom" || tempString == "corridorVertRoom" || tempString == "cornerTopLeftRoom" || tempString == "cornerTopRightRoom" || tempString == "tLeftRoom" || tempString == "tRightRoom" || tempString == "tDownRoom" || tempString == "xRoom")
                    connection = 1;
            }
        }

        else if (direction == "down") //checks lower connection
        {
            if (row == 0) //if at edge limt of grid, return blocked
                connection = 2;
            else if (gridTypes[col, row - 1] == "") //if space empty, return not generated
                connection = 0;
            else
            {
                tempString = TestForType(col, row - 1);

                //if room below has a upper connection, return connected
                if (tempString == "startRoom" || tempString == "endBottomRoom" || tempString == "corridorVertRoom" || tempString == "cornerBottomLeftRoom" || tempString == "cornerBottomRightRoom" || tempString == "tLeftRoom" || tempString == "tRightRoom" || tempString == "tUpRoom" || tempString == "xRoom")
                    connection = 1;
            }
        }

        else if (direction == "right") //checks right connection
        {
            if (col == totalColumns) //if at edge limt of grid, return blocked
                connection = 2;
            else if (gridTypes[col + 1, row] == "") //if space empty, return not generated
                connection = 0;
            else
            {
                tempString = TestForType(col + 1, row);

                //if room to right has a left connection, return connected
                if (tempString == "treasureRoom" || tempString == "endRightRoom" || tempString == "corridorHoriRoom" || tempString == "cornerTopLeftRoom" || tempString == "cornerBottomLeftRoom" || tempString == "tLeftRoom" || tempString == "tDownRoom" || tempString == "tUpRoom" || tempString == "xRoom")
                    connection = 1;
            }
        }

        else if (direction == "left") //checks left connection
        {
            if (col == 0) //if at edge limt of grid, return blocked
                connection = 2;
            else if (gridTypes[col - 1, row] == "") //if space empty, return not generated
                connection = 0;
            else
            {
                tempString = TestForType(col - 1, row);

                //if room to left has a right connection, return connected
                if (tempString == "startRoom" || tempString == "endLeftRoom" || tempString == "corridorHoriRoom" || tempString == "cornerBottomRightRoom" || tempString == "cornerTopRightRoom" || tempString == "tDownRoom" || tempString == "tRightRoom" || tempString == "tUpRoom" || tempString == "xRoom")
                    connection = 1;
            }
        }

        return connection;
    }

    void InstantiateRoom(string roomType, int col, int row) //instantiates room based on type and position
    {
        totalRooms++; //add 1 to room count, will be subtracted if empty room

        //instantiate room of random index in array of room type
        if (roomType == "startRoom")
            tempRoom = startRoom[Random.Range(0, startRoom.Length)];

        else if (roomType == "corridorHoriRoom")
            tempRoom = corridorHoriRoom[Random.Range(0, corridorHoriRoom.Length)];

        else if (roomType == "corridorVertRoom")
            tempRoom = corridorVertRoom[Random.Range(0, corridorVertRoom.Length)];

        else if (roomType == "cornerBottomLeftRoom")
            tempRoom = cornerBottomLeftRoom[Random.Range(0, cornerBottomLeftRoom.Length)];

        else if (roomType == "cornerBottomRightRoom")
            tempRoom = cornerBottomRightRoom[Random.Range(0, cornerBottomRightRoom.Length)];

        else if (roomType == "cornerTopLeftRoom")
            tempRoom = cornerTopLeftRoom[Random.Range(0, cornerTopLeftRoom.Length)];

        else if (roomType == "cornerTopRightRoom")
            tempRoom = cornerTopRightRoom[Random.Range(0, cornerTopRightRoom.Length)];

        else if (roomType == "treasureRoom")
            tempRoom = treasureRoom[Random.Range(0, treasureRoom.Length)];

        else if (roomType == "bossRoom")
            tempRoom = bossRoom[Random.Range(0, bossRoom.Length)];

        else if (roomType == "tLeftRoom")
            tempRoom = tLeftRoom[Random.Range(0, tLeftRoom.Length)];

        else if (roomType == "tRightRoom")
            tempRoom = tRightRoom[Random.Range(0, tRightRoom.Length)];

        else if (roomType == "tUpRoom")
            tempRoom = tUpRoom[Random.Range(0, tUpRoom.Length)];

        else if (roomType == "tDownRoom")
            tempRoom = tDownRoom[Random.Range(0, tDownRoom.Length)];

        else if (roomType == "xRoom")
            tempRoom = xRoom[Random.Range(0, xRoom.Length)];

        else if (roomType == "endTopRoom")
            tempRoom = endTopRoom[Random.Range(0, endTopRoom.Length)];

        else if (roomType == "endBottomRoom")
            tempRoom = endBottomRoom[Random.Range(0, endBottomRoom.Length)];

        else if (roomType == "endLeftRoom")
            tempRoom = endLeftRoom[Random.Range(0, endLeftRoom.Length)];

        else if (roomType == "endRightRoom")
            tempRoom = endRightRoom[Random.Range(0, endRightRoom.Length)];

        else if (roomType == "exitRoom")
            tempRoom = exitRoom;

        else    //check if room name does not match
        {
            totalRooms--; //subtract 1 from room count
            tempRoom = emptySpaceRoom; //use empty space room
        }            

        prevRoom = roomType;
        gridTypes[col, row] = roomType;
        tempRoom = Instantiate(tempRoom, (transform.position + new Vector3((col * 89), (row * 89), 0f) + offset), transform.rotation); //instantiate room of type in position (col, row) multiplied by 70 to scale
        tempRoom.transform.parent = grid.transform; //make child of grid
        tempRoom.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);  //set room scale

        roomArray[col, row] = tempRoom; //add to array of rooms
    }

    void InstantiateRoomOfConnection(int connectionDown, int connectionUp, int connectionRight, int connectionLeft, int col, int row) //instantiates room based on connections and col/row position
    {
        /*
         *The algorithm will go through every case of four directional connections at the position (col, row), where each 
         *connection can be non generated (0), found (1) or blocked (2), and places a random room of those with respective connections
         */

        //below cases should occur on first generation pass
        if (connectionDown == 1 && connectionLeft == 2 && connectionRight == 0 && connectionUp == 0) //down: connected, left: blocked, right: not generated, up: not generated
        {
            int tempRange = Random.Range(0, 5);

            if (tempRange == 0)
                InstantiateRoom("tRightRoom", col, row);
            else if (tempRange == 1)
                InstantiateRoom("cornerTopRightRoom", col, row);
            else if (tempRange == 2 && !hasBossRoomSpawned && totalRooms > totalRoomsMin)
            {
                InstantiateRoom("bossRoom", col, row);
                hasBossRoomSpawned = true;
            }
            else if (tempRange == 3 && totalRooms > totalRoomsMin)
                InstantiateRoom("endTopRoom", col, row);
            else
                InstantiateRoom("corridorVertRoom", col, row);
        }

        else if (connectionDown == 1 && connectionLeft == 2 && connectionRight == 0 && connectionUp == 2) //down: connected, left: blocked, right: not generated, up: blocked
            InstantiateRoom("cornerTopRightRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 1 && connectionRight == 0 && connectionUp == 0) //down: blocked, left: connected, right: not generated, up: not generated
        {
            int tempRange = Random.Range(0, 5);

            if (tempRange == 0)
                InstantiateRoom("tUpRoom", col, row);
            else if (tempRange == 1)
                InstantiateRoom("cornerBottomLeftRoom", col, row);
            else if (tempRange == 2 && !hasTreasureRoomSpawned && totalRooms > totalRoomsMin)
            {
                InstantiateRoom("treasureRoom", col, row);
                hasTreasureRoomSpawned = true;
            }
            else if (tempRange == 1 && totalRooms > totalRoomsMin)
                InstantiateRoom("endRightRoom", col, row);
            else
                InstantiateRoom("corridorHoriRoom", col, row);
        }

        else if (connectionDown == 2 && connectionLeft == 2 && connectionRight == 1 && connectionUp == 0) //down: blocked, left: blocked, right: connected, up: not generated
            InstantiateRoom("cornerBottomRightRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 1 && connectionRight == 0 && connectionUp == 2) //down: blocked, left: connected, right: not generated, up: blocked
        {
            int tempRange = Random.Range(0, 2);
            
            if (tempRange == 0 && !hasTreasureRoomSpawned && totalRooms > totalRoomsMin)
            {
                InstantiateRoom("treasureRoom", col, row);
                hasTreasureRoomSpawned = true;
            }
            else if (tempRange == 1 && totalRooms > totalRoomsMin)
                InstantiateRoom("endRightRoom", col, row);
            else
                InstantiateRoom("corridorHoriRoom", col, row);
        }

        else if (connectionDown == 1 && connectionLeft == 1 && connectionRight == 0 && connectionUp == 2) //down: connected, left: connected, right: not generated, up: blocked
            InstantiateRoom("tDownRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 1 && connectionRight == 0 && connectionUp == 0) //down: connected, left: connected, right: not generated, up: not generated
        {
            int tempRange = Random.Range(0, 3);

            if (tempRange == 0)
                InstantiateRoom("tLeftRoom", col, row);
            else if (tempRange == 1)
                InstantiateRoom("tDownRoom", col, row);
            else if (tempRange == 2)
                InstantiateRoom("cornerTopLeftRoom", col, row);
            else
                InstantiateRoom("xRoom", col, row);
        }

        else if (connectionDown == 2 && connectionLeft == 1 && connectionRight == 2 && connectionUp == 0) //down: blocked, left: connected, right: blocked, up: not generated
            InstantiateRoom("cornerBottomLeftRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 2 && connectionRight == 2 && connectionUp == 0) //down: connection, left: blocked, right: blocked, up: not generated
        {
            int tempRange = Random.Range(0, 2);

            if (tempRange == 0 && !hasBossRoomSpawned && totalRooms > totalRoomsMin)
            {
                InstantiateRoom("bossRoom", col, row);
                hasBossRoomSpawned = true;
            }
            else if (tempRange == 1 && totalRooms > totalRoomsMin)
                InstantiateRoom("endTopRoom", col, row);
            else
                InstantiateRoom("corridorVertRoom", col, row);
        }

        else if (connectionDown == 1 && connectionLeft == 1 && connectionRight == 2 && connectionUp == 0) //down: connected, left: connected, right: blocked, up: not generated
        {
            int tempRange = Random.Range(0, 2);

            if (tempRange == 0)
                InstantiateRoom("tLeftRoom", col, row);
            else
                InstantiateRoom("cornerTopLeftRoom", col, row);
        }

        else if (connectionDown == 1 && connectionLeft == 1 && connectionRight == 2 && connectionUp == 2) //down: connected, left: connected, right: blocked, up: blocked
            InstantiateRoom("cornerTopLeftRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 2 && connectionRight == 2 && connectionUp == 2) //down: connected, left: blocked, right: blocked, up: blocked
            InstantiateRoom("endTopRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 1 && connectionRight == 2 && connectionUp == 2) //down: blocked, left: connected, right: blocked, up: blocked
        {
            if (!hasTreasureRoomSpawned && totalRooms > totalRoomsMin)
            {
                InstantiateRoom("treasureRoom", col, row);
                hasTreasureRoomSpawned = true;
            }
            else
                InstantiateRoom("endRightRoom", col, row);
        }

        //below cases should not occur on first generation pass, but my appear for rooms being refreshed
        else if (connectionDown == 2 && connectionLeft == 1 && connectionRight == 2 && connectionUp == 1) //down: blocked, left: connected, right: blocked, up: connected
            InstantiateRoom("cornerDownLeftRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 2 && connectionRight == 2 && connectionUp == 1) //down: connected, left: blocked, right: blocked, up: connected
            InstantiateRoom("corridorVertRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 2 && connectionRight == 2 && connectionUp == 1) //down: blocked, left: blocked, right: blocked, up: connected
            InstantiateRoom("endDownRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 1 && connectionRight == 2 && connectionUp == 1) //down: connected, left: connected, right: blocked, up: connected
            InstantiateRoom("tLeftRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 1 && connectionRight == 1 && connectionUp == 1) //down: connected, left: connected, right: connected, up: connected
            InstantiateRoom("xRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 2 && connectionRight == 1 && connectionUp == 2) //down: blocked, left: blocked, right: connected, up: blocked
            InstantiateRoom("endLeftRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 2 && connectionRight == 1 && connectionUp == 1) //down: blocked, left: blocked, right: connected, up: connected
            InstantiateRoom("cornerDownRightRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 1 && connectionRight == 1 && connectionUp == 1) //down: blocked, left: connected, right: connected, up: connected
            InstantiateRoom("tUpRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 2 && connectionRight == 1 && connectionUp == 1) //down: connected, left: blocked, right: connected, up: connected
            InstantiateRoom("tRightRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 2 && connectionRight == 1 && connectionUp == 2) //down: connected, left: blocked, right: blocked, up: connected
            InstantiateRoom("cornerTopRightRoom", col, row);

        else if (connectionDown == 1 && connectionLeft == 1 && connectionRight == 1 && connectionUp == 2) //down: connected, left: connected, right: connected, up: blocked
            InstantiateRoom("tDownRoom", col, row);

        else if (connectionDown == 2 && connectionLeft == 1 && connectionRight == 1 && connectionUp == 2) //down: blocked, left: connected, right: connected, up: blocked
            InstantiateRoom("corridorHoriRoom", col, row);

        else    //if otherwise cannot find any valid connections                                           //down: blocked, left: blocked, right: blocked, up: blocked
            InstantiateRoom("emptySpaceRoom", col, row);
    }
    
    void PopulateBoard() //sets up and populates the game board
    {
        //loop along x axis
        for (int col = 0; col < totalColumns + 1; col++)
        {
            //Loop along y axis
            for (int row = 0; row < totalRows + 1; row++)
            {
                if (prevRoom == "") //checks for first room placed
                    InstantiateRoom("startRoom", col, row); //places start room

                else if (prevRoom == "bossRoom") //checks for boss room
                {
                    InstantiateRoom("exitRoom", col, row); //places exit room

                    if (TestForConnection("left", col, row) == 1) //checks if connection to left broken
                    {
                        GameObject.Destroy(roomArray[col - 1, row]); //destroy old room
                        //reload room
                        InstantiateRoomOfConnection(TestForConnection("down", col - 1, row), TestForConnection("up", col - 1, row), TestForConnection("right", col - 1, row), TestForConnection("left", col - 1, row), col - 1, row);
                    }
                }                    
                else //generate room based on connections and position (col, row)
                    InstantiateRoomOfConnection(TestForConnection("down", col, row), TestForConnection("up", col, row), TestForConnection("right", col, row), TestForConnection("left", col, row), col, row);
             }
        }
    }

    void ResetBoard() //resets the board
    {
        foreach (Transform child in grid.transform) //destroys all prefabs in board
            GameObject.Destroy(child.gameObject);

        //reset variables
        hasBossRoomSpawned = false;
        hasTreasureRoomSpawned = false;
        totalRooms = 0;
        prevRoom = "";
        gridTypes = new string[totalColumns + 1, totalRows + 1];
        roomArray = new GameObject[totalColumns + 1, totalRows + 1];
    }

    // Use this for initialization
    void Start()
    {
        //seed = Random.state; //set seed
        
        gridTypes = new string[totalColumns + 1, totalRows + 1];      //allocate grid type array
        roomArray = new GameObject[totalColumns + 1, totalRows + 1];  //allocate room object array
        RefreshGridTypes();
        PopulateBoard();
        
        //checks if generated level meets requirements, if not reloads for a max of 8 times
        int maxLoops = 0;
        while ((hasBossRoomSpawned == false || hasTreasureRoomSpawned == false || totalRooms > totalRoomsMax || totalRooms < totalRoomsMin) && maxLoops < 10)
        {
            ResetBoard();
            RefreshGridTypes();
            PopulateBoard();
            maxLoops++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) //check if L key pressed [DEBUG]
        {
            FindObjectOfType<AudioManager>().Play("Reload"); //play sound
            ResetBoard();       //reload level
            RefreshGridTypes();
            PopulateBoard();            
        }
    }
}
