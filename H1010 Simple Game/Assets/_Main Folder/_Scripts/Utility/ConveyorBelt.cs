using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    bool ableToSpawn;

    [SerializeField] GameObject BoxObject;
    [SerializeField] Transform boxSpawnPos;
    [SerializeField] Transform BOXES;

    List<Box> Boxes = new List<Box>();
    Box CurrentBox;

    private void OnEnable()
    {
        LevelEventManager.onTimesUp += GameOver_Behaviour;
        LevelEventManager.onNoLifeRemains += GameOver_Behaviour;
        LevelEventManager.onPausedGame += GamePaused_Behaviour;
        LevelEventManager.onResumedGame += GameResume_Behaviour;
    }

    private void OnDisable()
    {
        LevelEventManager.onTimesUp -= GameOver_Behaviour;
        LevelEventManager.onNoLifeRemains -= GameOver_Behaviour;
        LevelEventManager.onPausedGame -= GamePaused_Behaviour;
        LevelEventManager.onResumedGame -= GameResume_Behaviour;
    }

    private void Start()
    {
        ableToSpawn = true;
        StartCoroutine(BringBoxes());
    }

    IEnumerator BringBoxes()
    {
        while (ableToSpawn)
        {
            GameObject box = Instantiate(BoxObject, boxSpawnPos.position, Quaternion.identity, BOXES);
            BoxManager.instance.allBoxes.Add(box);
            if (BoxManager.instance.currentBox == null)
            {
                BoxManager.instance.currentBox = box.transform;
            }
            yield return new WaitForSeconds(5f);
        }
    }

    void GameOver_Behaviour()
    {
        ableToSpawn = false;
    }

    void GamePaused_Behaviour()
    {
        ableToSpawn = false;
    }

    void GameResume_Behaviour()
    {
        ableToSpawn = true;
    }
}
