using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using Arr.UnityUtils;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Modules.Battle.Events;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Game.Events;
using KillSkill.UI.Game.Modules;
using SessionData.Implementations;
using UnityEngine;

namespace KillSkill.Modules.Battle
{
    public class BattleControllerModule : BaseModule
    {
        [InjectModule] private ResultViewModule resultView;
        [InjectModule] private CountdownViewModule countdown;
        [InjectModule] private BattleFactoryModule battleFactory;
        private Character player, enemy;
        private BattleLevel level;

        private bool hasPlayerWon, isBattlePaused;

        public bool IsBattlePaused => isBattlePaused;

        private BattleResultData result;

        protected override Task OnInitialized()
        {
            CoroutineUtility.Start(Start());
            return base.OnInitialized();
        }

        private IEnumerator Start()
        {
            InitializeBattle();
            
            player.onDeath += OnPlayerDeath;
            enemy.onDeath += OnEnemyDeath;
            
            SetBattlePause(true);
            
            for (int i = 3; i > 0; i--)
            {
                countdown.Count(i);
                yield return new WaitForSeconds(1f);
            }
            
            countdown.Count(0);
            
            SetBattlePause(false);
        }

        private void InitializeBattle()
        {
            var skillsSession = Session.GetData<SkillsSessionData>();
            player = battleFactory.CreatePlayer(skillsSession);

            var battleSession = Session.GetData<BattleSessionData>();
            var data = battleSession.StartData;
            enemy = battleFactory.CreateNpc(data.enemyData);

            level = battleFactory.CreateLevel(data.levelId);

            player.Position = level.PlayerSpawnPoint.position;
            enemy.Position = level.EnemySpawnPoint.position;
        }

        private void OnEnemyDeath(ICharacter character)
        {
            enemy.onDeath -= OnEnemyDeath;
            OnAnyDeath();
        }

        private void OnPlayerDeath(ICharacter character)
        {
            player.onDeath -= OnPlayerDeath;
            OnAnyDeath();
        }

        public void SetBattlePause(bool pause)
        {
            isBattlePaused = pause;
            player.SetBattlePaused(pause);
            enemy.SetBattlePaused(pause);
            GlobalEvents.Fire(new BattlePauseEvent(pause));
        }

        public void OnAnyDeath()
        {
            SetBattlePause(true);

            hasPlayerWon = player.IsAlive;

            var battleSession = Session.GetData<BattleSessionData>();

            var battleTimeSeconds = GlobalEvents.Query<BattleTimerQuery>().timeInSeconds;

            var state = new BattleResultState(hasPlayerWon, player.Resources.Current, enemy.Resources.Current, battleTimeSeconds);
            
            var data = battleSession.StartData.enemyData;
            var rewards = CalculateReward(data, state);

            result = new(hasPlayerWon, rewards);

            var resourcesSession = Session.GetData<ResourcesSessionData>();
            
            foreach (var resource in rewards)
                resourcesSession.AddResource(resource.resourceId, resource.resourceAmount);

            CoroutineUtility.Start(EndingSequence());
        }

        private List<BattleReward> CalculateReward(IEnemyData data, BattleResultState state)
        {
            var list = new List<BattleReward>();
            foreach (var reward in data.Rewards)
            {
                if (!reward.TryCalculateReward(state, out var calculatedReward)) continue;
                list.Add(calculatedReward);
            }

            return list;
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