using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using Arr.UnityUtils;
using Arr.Utils;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Modules.Battle.Events;
using KillSkill.Modules.Network;
using KillSkill.Modules.Network.Events;
using KillSkill.Network;
using KillSkill.Network.Messages.Battle;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Battle.Events;
using KillSkill.UI.Battle.Modules;
using SessionData.Implementations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KillSkill.Modules.Battle
{
    public class BattleControllerModule : BaseModule,
        IEventListener<NetMessageEvent<BattleStartNetMessage>>,
        IEventListener<NetMessageEvent<BattleEndNetMessage>>
    {
        [InjectModule] private ResultViewModule resultView;
        [InjectModule] private CountdownViewModule countdown;
        [InjectModule] private BattleRegistryModule battleRegistry;
        [InjectModule] private CameraControlModule cameraControl;
        [InjectModule] private CommitModule commitModule;

        private Dictionary<ulong, Character> players = new();
        private List<ICharacter> alivePlayers = new();
        private Character enemy;
        private BattleLevel level;

        private TaskCompletionSource<BattleStartNetMessage> battleStartTsc = new();

        private bool hasPlayerWon, isBattlePaused;

        public bool IsBattlePaused => isBattlePaused;

        protected override Task OnLoad()
        {
            Start().CatchExceptions();
            return base.OnLoad();
        }

        protected override Task OnUnload()
        {
            foreach (var player in players.Values)
                Object.Destroy(player);
            
            if (enemy != null) Object.Destroy(enemy.gameObject);
            Object.Destroy(level.gameObject);
            return base.OnUnload();
        }

        private async Task Start()
        {
            if (Net.IsServer()) await InitializeServer();
            if (Net.IsClient()) await battleStartTsc.Task;
            
            SetBattlePause(true);
            
            for (int i = 3; i > 0; i--)
            {
                countdown.Count(i);
                await Task.Delay(1000);
            }
            
            countdown.Count(0);
            
            SetBattlePause(false);
        }

        private async Task InitializeServer()
        {
            var committedClients = await commitModule.CommittedClientsTsc.Task;
            
            Debug.Log($"[BCM] INITIALIZING SERVER WITH {committedClients.Length} COMMITTED CLIENT IDS: " +
                      $"{String.Join(", ", committedClients)}".LogColor("yellow"));
            
            var battleSession = Session.GetData<BattleSessionData>();
            var data = battleSession.StartData;
            enemy = battleRegistry.CreateNpc(data.npcDefinition);

            level = battleRegistry.CreateLevel(data.levelId);
            
            enemy.Position = level.EnemySpawnPoint.position;
            enemy.onDeath += ServerOnEnemyDeath;
            
            var cam = cameraControl.Controller;
            cam.AddTargetToGroup(enemy.transform);
            
            var party = Session.GetData<NetworkPartySessionData>();

            for (var i = 0; i < committedClients.Length; i++)
            {
                var id = committedClients[i];
                if (!party.TryGet(id, out var user))
                {
                    Debug.LogError($"[BCM] COULD NOT FIND ID {id} IN PARTY");
                    continue;
                }

                Debug.Log($"[BCM] SPAWNING ID {id}".LogColor("red"));
                
                var player = battleRegistry.CreatePlayer(user.Skills, id);
                
                player.Position = level.PlayerSpawnPoints[i].position;
                player.SetTarget(enemy);
                player.onDeath += OnPlayerDeath;
                cam.AddTargetToGroup(player.transform);
                
                players.Add(id, player);
                alivePlayers.Add(player);
            }
            
            enemy.SetTarget(players.First().Value);
            Net.Server.Broadcast(new BattleStartNetMessage());
        }

        private void ServerOnEnemyDeath(ICharacter character)
        {
            enemy.onDeath -= ServerOnEnemyDeath;
            ServerOnEndGame();
        }

        private void OnPlayerDeath(ICharacter character)
        {
            character.onDeath -= OnPlayerDeath;
            alivePlayers.Remove(character);
            if (alivePlayers.Count == 0) ServerOnEndGame();
        }

        public void SetBattlePause(bool pause)
        {
            if (!Net.IsServer()) return;
            isBattlePaused = pause;

            foreach (var player in players.Values)
                player.SetBattlePaused(pause);
            
            enemy.SetBattlePaused(pause);
            Net.Server.Broadcast(new BattlePauseNetMessage(pause));
        }

        public void ServerOnEndGame()
        {
            SetBattlePause(true);

            hasPlayerWon = alivePlayers.Count > 0;

            var battleSession = Session.GetData<BattleSessionData>();

            var battleTimeSeconds = GlobalEvents.Query<BattleTimerQuery>().timeInSeconds;

            var characters = new List<ICharacter>();
            characters.AddRange(players.Cast<ICharacter>());
            characters.Add(enemy);
            
            var state = new BattleResultState(hasPlayerWon, battleTimeSeconds, characters);
            
            var data = battleSession.StartData.npcDefinition;
            var rewards = CalculateReward(data, state);

            var milestones = new string[]
            {
                Milestones.HasDefeated(data.Id)
            };

            var result = new BattleResultData(hasPlayerWon, rewards, milestones);

            Net.Server.Broadcast(new BattleEndNetMessage());
        }

        private List<BattleReward> CalculateReward(INpcDefinition data, BattleResultState state)
        {
            var list = new List<BattleReward>();
            foreach (var reward in data.Rewards)
            {
                if (!reward.TryCalculateReward(state, out var calculatedReward)) continue;
                list.Add(calculatedReward);
            }

            return list;
        }

        private void ClientOnEndGame(BattleResultData data)
        {
            var resourcesSession = Session.GetData<ResourcesSessionData>();
            
            foreach (var resource in data.Rewards)
                resourcesSession.AddResource(resource.ResourceId, resource.ResourceAmount);
            
            var milestonesSession = Session.GetData<MilestonesSessionData>();

            foreach (var milestone in data.Milestones)
                milestonesSession.TryAdd(milestone);

            CoroutineUtility.Start(EndingSequence(data));
        }

        private IEnumerator EndingSequence(BattleResultData data)
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
            
            resultView.Display(data);
        }

        public void OnEvent(NetMessageEvent<BattleStartNetMessage> data)
        {
            Debug.Log("[BCM] GOT BATTLE START MESSAGE");
            battleStartTsc.SetResult(data.message);
        }

        public void OnEvent(NetMessageEvent<BattleEndNetMessage> data)
        {
            Debug.Log("[BCM] GOT BATTLE END MESSAGE");
            ClientOnEndGame(data.message.Result);
        }
    }
}