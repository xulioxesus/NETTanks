using System;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    [SerializeField] private InputReader inputReader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputReader.MoveEvent += HandleMove;
    }

    private void HandleMove(Vector2 vector)
    {
        Debug.Log(vector);
    }

    void OnDestroy()
    {
        inputReader.MoveEvent -= HandleMove;
    }
}
