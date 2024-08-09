using System;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.Utils;
using Arr.ViewModuleSystem;
using DG.Tweening;
using KillSkill.Modules.Network.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Multiplayer
{
    public class MultiplayerView : View
    {
        [SerializeField] private GameObject holder, loadingGroup;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private TMP_InputField ipInputField;
        [SerializeField] private Button joinButton;
        [SerializeField] private PartyPanel partyPanel;

        private void Awake()
        {
            joinButton.onClick.AddListener(OnJoinButtonClicked);
        }

        private void OnJoinButtonClicked()
        {
            if (string.IsNullOrEmpty(ipInputField.text))
            {
                SetErrorText("IP field is empty!");
                return;
            }
            
            GlobalEvents.Fire(new StartJoinEvent(ipInputField.text));
        }

        public void UpdateParty(NetworkPartySessionData partySessionData, NetworkIdSessionData networkIdSessionData) 
            => partyPanel.Display(partySessionData, networkIdSessionData);

        public void SetLoading(bool isLoading)
        {
            holder.SetActive(!isLoading);
            loadingGroup.SetActive(isLoading);
        }

        public void SetErrorText(string text)
        {
            errorText.text = text;
            errorText.color = errorText.color.Alpha(1f);
            errorText.CrossFadeAlpha(0f, 4f, true);
        }
    }
}