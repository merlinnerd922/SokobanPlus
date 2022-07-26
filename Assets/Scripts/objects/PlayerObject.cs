using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class PlayerObject : MonoBehaviour
{
    private SokobanGameManager _gameManager;
    
    
    private Vector2Int? _currentPosition;
    private readonly Dictionary<PlayerDirection, Vector2Int> _directionVectorMapping = new()
    {
        {PlayerDirection.LEFT, Vector2Int.left},
        {PlayerDirection.RIGHT, Vector2Int.right},
        {PlayerDirection.FORWARD, Vector2Int.up},
        {PlayerDirection.BACKWARD, Vector2Int.down}
    };

    private float PLAYER_ELEVATION = 1;


    public IEnumerator Move(PlayerDirection dir)
    {
        _gameManager._gameState = SokobanGameState.IN_MOTION;
        var thisCurrentPosition = GetCurrentPosition();
        Vector2Int oldPosition = thisCurrentPosition;
        Vector2Int newPosition = thisCurrentPosition + GetIncrementFromDirection(dir);

        float deltaTime = 0;
        while (deltaTime < 1)
        {
            UpdateMovement(deltaTime, oldPosition, newPosition);
            deltaTime += Time.deltaTime;
            yield return null;    
        }
        
        SetPlayerPosition(newPosition);
        _gameManager.sokobanBoard.RemoveEmptySlot(newPosition);
        _gameManager.sokobanBoard.AddEmptySlot(oldPosition);
        _gameManager._gameState = SokobanGameState.STATIONARY;
    }

    private Vector2Int GetIncrementFromDirection(PlayerDirection dir)
    {
        return _directionVectorMapping[dir];
    }

    private Vector2Int GetCurrentPosition()
    {
        Debug.Assert(_currentPosition != null, nameof(_currentPosition) + " != null");
        return (Vector2Int) _currentPosition;
    }

    private void UpdateMovement(float deltaTime, Vector2Int oldPosition, Vector2Int newPosition)
    {
        transform.position = Vector3.Lerp(new Vector3(oldPosition.x, PLAYER_ELEVATION, oldPosition.y),
                                            new Vector3(newPosition.x, PLAYER_ELEVATION, newPosition.y),
                                            deltaTime);
    }

    public void Init(SokobanGameManager sokobanGameManager, Vector2Int playerPosition)
    {
        _gameManager = sokobanGameManager;
        SetPlayerPosition(playerPosition);
    }

    private void SetPlayerPosition(Vector2Int playerPosition)
    {
        this._currentPosition = playerPosition;
    }
}