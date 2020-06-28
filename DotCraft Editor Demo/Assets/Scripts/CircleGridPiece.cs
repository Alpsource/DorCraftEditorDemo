using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleGridPiece : MonoBehaviour,IGridPiece,IPooledObject
{
    public Vector2 point { get; set; }
    public Color selfColor { get; set; }
    private Coroutine ScaleUpdate;
    private Coroutine PositionUpdate;
    public void initialize()
    {
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        selfColor = new Color(0.4f, 0.4f, 0.4f);
        transform.GetComponent<SpriteRenderer>().color = selfColor;
    }
    public void UpdatePositionandScale(Vector3 pos, Vector3 scale)
    {
        if (PositionUpdate != null)
        {
            StopCoroutine(PositionUpdate);
        }
        PositionUpdate = StartCoroutine(UpdatePos(pos));
        if (ScaleUpdate != null)
        {
            StopCoroutine(ScaleUpdate);
        }
        ScaleUpdate = StartCoroutine(UpdateScale(scale));
    }
    private IEnumerator UpdatePos(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime*10);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localPosition = targetPosition;
    }
    private IEnumerator UpdateScale(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime*10);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = targetScale;
    }
    public void Disappear()
    {
        transform.parent = null;
        StartCoroutine(SelfDisappear());
    }
    private IEnumerator SelfDisappear()
    {
        while (Vector3.Distance(transform.localScale, Vector3.zero) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 10);
            if (ScaleUpdate != null)
            {
                StopCoroutine(ScaleUpdate);
            }
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.SetActive(false);
    }
    private void OnMouseDown()
    {
        Debug.Log("Clicked on: " + point);
        selfColor = transform.GetComponentInParent<GridBehaviour>().currentColor;
        transform.GetComponent<SpriteRenderer>().color = selfColor;
    }
}
