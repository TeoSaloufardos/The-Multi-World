using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Oscillator : MonoBehaviour
{

    Vector3 startingPosition;

    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0,1)] float movementFactor;
    [SerializeField] float period = 2f;

    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveObstacleWithMaths();
    }

    private void moveObstacleWithMaths()
    {
        if(period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; //������� ������� �������� ��� �����

        const float tau = Mathf.PI * 2; //������ �������� � (����������)
        float rawSinWave = Mathf.Sin(cycles * tau); //���������������� ��� �� 0 ��� �� 1 

        movementFactor = (rawSinWave + 1f) / 2f;

        Vector3 offSet = movementVector * movementFactor;
        transform.position = startingPosition + offSet;
    }
}
