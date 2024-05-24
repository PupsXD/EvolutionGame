using System;
using UnityEngine;

public class FunctionPeriodic
{
    public static void Create(Action action, float timer)
    {
        GameObject gameObject = new GameObject("FunctionPeriodic", typeof(MonoBehaviourHook));
        MonoBehaviourHook monoBehaviourHook = gameObject.GetComponent<MonoBehaviourHook>();
        monoBehaviourHook.StartCoroutine(UpdateCoroutine(action, timer));
    }

    private static System.Collections.IEnumerator UpdateCoroutine(Action action, float timer)
    {
        while (true)
        {
            action();
            yield return new WaitForSeconds(timer);
        }
    }

    private class MonoBehaviourHook : MonoBehaviour { }
}