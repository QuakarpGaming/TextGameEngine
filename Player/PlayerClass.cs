using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGameEngine.Player
{
    public class PlayerClass
    {

        #region Constructors
        public PlayerClass()
        {
            this.Equipment = new List<Equipment>();//this has to be first, it runs the max health check and this needs to be set.
            this.CurrentHealth = 10;
            this.CurrentHealthPrivate = 10;
            this.BaseMaxHealth = 10;
            this.MinDamageOutput = 1;
            this.MaxDamageOutput = 3;
            this.DamageReduction = 0;
            this.TakeAtLeastOneDamage = true;
            this.BaseHitChance = 90;
            this.BaseDodgeChance = 5;
            this.Name = "Rather Dashing";
        }
        public PlayerClass(string? name = null, int currentHealth = 10, int maxHealth = 10, int minDamageOutput =1, int maxDamageOutput = 3, int damageReduction=0, bool takeAtLeastOneDamage = true, int baseHitChance = 90, int baseDodgeChance = 5, List<Equipment>? equipment = null)
        {
            Equipment = equipment ?? [];//this has to be first, it runs the max health check and this needs to be set.
            Name = name ?? "Rather Dashing";
            CurrentHealth = currentHealth;
            CurrentHealthPrivate = currentHealth;
            BaseMaxHealth = maxHealth;
            MinDamageOutput = minDamageOutput;
            MaxDamageOutput = maxDamageOutput;
            DamageReduction = damageReduction;
            TakeAtLeastOneDamage = takeAtLeastOneDamage;
            BaseHitChance = baseHitChance;
            BaseDodgeChance = baseDodgeChance;
        }

        #endregion

        #region Stats
        public string Name { get; set; }
        #region Base Stats
        public int CurrentHealth { get { return CurrentHealthPrivate; } set { CheckMaxHealth(value); } }
        private int CurrentHealthPrivate {  get; set; }
        public int MinDamageOutput { get; set; }
        public int MaxDamageOutput { get; set; }
        public int DamageReduction { get; set; }
        public bool TakeAtLeastOneDamage { get; set; }
        public int BaseHitChance { get; set; }
        public int BaseDodgeChance { get; set; }
        #endregion

        #region Max/Total stats
        public int BaseMaxHealth { get; set; }
        public int TotalMaxHealthPublic { get { return TotalMaxHealth(); } }
        public int TotalMinDamagePublic { get { return TotalMinDamage(); } }
        public int TotalMaxDamagePublic { get { return TotalMaxDamage(); } }
        public int TotalDamageReductionPublic { get { return TotalDamageReduction(); } }
        public int TotalHitChancePublic { get { return TotalBaseHitChance(); } }
        public int TotalDodgeChancePublic { get { return TotalDodgeChance(); } }
        #endregion
        #endregion

        #region Equipment
        public List<Equipment> Equipment { get; set; }
        #endregion

        #region Calcs
        private void CheckMaxHealth(int value)
        {
            var totalMaxHealthTemp = TotalMaxHealth();
            if (value > totalMaxHealthTemp)
                value = totalMaxHealthTemp;

            this.CurrentHealthPrivate = value;
        }
        private int TotalMaxHealth()
        {
            var total = this.BaseMaxHealth;
            foreach (var equipment in Equipment.Where(x => x.IsEquiped == true))
            {
                total += equipment.HealthBoost;
            }
            return total;
        }
        private int TotalMinDamage()
        {
            var total = this.MinDamageOutput;
            foreach(var equipment in Equipment.Where(x => x.IsEquiped == true))
            {
                total += equipment.MinDamageBoost;
            }
            return total;
        }
        private int TotalMaxDamage()
        {
            var total = this.MaxDamageOutput;
            foreach (var equipment in Equipment.Where(x => x.IsEquiped == true))
            {
                total += equipment.MaxDamageBoost;
            }
            return total;
        }
        private int TotalDamageReduction()
        {
            var total = this.DamageReduction;
            foreach (var equipment in Equipment.Where(x => x.IsEquiped == true))
            {
                total += equipment.DamageReductionBoost;
            }
            return total;
        }
        private int TotalBaseHitChance()
        {
            var total = this.BaseHitChance;
            foreach (var equipment in Equipment.Where(x => x.IsEquiped == true))
            {
                total += equipment.HitBoost;
            }
            return total;
        }
        private int TotalDodgeChance()
        {
            var total = this.BaseDodgeChance;
            foreach (var equipment in Equipment.Where(x => x.IsEquiped == true))
            {
                total += equipment.DodgeBoost;
            }
            return total;
        }
        #endregion

        #region printers
        public void printAll()
        {
            printStats();
            Console.WriteLine();
            printEquipment();
        }

        public void printStats()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {this.Name}");
            sb.AppendLine($"Health: {this.CurrentHealth} / {TotalMaxHealth()}");
            sb.AppendLine($"Damage Output: {TotalMinDamage()}-{TotalMaxDamage()}");
            sb.AppendLine($"Damage Reduction: {TotalDamageReduction()}");
            sb.AppendLine($"Base Hit Chance: {TotalBaseHitChance()}");
            sb.AppendLine($"Base Dodge Chance: {TotalDodgeChance()}");
            Console.WriteLine(sb.ToString());
        }

        public void printEquipment()
        {
            var sb = new StringBuilder();
            sb.AppendLine("EQUIPPED");
            sb.AppendLine("-------\n");
            foreach(EquipmentType equiped in Enum.GetValues(typeof(EquipmentType)))
            {
                sb.AppendLine($"{equiped}:\t{Equipment.FirstOrDefault(x => x.IsEquiped && x.Type == equiped)?.Name ?? string.Empty}");
            }
            sb.AppendLine();
            sb.AppendLine("IN INVENTORY");
            sb.AppendLine("------------\n");
            foreach (var equipment in Equipment.Where(x => x.IsEquiped == false).OrderBy(x => x.Type))
            {
                sb.AppendLine($"{equipment.Name}\tSlot: {equipment.Type}");
            }
            Console.WriteLine(sb.ToString());
        }
        #endregion

    }


}
