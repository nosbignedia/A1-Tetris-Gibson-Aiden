using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{

    //public List<GameObject> tetrominoes;
    public GameObject[] tetrominoPrefabs;
    private TetrisGrid grid;
    private GameObject nextPiece;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
        if (grid == null)
        {
            Debug.LogError("No Grid");
            return;

        }
        //spawn initial piece here
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        //cal the center of grid
        Vector3 spawnPostition = new Vector3(
            Mathf.Floor(grid.width / 2),    //x
            grid.height,                //y
            0);                             //z

        if(nextPiece != null)
        {
            nextPiece.SetActive(true);
            nextPiece.transform.position = spawnPostition;
        }
        else
        {
            //spawn random piece
            nextPiece = InstantiateRandomPiece();
            nextPiece.transform.position = spawnPostition;
        }
        nextPiece = InstantiateRandomPiece();
        nextPiece.SetActive(false);

    }

    private GameObject InstantiateRandomPiece()
    {
        int index = Random.Range(0,tetrominoPrefabs.Length);
        return Instantiate(tetrominoPrefabs[index]);
    }

}
