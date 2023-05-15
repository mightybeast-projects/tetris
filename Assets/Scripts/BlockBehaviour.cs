using System;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _rotationPivot;

    private static int gridWidth = 5;
    private static int gridHeight = 11;
    private static Transform[,] grid = new Transform[gridWidth * 2 + 1, gridHeight * 2];
    
    private float _previousTime;
    private float _fallTime = 0.5f;

    private BlockSpawner _blockSpawner;
    private ScoreHandler _scoreHandler;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        FallDown();
    }

    public void SetBlockSpawner(BlockSpawner blockSpawner)
    {
        _blockSpawner = blockSpawner;
    }
    
    public void SetScoreHandler(ScoreHandler scoreHandler)
    {
        _scoreHandler = scoreHandler;
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RotateClockwise();
            if(!ValidMove())
                RotateCounterClockwise();
        }
        
        if(Input.GetKeyDown(KeyCode.A)){
            MoveLeft();
            if(!ValidMove())
                MoveRight();
        }
        
        if(Input.GetKeyDown(KeyCode.D)){
            MoveRight();
            if(!ValidMove())
                MoveLeft();
        }
        
        
    }
    
    private void FallDown()
    {
        float currentTime = Time.time;
        if(currentTime - _previousTime > (Input.GetKey(KeyCode.S)? _fallTime / 10 : _fallTime))
        {
            transform.position += new Vector3(0, -0.5f, 0);
            
            if(!ValidMove())
            {
                transform.position += new Vector3(0, 0.5f, 0);
                
                AddToGrid();
                CheckFullLines();
                enabled = false;
                
                _blockSpawner.SpawnBlock();
            }
            
            _previousTime = currentTime;
        }
    }

    private void CheckFullLines()
    {
        for(int i = gridHeight * 2 - 1; i >= 0; i--)
        {
            if(HasFullLine(i))
            {
                DeleteLine(i);
                MoveRowDown(i);
                
                _scoreHandler.AddPoint();
                _scoreHandler.UpdateScoreText();
            }
        }
    }

    private bool HasFullLine(int i)
    {
        for (int j = 0; j < gridWidth * 2; j++)
        {
            if(grid[j, i] == null) return false;
        }
        
        return true;
    }
    
    private void DeleteLine(int i)
    {
        for (int j = 0; j < gridWidth * 2; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }
    
    private void MoveRowDown(int i)
    {
        for(int j = i; j < gridHeight * 2; j++)
        {
            for(int k = 0; k < gridWidth * 2; k++)
            {
                if(grid[k, j] != null)
                {
                    grid[k, j - 1] = grid[k, j];
                    grid[k, j] = null;
                    grid[k, j - 1].position += new Vector3(0, -0.5f, 0);
                }
            }
        }
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            if(children.gameObject.name == "Pivot") continue;
            
            int childrenIndexX = (int) Math.Round(children.position.x * 2);
            int childrenIndexY = (int) Math.Round(children.position.y * 2);
            
            grid[childrenIndexX, childrenIndexY] = children;
        }
    }

    private void RotateClockwise()
    {
        transform.RotateAround(_rotationPivot.position, Vector3.forward, 90);
    }

    private void RotateCounterClockwise()
    {
        transform.RotateAround(_rotationPivot.position, Vector3.forward, -90);
    }

    private void MoveLeft()
    {
        transform.position += new Vector3(-0.5f, 0, 0);
    }

    private void MoveRight()
    {
        transform.position += new Vector3(0.5f, 0, 0);
    }
    
    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            if(children.gameObject.name == "Pivot") continue;
            if (!CheckChildren(children)) return false;
        }

        return true;
    }

    private bool CheckChildren(Transform children)
    {
        float childrenX = (float) Math.Round(children.position.x * 2) / 2;
        float childrenY = (float) Math.Round(children.position.y * 2) / 2;
        
        int childrenIndexX = (int) Math.Round(children.position.x * 2);
        int childrenIndexY = (int) Math.Round(children.position.y * 2);

        if (childrenX < 0 || childrenX > gridWidth || childrenY < 0 || childrenY > gridHeight)
            return false;
        
        if(grid[childrenIndexX, childrenIndexY] != null)
            return false;
        
        return true;
    }
}
