using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBoardBehaviour : MonoBehaviour,IGridPiece
{
    public Vector2 point { get; set; }
    public Color selfColor { get; set; }
    private Coroutine ScaleUpdate;
    public void UpdatePositionandScale(Vector3 pos, Vector3 scale)
    {
        if (ScaleUpdate != null)
        {
            StopCoroutine(ScaleUpdate);
        }
        ScaleUpdate = StartCoroutine(UpdateScale(scale));
    }
    private IEnumerator UpdateScale(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 10);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = targetScale;
    }
    public void Disappear()
    {

    }
}
