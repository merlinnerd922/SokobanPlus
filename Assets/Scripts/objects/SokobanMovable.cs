using UnityEngine;

public class SokobanMovable : MonoBehaviour
{
    protected internal Vector2Int? _currentGridPosition;
    protected internal void UpdateMovement(float deltaTime, Vector2Int oldPosition, Vector2Int newPosition, float
        objElevation)
    {
        transform.position = Vector3.Lerp(new Vector3(oldPosition.x, objElevation, oldPosition.y),
            new Vector3(newPosition.x, objElevation, newPosition.y),
            deltaTime);
    }
    
    private protected SokobanBoard sokobanBoard => _gameManager.sokobanBoard;
    
    private protected SokobanGameManager _gameManager;
    public void Init(SokobanGameManager sokobanGameManager, Vector2Int playerPosition)
    {
        _gameManager = sokobanGameManager;
        SetSokobanPosition(playerPosition);
    }


    public virtual void SetSokobanPosition(Vector2Int playerPosition)
    {
        _currentGridPosition = playerPosition;
    }

}