using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] animalPrefabs;
    private GroundBound ground;
    private float spawnLeft;
    private float spawnRight;
    private float spawnBottom;
    private float spawnUp;
    private float spawnOffSet;

    private float spawnDelay;
    [SerializeField] private float spawnInterval = 8f;

    // rotate animal at where it spawned
    Quaternion spawnTopRota = Quaternion.Euler(0, 180, 0);
    Quaternion spawnLeftRota = Quaternion.Euler(0, 90, 0);
    Quaternion spawnRightRota = Quaternion.Euler(0, 270, 0);
    Quaternion spawnDownRota = Quaternion.Euler(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {       
        ground = GameObject.FindWithTag("Ground").GetComponent<GroundBound>();
        //UpdateBound();

        InvokeRepeating("SpawnAnimal", spawnDelay, spawnInterval);

        // handle event from gameIver
        PlayerGetHit eventGameOverHandle = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGetHit>();
        eventGameOverHandle.eventGameOver.AddListener(SetActive);
    }
    //Event function
    void SetActive()
    {
        gameObject.SetActive(false);
    }
    public void UpdateBound(float topBound, float rightBound, float leftBound, float bottomBound)
    {
        spawnBottom = bottomBound;
        spawnLeft = leftBound;
        spawnRight = rightBound;
        spawnUp = topBound;

        //Debug.Log($"New spawn Bound: Top = {topBound}, left = {leftBound}, right = {rightBound}, bottom = {bottomBound}");
    }
    void SpawnAnimal()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        // generate random spawn position of X and Z
        float prosXRange = Random.Range(spawnLeft, spawnRight);
        float prosZRange = Random.Range(spawnBottom, spawnUp);
        int spawnIndex = Random.Range(1, 4);

        // spawn position
        switch (spawnIndex)
        {
            case 1:
                //Top
                Vector3 spawnProsXTop = new Vector3(prosXRange, animalPrefabs[animalIndex].transform.position.y, spawnUp + spawnOffSet);
                GameObject insTop = Instantiate(animalPrefabs[animalIndex], spawnProsXTop, spawnTopRota, transform);
                break;
            case 2:
                //Down
                Vector3 spawnProsXDown = new Vector3(prosXRange, animalPrefabs[animalIndex].transform.position.y, spawnBottom - spawnOffSet);
                GameObject insDown = Instantiate(animalPrefabs[animalIndex], spawnProsXDown, spawnDownRota, transform);
                break;
            case 3:
                //Left
                Vector3 spawnProsZLeft = new Vector3(-spawnLeft - spawnOffSet, animalPrefabs[animalIndex].transform.position.y, prosZRange);
                GameObject insLeft = Instantiate(animalPrefabs[animalIndex], spawnProsZLeft, spawnLeftRota, transform);
                break;

            case 4:
                //Right
                Vector3 spawnProsZRight = new Vector3(spawnLeft + spawnOffSet, animalPrefabs[animalIndex].transform.position.y, prosZRange);
                GameObject insRight = Instantiate(animalPrefabs[animalIndex], spawnProsZRight, spawnRightRota, transform);
                break;

        }
    }
}
