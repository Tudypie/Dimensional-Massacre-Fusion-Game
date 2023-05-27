using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{   
    [SerializeField] private Animator blackImageAnimator;
    private GameObject[] spawnpoints;
    private List<Transform> reachedSpawnpoints = new List<Transform>();
    private Transform playerTransform;

    public static RespawnManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {   
        spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
    
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            RespawnAtLatestSpawnpoint();
    }

    public void RespawnAtLatestSpawnpoint()
    {   
        StartCoroutine(RespawnSequence());
    }

    private IEnumerator RespawnSequence()
    {
        blackImageAnimator.Play("FadeInAndOut");
        yield return new WaitForSeconds(1f);

        if (reachedSpawnpoints.Count > 0)
        {
            playerTransform.position = reachedSpawnpoints[reachedSpawnpoints.Count - 1].position;
            playerTransform.rotation = reachedSpawnpoints[reachedSpawnpoints.Count - 1].rotation;
        }
        else
        {
            playerTransform.position = spawnpoints[0].transform.position;
            playerTransform.rotation = spawnpoints[0].transform.rotation;
        }
    }

    public void AddReachedSpawnpoint(Transform spawnpointTransform)
    {
        if(!reachedSpawnpoints.Contains(spawnpointTransform))
            reachedSpawnpoints.Add(spawnpointTransform);
    }
    
}
