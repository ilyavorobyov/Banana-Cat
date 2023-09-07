using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DangerousItemsSpawner))]
[RequireComponent(typeof(NerfItemsSpawner))]
public class FruitSpawner : Spawner
{
    [SerializeField] private float _minTimeOfAppearanceFruit;
    [SerializeField] private float _maxTimeOfAppearanceFruit;

    private Coroutine _createObject;
    private float _timeOfAppearanceFruit;
    private DangerousItemsSpawner _dangerousItemsSpawner;
    private NerfItemsSpawner _nerfItemsSpawner;

    private void Start()
    {
        _dangerousItemsSpawner = GetComponent<DangerousItemsSpawner>();
        _nerfItemsSpawner = GetComponent<NerfItemsSpawner>();
        _timeOfAppearanceFruit = Random.Range(_minTimeOfAppearanceFruit, _maxTimeOfAppearanceFruit);

        if (_createObject != null)
        {
            StopCoroutine(_createObject);
        }

        _createObject = StartCoroutine(CreateObject());
    }

    private IEnumerator CreateObject()
    {
        var waitForSeconds = new WaitForSeconds(_timeOfAppearanceFruit);

        while (true)
        {
            yield return waitForSeconds;

            if (IsCanCollectHiddenObjects())
            {
                SpawnPosition = new Vector2(Random.Range(MinXPosition, MaxXPosition), YSpawnPosition);
                XPosition = SpawnPosition.x;
                NextSpawnObject.Init(CalculateFallingSpeed(), SpawnPosition);
                _timeOfAppearanceFruit = Random.Range(_minTimeOfAppearanceFruit, _maxTimeOfAppearanceFruit);
                _dangerousItemsSpawner.ShowItem(XPosition);
                _nerfItemsSpawner.ShowItem(XPosition);
            }
        }
    }
}