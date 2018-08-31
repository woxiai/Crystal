using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crystal.Astar;

public class TestAstarCell : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var startCell = new AstarCell(0, 0, true);
        var endCell =new AstarCell(3, 4, true);
        AstarCell[,] astarCells = new AstarCell[,] {
            { startCell,                 new AstarCell(0, 1, false), new AstarCell(0, 2, true), new AstarCell(0, 3, true), new AstarCell(0, 4, true)},
            { new AstarCell(1, 0, true), new AstarCell(1, 1, true), new AstarCell(1, 2, true), new AstarCell(1, 3, true), new AstarCell(1, 4, true)},
            { new AstarCell(2, 0, true), new AstarCell(2, 1, false), new AstarCell(2, 2, false), new AstarCell(2, 3, true), new AstarCell(2, 4, false)},
            { new AstarCell(3, 0, true), new AstarCell(3, 1, false), new AstarCell(3, 2, false), new AstarCell(3, 3, true), endCell},
            { new AstarCell(4, 0, true), new AstarCell(4, 1, true), new AstarCell(4, 2, false), new AstarCell(4, 3, true), new AstarCell(4, 4, true)},
        };

        AstarMap am = new AstarMap(astarCells, 5, 5, startCell, endCell);
        MyAstar myAstar = new MyAstar();
        var find = myAstar.Find(am);
        if (find)
        {
            myAstar.PrintMap(endCell);
        } else
        {
            Debug.Log("Not Found Path.");
        }
      
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class MyAstar : AbsAstar { }
