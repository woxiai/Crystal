using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crystal;
using Crystal.Pool;
using Crystal.Res;

public class TestMonoPool : MonoBehaviour
{

    PrefabGameObjectPoolFactory<CubePrefab> cubeFactory = new PrefabGameObjectPoolFactory<CubePrefab>();

    PrefabGameObjectPoolFactory<SpherePrefab> sphereFactory = new PrefabGameObjectPoolFactory<SpherePrefab>();

    GameObjectPool<CubePrefab> cubesPool;

    GameObjectPool<SpherePrefab> spherePool;

    private Stack<CubePrefab> cubeStack = new Stack<CubePrefab>();

    private Stack<SpherePrefab> sphereStack = new Stack<SpherePrefab>();

    private void Awake()
    {
        cubesPool = PoolManager.Instance.CreateGameObjectPool<CubePrefab>(null, cubeFactory, 8);
        spherePool = PoolManager.Instance.CreateGameObjectPool<SpherePrefab>(null, sphereFactory, 8);
    }

    private void Start()
    {
        //var cube = cubesPool.Get(transform);
        //var sphere = spherePool.Get(transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var cube = cubesPool.Get(transform, RandomPosition(), Quaternion.identity);
            if (cube != null)
            {
                this.cubeStack.Push(cube);
            }
            var sphere = spherePool.Get(transform, RandomPosition(), Quaternion.identity);
            if (sphere != null)
            {
                this.sphereStack.Push(sphere);
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            while (cubeStack.Count > 0)
            {
                cubesPool.Recycle(cubeStack.Pop());
            }
            while (sphereStack.Count > 0)
            {
                spherePool.Recycle(sphereStack.Pop());
            }
        }
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-2, 2f), Random.Range(-2, 2f), Random.Range(-2, 2f));
    }
}
