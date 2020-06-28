using UnityEngine;
public interface IGridPiece
{
    Vector2 point { get; set; }
    Color selfColor { get; set; }
    void UpdatePositionandScale(Vector3 pos,Vector3 scale);
    void Disappear();
}

