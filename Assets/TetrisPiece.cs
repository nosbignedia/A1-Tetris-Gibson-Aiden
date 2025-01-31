using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    private TetrisGrid grid;
    private float dropTimer;
    private float dropInterval = 1;
    private bool isLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
        dropTimer = dropInterval;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAutomaticDrop();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
           RotatePiece();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;

        if(!IsValidPosition())
        {
            transform.position -= direction; // back if not valid

            if(direction == Vector3.down)
            {
                LockPiece();
                Debug.Log("LockPiece");
            }
        }

    }

    private void RotatePiece()
    {
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        transform.Rotate(0, 0, -90);


        if(!IsValidPosition()) 
        {
         
            if(!TryWallKick(originalPosition, originalRotation))
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                Debug.Log("rotation invalid, reverting back to original state");
            }
            else
            {
                Debug.Log("Did a flip");
            }
            
        }
    }

    private bool TryWallKick(Vector3 originalPosition, Quaternion originalRoatation)
    {

        Vector2Int[] wallKickOffsets = new Vector2Int[]
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,-1),

            new Vector2Int(2,0),
            new Vector2Int(-2,0),
            new Vector2Int(0,-2),
            new Vector2Int(2,-1),
            new Vector2Int(-2,-1),
            new Vector2Int(2,-2),
            new Vector2Int(-2,-2),


            new Vector2Int(3,0),
            new Vector2Int(-3,0),
            new Vector2Int(0,-3),
            new Vector2Int(3,-1),
            new Vector2Int(-3,-1),
            new Vector2Int(3,-2),
            new Vector2Int(-3,-2),
            new Vector2Int(3,-3),
            new Vector2Int(-3,-3),

        };

        foreach (var offset in  wallKickOffsets)
        {

            transform.position += (Vector3Int)offset;

            if (IsValidPosition())
            {
                return true;
            }

            transform.position -= (Vector3Int)offset;
        }

        return false;
    }

    private void HardDrop()
    {
        do
        {
            Move(Vector3.down);

        } while (!isLocked);
    }
    private bool IsValidPosition()
    {
        Debug.Log("isValidPos");
        foreach(Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);

            if(grid.IsCellOccupied(position))
            {
                return false;

            }
        }
        return true;
    } 

    private void HandleAutomaticDrop()
    {
        dropTimer -= Time.deltaTime;

        if(dropTimer <= 0)
        {
            Move(Vector3.down);
            dropTimer = dropInterval;
        }
    }

    private void LockPiece()
    {
        isLocked = true;
        foreach(Transform block in transform) 
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);   
            grid.AddBlockToGrid(block, position);
        }

        grid.ClearFullLines();
        if(FindObjectOfType<TetrisSpawner>())
        {
            FindObjectOfType<TetrisSpawner>().SpawnPiece();
            Debug.Log("Spanw");
        }
        
        Destroy(this);

    }
   

}
