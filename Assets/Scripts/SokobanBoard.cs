using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class SokobanBoard : MonoBehaviour
{
    internal const int BOARD_WIDTH = 10;
    internal const int BOARD_LENGTH = 10;

    [FormerlySerializedAs("boardCubeOddPrefab")] [FormerlySerializedAs("boardCubePrefab")]
    public BoardCube floorCubeOddPrefab;

    [FormerlySerializedAs("boardCubeEvenPrefab")]
    public BoardCube floorCubeEvenPrefab;

    public Camera mainCamera;
    private int NUM_BOXES = 3;
    public SokobanBlock boxPrefab;
    private HashSet<Vector2Int> _emptySpots;
    
    public PlayerObject playerPrefab;
    private HashSet<Vector2Int> boxReceptacles;
    [FormerlySerializedAs("boxSlotPrefab")] 
    public BoxReceptacle boxReceptacle;
    private HashSet<Vector2Int> corners;
    private SokobanGameManager _sokobanGameManager;
    internal PlayerObject thisPlayer;
    internal Dictionary<Vector2Int, SokobanBlock> _blockPositions;
    internal const int BLOCK_ELEVATION = 1;


    public void Initialize(SokobanGameManager sokobanGameManager)
    {
        _sokobanGameManager = sokobanGameManager;
        
        InitializeEmptySpots();
        CalculateCorners();

        AllocateBoxSlots();
        GenerateFloorCubes();
        GenerateBoxes();
        GeneratePlayer();
    }

    private void CalculateCorners()
    {
        corners = new HashSet<Vector2Int>
        {
            new(0, 0),
            new(0, BOARD_LENGTH - 1),
            new(BOARD_WIDTH - 1, 0),
            new(BOARD_WIDTH - 1, BOARD_LENGTH - 1)
        };
    }

    private void InitializeEmptySpots()
    {
        _emptySpots = new HashSet<Vector2Int>();
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                AddEmptySlot(i, j);
            }
        }
    }

    private void AddEmptySlot(int i, int j)
    {
        _emptySpots.Add(new Vector2Int(i, j));
    }

    private void AllocateBoxSlots()
    {
        boxReceptacles = GetRandomEmptySlots(numSlots: NUM_BOXES);
    }

    private HashSet<Vector2Int> GetRandomEmptySlots(int numSlots)
    {
        return _emptySpots.GetRandomN(3);
    }

    private void GenerateBoxes()
    {
        _blockPositions = new Dictionary<Vector2Int, SokobanBlock>();
        for (int i = 0; i < NUM_BOXES; i++)
        {
            Vector2Int vec = GetRandomEmptySlot(useBoxSlots: false, ignoreCorners:true);
            GenerateBox(vec.x, vec.y);
        }
    }

    private void GenerateFloorCubes()
    {
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                GenerateFloorCube(i, j);
            }
        }
    }

    private Vector2Int GetRandomEmptySlot(bool useBoxSlots = true, bool ignoreCorners = false)
    {
        HashSet<Vector2Int> theseSlots = new(_emptySpots);
        if (!useBoxSlots)
        {
            theseSlots.RemoveWhere(x => boxReceptacles.Contains(x));
        }

        if (ignoreCorners)
        {
            theseSlots.RemoveWhere(x => corners.Contains(x));
        }

        if (theseSlots.Count == 0)
        {
            throw new EmptyCollectionException("Unable to generate an empty slot!");
        }

        return theseSlots.GetRandom();
    }

    private void GeneratePlayer()
    {
        Vector2Int vec = GetRandomEmptySlot(useBoxSlots: true);
        thisPlayer = Instantiate(playerPrefab, new Vector3(vec.x, 1, vec.y), Quaternion.identity);
        Vector2Int playerPosition = new Vector2Int(vec.x, vec.y);
        RemoveEmptySlot(playerPosition);

        thisPlayer.Init(_sokobanGameManager, playerPosition);
    }

    internal void RemoveEmptySlot(Vector2Int playerPosition)
    {
        _emptySpots.Remove(playerPosition);
    }

    private void GenerateBox(int i, int j)
    {
        SokobanBlock newPrefab = Instantiate(boxPrefab, new Vector3(i, BLOCK_ELEVATION, j), Quaternion.identity);
        newPrefab.InitBlock(i, j, _sokobanGameManager);
    }

    public void RemoveEmptySlot(int i, int j)
    {
        RemoveEmptySlot(new Vector2Int(i, j));
    }


    private void GenerateFloorCube(int i, int j)
    {
        if (boxReceptacles.Contains(new Vector2Int(i, j)))
        {
            Instantiate(boxReceptacle.gameObject, new Vector3(i, 0, j), Quaternion.identity, transform);
            return;
        }

        var prefabToGenerate = (i + j).IsOdd() ? floorCubeOddPrefab.gameObject : floorCubeEvenPrefab.gameObject;
        Instantiate(prefabToGenerate, new Vector3(i, 0, j), Quaternion.identity, transform);
    }

    public void AddEmptySlot(Vector2Int oldPosition)
    {
        AddEmptySlot(oldPosition.x, oldPosition.y);
    }

    public bool PositionHasBlock(Vector2Int newPosition)
    {
        return _blockPositions.Keys.Contains(newPosition);
    }

    public Vector2Int GetPositionRelativeToInDirection(Vector2Int newPosition, PlayerDirection directionFromKeyCode)
    {
        return newPosition + SokobanGameManager.DIRECTION_VECTOR_MAPPING[directionFromKeyCode];
    }

    public static bool PositionIsOnBoard(Vector2Int somePosition)
    {
        return somePosition.x.InRange(lowerBoundInclusive: 0, upperBoundExclusive: BOARD_WIDTH) &&
               somePosition.y.InRange(lowerBoundInclusive: 0, upperBoundExclusive: BOARD_LENGTH);
    }

    public SokobanBlock GetBlock(Vector2Int blockPosition)
    {
        return _blockPositions[blockPosition];
    }

    public static Vector2Int GetVectorInDirection(PlayerDirection dir)
    {
        return SokobanGameManager.DIRECTION_VECTOR_MAPPING[dir];
    }
}