using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public Vector2 minSize;
    public Vector2 maxSize;
    public Vector2 gridSize;
    [Space(10)]
    public Vector2 minDistanceBetweenElements;
    public Vector2 maxDistanceBetweenElements;
    [Space(10)]
    public Transform BackgroundBoard;
    public Vector2 boardmaxScale,boardminScale;
    private float minScale = 0.3f;
    private float maxScale = 0.7f;
    [Space(10)]
    public Color currentColor = Color.white;

    void Start()
    {
        StartCoroutine(startDelay());
        ControllerEvents.Instance.PressedDownButton += downPressed;
        ControllerEvents.Instance.PressedLeftButton += leftPressed;
        ControllerEvents.Instance.PressedRightButton += rightPressed;
        ControllerEvents.Instance.PressedUpButton += upPressed;
        ControllerEvents.Instance.onColorChanged += ChangeColor;
        ControllerEvents.Instance.onSave += SaveGrid;
    }

    
    public void createGrid(Vector2 size)
    {
        for(int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                Transform createdPiece = ObjectPooler.Instance.SpawnFromPool("circle", Vector3.zero, Quaternion.identity).transform;
                createdPiece.GetComponent<IGridPiece>().point = new Vector2(i, j);
                createdPiece.parent = transform;
                
                //createdPiece.localPosition = new Vector3(newPos.x, newPos.y, 0);
                createdPiece.GetComponent<IGridPiece>().UpdatePositionandScale(calculatePos(i,j), calculateScale());
            }
        }
    }
    public void updateGrid()
    {
        if (gridSize.x * gridSize.y >= transform.childCount)// to understand that should we add or remove elements in the new situation
        {
            int[,] hashMap = new int[(int)(gridSize.x), (int)(gridSize.y)];
            foreach (Transform element in transform)
            {
                IGridPiece gridInterface = element.GetComponent<IGridPiece>();
                hashMap[(int)gridInterface.point.x, (int)gridInterface.point.y] = 1;
            }
            Debug.Log("Hashmap length is: " + hashMap.Length);
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    if (hashMap[i, j] != 1)
                    {
                        Transform createdPiece = ObjectPooler.Instance.SpawnFromPool("circle", Vector3.zero, Quaternion.identity).transform;
                        createdPiece.GetComponent<IGridPiece>().point = new Vector2(i, j);
                        createdPiece.parent = transform;
                        //createdPiece.GetComponent<IGridPiece>().UpdatePositionandScale(calculatePos(i, j), calculateScale());
                    }
                }
            }
        }
        else
        {
            int childrenNumber = transform.childCount;
            for (int i = childrenNumber - 1; i >= 0; i--)
            {
                IGridPiece gridPiece = transform.GetChild(i).GetComponent<IGridPiece>();
                Vector2 pointOfelement = gridPiece.point;
                if (pointOfelement.x >= gridSize.x || pointOfelement.y >= gridSize.y)
                {
                    transform.GetChild(i).parent = null;
                    gridPiece.Disappear();
                }
            }
        }
        foreach(Transform element in transform)
        {
            IGridPiece gridInterface = element.GetComponent<IGridPiece>();
            gridInterface.UpdatePositionandScale(calculatePos((int)gridInterface.point.x, (int)gridInterface.point.y), calculateScale());
            //element.localPosition = calculatePos((int)gridInterface.point.x, (int)gridInterface.point.y);
        }
    }
    public void updateBackgroundBoard()
    {
        Vector3 targetScale = Vector3.zero;
        targetScale.y = (boardmaxScale.x - boardminScale.x) / (maxSize.x - minSize.x) * (gridSize.x - minSize.x) + boardminScale.x;
        targetScale.x = (boardmaxScale.y - boardminScale.y) / (maxSize.y - minSize.y) * (gridSize.y - minSize.y) + boardminScale.y;
        BackgroundBoard.GetComponent<IGridPiece>().UpdatePositionandScale(Vector3.zero, targetScale);
    }
    public Vector3 calculateScale()
    {
        float calculatedScale = (maxScale - minScale) / ((maxSize.x - minSize.x) + (maxSize.y - minSize.y)) * ((maxSize.x - gridSize.x) + (maxSize.y - gridSize.y)) + minScale;
        return new Vector3(calculatedScale, calculatedScale, calculatedScale);
    }
    public Vector3 calculatePos(int i,int j)
    {
        Vector2 distanceBetweenElements;
        distanceBetweenElements.x = (maxDistanceBetweenElements.x - minDistanceBetweenElements.x) / (maxSize.x - minSize.x) * (maxSize.x - gridSize.x)
            + minDistanceBetweenElements.x;
        distanceBetweenElements.y = (maxDistanceBetweenElements.y - minDistanceBetweenElements.y) / (maxSize.y - minSize.y) * (maxSize.y - gridSize.y)
            + minDistanceBetweenElements.y;
        Vector2 newPos;
        newPos.y = distanceBetweenElements.x * (gridSize.x - 1) / 2 - i * distanceBetweenElements.x;
        newPos.x = -distanceBetweenElements.y * (gridSize.y - 1) / 2 + j * distanceBetweenElements.y;
        return new Vector3(newPos.x, newPos.y, 0);
    }
    public IEnumerator startDelay()
    {
        yield return new WaitForSeconds(0.5f);
        createGrid(gridSize);
    }

    public void downPressed()
    {
        if (gridSize.x < maxSize.x)
        {
            gridSize.x++;
            updateGrid();
            updateBackgroundBoard();
        }
    }
    public void upPressed()
    {
        if (gridSize.x > minSize.x)
        {
            gridSize.x--;
            updateGrid();
            updateBackgroundBoard();
        }
    }
    public void rightPressed()
    {
        if (gridSize.y < maxSize.y)
        {
            gridSize.y++;
            updateGrid();
            updateBackgroundBoard();
        }
    }
    public void leftPressed()
    {
        if (gridSize.y > minSize.y)
        {
            gridSize.y--;
            updateGrid();
            updateBackgroundBoard();
        }
    }
    public void ChangeColor(Color targetColor)
    {
        currentColor = targetColor;
    }
    public void SaveGrid()
    {
        List<GridElement> gridList = new List<GridElement>();
        foreach(Transform element in transform)
        {
            IGridPiece piece = element.GetComponent<IGridPiece>();
            GridElement thisPiece = new GridElement { point = piece.point.ToString(), color = piece.selfColor.ToString() };
            gridList.Add(thisPiece);
        }
        Debug.Log(gridList.Count);
        SaveSystem.SaveToJson(gridList);
    }
}
