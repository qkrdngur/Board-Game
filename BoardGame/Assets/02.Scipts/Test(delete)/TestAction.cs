using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction : MonoBehaviour
{
    public event Action testAction;

    // Start is called before the first frame update
    void Start()
    {
        testAction += TestStart;
    }

    private void TestStart()
    {
        print("������");
    } 
    
    private void TestRunning()
    {
        print("������");
    }
    
    private void TestEnd()
    {
        print("�ߴ���");
    }

    // Update is called once per frame
    void Update()
    {
        testAction.Invoke();
    }

    private void OnDisable()
    {
        testAction += TestEnd;
        testAction();
    }
}
