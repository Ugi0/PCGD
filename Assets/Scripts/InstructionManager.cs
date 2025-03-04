using System;
using System.Data;
using Unity.Collections;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private STATE currentState = STATE.HIDE;
    private bool isTutorialOff;


    private enum STATE {
        READY,
        AIM,
        POWER,
        THROW,
        HIT,
        MISS,
        HIDE
    }

    void Start()
    {   
        isTutorialOff = (PlayerPrefs.GetInt("isTutorialOff") != 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTutorialOff)
        {
            if (getCurrentState() == currentState) return;
            currentState = getCurrentState();
            disableAllInstructions();
            switch (currentState) {
                case STATE.READY:
                    gameObject.transform.Find("1").gameObject.SetActive(true);
                    break;
                case STATE.AIM:
                    gameObject.transform.Find("2").gameObject.SetActive(true);
                    break;
                case STATE.POWER:
                    gameObject.transform.Find("3").gameObject.SetActive(true);
                    break;
                case STATE.THROW:
                    gameObject.transform.Find("4").gameObject.SetActive(true);
                    break;
                case STATE.HIT:
                    gameObject.transform.Find("hit").gameObject.SetActive(true);
                    PlayerPrefs.SetInt("isTutorialOff", true ? 1 : 0);
                    break;
                case STATE.MISS:
                    gameObject.transform.Find("miss").gameObject.SetActive(true);
                    break;
                case STATE.HIDE:
                    break;
            }
        }
    }

    private STATE getCurrentState() {
        if (obstacleSpawner.targetSpawnCount != 0 || playerController.playerState == PlayerState.THROWING) return STATE.HIDE;
        if (playerController.playerState == PlayerState.IDLE && gameStateManager.Health != 3 && playerController.throwCount != 0) return STATE.MISS;
        if (playerController.playerState == PlayerState.SKATING && playerController.throwCount != 0) return STATE.HIT;

        if (playerController.playerState == PlayerState.AIMING) return STATE.AIM;
        if (playerController.playerState == PlayerState.POWERING) return STATE.POWER;
        if (playerController.playerState == PlayerState.WAITING) return STATE.THROW;

        return STATE.READY;
    }


    private void disableAllInstructions() {
        for (int i = 1; i <= 4; i++) {
            gameObject.transform.Find(i.ToString()).gameObject.SetActive(false);
        }
        gameObject.transform.Find("hit").gameObject.SetActive(false);
        gameObject.transform.Find("miss").gameObject.SetActive(false);
    }
}
