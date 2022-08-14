using UnityEngine;

public class SokobanMovable : MonoBehaviour
{
    public SVector2Int currentGridPosition
    {
        get;
        private set;
    }

    protected internal virtual void UpdateMovement(float deltaTime, SVector2Int oldPosition, SVector2Int newPosition, float
        objElevation)
    {
        transform.position = Vector3.Lerp(new Vector3(oldPosition.x, objElevation, oldPosition.y),
            new Vector3(newPosition.x, objElevation, newPosition.y),
            deltaTime);
    }
    
    private protected SokobanBoard sokobanBoard => gameManager.sokobanBoard;
    
    private protected SokobanGameManager gameManager;

    public virtual void Init(SokobanGameManager sokobanGameManager, SVector2Int playerPosition)
    {
        gameManager = sokobanGameManager;
        SetSokobanPosition(playerPosition, null, MovementSetType.INSTANTIATE);
    }


    public virtual void SetSokobanPosition(SVector2Int positionToSet, SVector2Int previousPosition,
        MovementSetType setType)
    {
        currentGridPosition = positionToSet;
    }

}