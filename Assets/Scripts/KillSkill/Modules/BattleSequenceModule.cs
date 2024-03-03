using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Game;
using SessionData.Implementations;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules
{
    public class BattleSequenceModule : MonoBehaviour
    {
        [SerializeField] private ResultView resultView;
        [SerializeField] private CountdownUI countdown;
        [SerializeField] private Character player, enemy;

        private float battleTimeSeconds = 0f;

        private bool hasPlayerWon, isBattlePaused;

        private BattleResultData result;

        private IEnumerator Start()
        {
            player.onDeath += OnAnyDeath;
            enemy.onDeath += OnAnyDeath;
            
            SetBattlePause(true);
            
            for (int i = 3; i > 0; i--)
            {
                countdown.Count(i);
                yield return new WaitForSeconds(1f);
            }
            
            countdown.Count(0);
            
            SetBattlePause(false);
        }

        private void Update()
        {
            if (isBattlePaused) return;
            battleTimeSeconds += Time.deltaTime;
        }

        public void SetBattlePause(bool pause)
        {
            isBattlePaused = pause;
            player.SetBattlePaused(pause);
            enemy.SetBattlePaused(pause);
        }

        public void OnAnyDeath()
        {
            player.onDeath -= OnAnyDeath;
            enemy.onDeath -= OnAnyDeath;

            SetBattlePause(true);

            hasPlayerWon = player.IsAlive;

            var battleSession = Session.GetData<BattleSessionData>();

            var state = new BattleResultState(hasPlayerWon, player.Resources.Current, enemy.Resources.Current, battleTimeSeconds);

            result = new(hasPlayerWon, battleSession.CalculateReward(state));

            var resourcesSession = Session.GetData<ResourcesSessionData>();
            
            foreach (var resource in result.gainedResources)
                resourcesSession.AddResource(resource.Key, resource.Value);

            StartCoroutine(EndingSequence());
        }

        private IEnumerator EndingSequence()
        {
            Time.timeScale = 0.2f;

            var animTime = 3f;
            var time = 0f;

            while (time < animTime)
            {
                var a = time / animTime;
                Time.timeScale = Mathf.Lerp(0.2f, 0.5f, a);
                time += Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }
            Time.timeScale = 1f;
            
            resultView.Display(result);
        }
    }
}