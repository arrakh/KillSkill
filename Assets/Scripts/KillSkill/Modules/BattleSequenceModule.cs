using System;
using System.Collections;
using Actors;
using DefaultNamespace;
using KillSkill.Constants;
using KillSkill.SessionData;
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

        private bool hasPlayerWon;

        private BattleResultData result;
        
        private IEnumerator Start()
        {
            player.onDeath += OnAnyDeath;
            enemy.onDeath += OnAnyDeath;
            
            player.SetBattlePaused(true);
            enemy.SetBattlePaused(true);

            for (int i = 3; i > 0; i--)
            {
                countdown.Count(i);
                yield return new WaitForSeconds(1f);
            }
            
            countdown.Count(0);
            
            player.SetBattlePaused(false);
            enemy.SetBattlePaused(false);
        }

        public void OnAnyDeath()
        {
            player.onDeath -= OnAnyDeath;
            enemy.onDeath -= OnAnyDeath;

            hasPlayerWon = player.IsAlive;

            result = new()
            {
                playerWon = hasPlayerWon,
                gainedResources = new()
                {
                    {GameResources.COINS, hasPlayerWon ? Random.Range(40, 70) : 0}
                }
            };

            var resourcesSession = Session.GetData<ResourcesSessionData>();
            
            foreach (var resource in result.gainedResources)
                resourcesSession.AddResource(resource.Key, resource.Value);
            
            Debug.Log("WINNNNN");
            
            StartCoroutine(EndingSequence());
        }

        private IEnumerator EndingSequence()
        {
            player.SetBattlePaused(true);
            enemy.SetBattlePaused(true);
            
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