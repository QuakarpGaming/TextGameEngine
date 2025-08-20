using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGameEngine.Player
{
    public enum EquipmentType
    {
        Head,
        Body,
        Arms,
        Legs,
        Weapon,
        Shield,
        Ring,
        Neck
    }

    public class Equipment
    {
        #region Constructors
        public Equipment()
        {
            this.Type = EquipmentType.Head;
            this.IsEquiped = false;
            this.MinDamageBoost = 0;
            this.MaxDamageBoost = 0;
            this.DodgeBoost = 0;
            this.HitBoost = 0;
            this.DamageReductionBoost = 0;
            this.HealthBoost = 0;
            this.Name = "Stuff";
            this.NamePrivate = this.Name.ToUpper();
        }
        public Equipment(string? name = null,EquipmentType type = EquipmentType.Head, bool isEquiped = false, int minDamageBoost = 0, int maxDamageBoost = 0, int dodgeBoost = 0, int hitBoost = 0, int damageReductionBoost = 0, int healthBoost = 0)
        {
            Type = type;
            IsEquiped = isEquiped;
            MinDamageBoost = minDamageBoost;
            MaxDamageBoost = maxDamageBoost;
            DodgeBoost = dodgeBoost;
            HitBoost = hitBoost;
            DamageReductionBoost = damageReductionBoost;
            HealthBoost = healthBoost;
            this.Name = name?.Trim().ToUpper() ?? string.Empty;
            this.NamePrivate = name?.Trim().ToUpper() ?? string.Empty;

        }

        #endregion

        #region Data - public
        public EquipmentType Type { get; set; }
        public string Name { get { return this.NamePrivate.ToUpper(); } set { this.NamePrivate = value.Trim().ToUpper(); } }
        private string NamePrivate {  get; set; }
        public bool IsEquiped { get; set; }
        public int MinDamageBoost { get; set; }
        public int MaxDamageBoost { get; set; }
        public int DodgeBoost { get; set; }
        public int HitBoost { get; set; }
        public int DamageReductionBoost { get; set; }
        public int HealthBoost { get; set; }
        #endregion
    }
}
