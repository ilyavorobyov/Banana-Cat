using UnityEngine;

[RequireComponent(typeof(BananaCatMover))]
public class BananaCat : MonoBehaviour
{
    private BananaCatMover _bananaCatMover;

    private void Start()
    {
        _bananaCatMover = GetComponent<BananaCatMover>();
    }

    public void Die()
    {
        _bananaCatMover.Cry();
        Debug.Log("game over");
    }
}