using KillSkill.Network;
using TMPro;
using UnityEngine;

namespace KillSkill.UI.Multiplayer
{
    public class PartyPanelElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private PartySkillDisplay partySkill;

        public void Display(bool isLocal, LobbyUser user)
        {
            playerName.text = user.NetworkId.DisplayName + (isLocal ? " - (You)" : "");
            partySkill.Display(user.Skills);
        }
    }
}