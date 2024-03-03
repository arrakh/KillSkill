﻿using System;
using CharacterResources;
using KillSkill.Characters;
using UI;
using UnityEngine;

namespace KillSkill.CharacterResources.Implementations
{
    public class Shield : ICharacterResource, IModifyDamage, IResourceBarDisplay
    {
        public Shield(Character owner, double charge)
        {
            this.owner = owner;
            this.charge = charge;
            maxCharge = charge;

            DisplayData = new()
            {
                value = this.charge,
                min = 0, max = this.charge,
                barColor = new Color(58/255f, 52f/255f, 235f/255f),
            };

        }

        private Character owner;
        private double charge;
        private double maxCharge;

        public void AddCharge(double delta)
        {
            charge += delta;
            if (charge > maxCharge) maxCharge = charge;

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            DisplayData.value = charge;
            DisplayData.max = maxCharge;
            OnUpdateDisplay?.Invoke(DisplayData);
        }

        public void ModifyDamage(Character damager, ref double damage)
        {
            var lowest = Math.Min(damage, charge);
            Debug.Log($"[SHIELD] LOWEST {lowest}, DMG {damage}, CHARGE {charge}");
            damage -= lowest;
            charge -= lowest;

            UpdateDisplay();

            if (charge <= 0) owner.Resources.Unassign<Shield>();
        }

        public event Action<ResourceBarDisplay> OnUpdateDisplay;
        public ResourceBarDisplay DisplayData { get; private set; }
    }
}