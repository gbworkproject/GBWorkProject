﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Enemy;
using Game.TerrainGeneration;
namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject mainMenuPannel;
        public GameObject gameBar;



        [Header("Textfields")]
        public Text countdownText;
        public Text livesText;
        public Text moneytext;

        [Header("Players parameters")]
        public int startLives;
        public int startMoney;

        private float _countdown;
        private int _waveIndex = 0;
        [HideInInspector]
        public bool isCountingDown = false;
        [HideInInspector]
        public int currentLives;
        [HideInInspector]
        public int currentMoney;

        

        private void FixedUpdate()
        {
            if (isCountingDown)
            {
                countdownText.text = ((int)_countdown).ToString();
                _countdown -= Time.fixedDeltaTime;
                if (_countdown <= 0 )
                {
                    EnemiesController.Instance.SpawnSimultaneously(_waveIndex);
                    if (_waveIndex < EnemiesController.Instance.waves.Length-1)
                    {
                        _waveIndex += 1;
                        _countdown = EnemiesController.Instance.waves[_waveIndex].countdownToSpawn;
                    }
                    else
                    {
                        isCountingDown = false;
                        return;
                    }
                }
            }
        }
        public void Pause()
        {
            Time.timeScale = 0;
           
        }
        public void Resume()
        {
            Time.timeScale = 1;
            
        }
        public void FinishGame()
        {
            TerrainGeneratorController.Instance.DestroyTerrain();
            _waveIndex = 0;
            EnemiesController.Instance.ClearEnemyList();
            EnemiesController.Instance.ClearSpawnerList();
            currentMoney = startMoney;
            currentLives = startLives;
            isCountingDown = false;
            gameBar.SetActive(false);
            mainMenuPannel.SetActive(true);
        }
        public void StartGame()
        {
            TerrainGeneratorController.Instance.GenerateTerrain();
            currentMoney = startMoney;
            currentLives = startLives;
            _countdown = EnemiesController.Instance.waves[_waveIndex].countdownToSpawn;
            countdownText.text += ((int)_countdown).ToString();
            livesText.text = currentLives.ToString();
            moneytext.text = currentMoney.ToString();
            isCountingDown = true;
            mainMenuPannel.SetActive(false);
            gameBar.SetActive(true);

        }
    }
}


