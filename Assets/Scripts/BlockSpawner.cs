using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;

    private void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        GameObject newBlock = Instantiate(_prefabs[Random.Range(0, _prefabs.Length)]);
        newBlock.transform.position = new Vector3(0, 9, 0);
        newBlock.GetComponent<BlockBehaviour>().SetBlockSpawner(this);
        newBlock.GetComponent<BlockBehaviour>().SetScoreHandler(GetComponent<ScoreHandler>());
    }
}