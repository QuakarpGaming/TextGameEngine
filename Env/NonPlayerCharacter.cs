using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextGameEngine.Player;

namespace TextGameEngine.Env
{
    public enum WhenToFight
    {
        never,
        enter,
        leave,
        ask
    }
    public class NonPlayerCharacter
    {
        #region Constructors
        public NonPlayerCharacter() 
        {
            Name = string.Empty;
            Description = string.Empty;
            Inventory = new List<Item>();
            Responses = new Dictionary<string, string>();
            NamePrivate = this.Name;
            DroppedOnDeathEquipment = [];
        }

        public NonPlayerCharacter(string name, string? description = null, List<Item>? inventory = null, Dictionary<string, string>? responses = null,int currentHealth = 10,int maxhealth = 10,int minDamage = 1, int maxDamage = 3
                                 ,bool atLeast1Dmg = true, int damageReduction = 0,int hitchance = 90,int dodgeChance = 5,bool willFight = false, WhenToFight whenToFight = WhenToFight.never ,List<Equipment>? droppedOnDeathEquipment = null,
                                 int droppedGold = 0) 
        {
            Name = name.ToUpper();
            Description = description ?? string.Empty;
            Inventory = inventory ?? new List<Item>();
            Responses = responses ?? new Dictionary<string, string>();
            NamePrivate = this.Name;
            CurrentHealth = currentHealth <= maxhealth ? currentHealth : maxhealth;
            MaxHealth = maxhealth;
            MinDamageOutput = minDamage;
            MaxDamageOutput = maxDamage;
            TakeAtLeastOneDamage = atLeast1Dmg;
            DamageReduction = damageReduction;
            HitChance = hitchance;
            DodgeChance = dodgeChance;
            WillFight = willFight;
            WhenToFight = whenToFight;
            DroppedOnDeathEquipment = droppedOnDeathEquipment ?? [];
            DroppedGold = droppedGold;
        }

        #endregion

        #region Data - Public
        public string Name { get { return NamePrivate.ToUpper(); } set { this.NamePrivate = value.ToUpper(); } }
        public string Description { get; set; }

        public List<Item> Inventory { get; set; }
        
        public Dictionary<string,string> Responses { get; set; }
        #endregion
        #region Data Private
        private string NamePrivate { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }

        public int MinDamageOutput { get; set; }
        public int MaxDamageOutput { get; set; }
        public int DamageReduction { get; set; }
        public bool TakeAtLeastOneDamage { get; set; }
        public int HitChance { get; set; }
        public int DodgeChance { get; set; }
        public bool WillFight { set; get; }
        public WhenToFight WhenToFight { get; set; }
        public List<Equipment> DroppedOnDeathEquipment { get; set; }
        public int DroppedGold { get; set; }
        #endregion
        #region Printers
        public string PrintLookedAt()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("You see " + this.Name);
            sb.AppendLine(this.Description);
            if(this.Inventory.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("They are holding: " + printInv());
            }

            return sb.ToString();
        }
        public void printCombatStats()
        {
            var sb = new StringBuilder();
            sb.AppendLine("You see " + this.Name);
            sb.AppendLine();
            sb.AppendLine("Combat Stats:");
            sb.AppendLine("=============");
            sb.AppendLine($"HP:\t\t{CurrentHealth} / {MaxHealth}");
            sb.AppendLine($"Damage:\t\t{MinDamageOutput} - {MaxDamageOutput}");
            sb.AppendLine($"Take 1:\t\t{TakeAtLeastOneDamage.ToString()}");
            sb.AppendLine($"DR:\t\t{DamageReduction}");
            sb.AppendLine($"Hit Chance:\t{HitChance}");
            sb.AppendLine($"Dodge Chance:\t{DodgeChance}");
            sb.AppendLine($"Will Fight:\t{WillFight.ToString()} when {WhenToFight.ToString()}");
            Console.WriteLine(sb.ToString());
        }
        private string printInv()
        {
            var sb = new StringBuilder();
            foreach (Item item in Inventory)
            {
                if (sb.Length == 0)
                {
                    sb.Append(item.Code);
                }
                else
                {
                    sb.Append(", " + item.Code);
                }
            }
            sb.Append('.');
            return sb.ToString();
        }
        

        public void printAll()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {this.Name}");
            sb.AppendLine();
            sb.AppendLine($"Description: {this.Description}");
            sb.AppendLine();
            sb.AppendLine("Inventory:");
            foreach (Item item in Inventory)
            {
                sb.AppendLine($"\t{item.Code}\t-\t{item.Description}");
            }
            sb.AppendLine();
            sb.AppendLine("Responses:");
            foreach (var key in Responses.Keys)
            {
                sb.AppendLine($"\t{key}\t-\t{Responses[key]}");
            }
            sb.AppendLine();
            sb.AppendLine("Combat Stats:");
            sb.AppendLine($"HP:\t\t{CurrentHealth} / {MaxHealth}");
            sb.AppendLine($"Damage:\t\t{MinDamageOutput} - {MaxDamageOutput}");
            sb.AppendLine($"Take 1:\t\t{TakeAtLeastOneDamage.ToString()}");
            sb.AppendLine($"DR:\t\t{DamageReduction}");
            sb.AppendLine($"Hit Chance:\t{HitChance}");
            sb.AppendLine($"Dodge Chance:\t{DodgeChance}");
            sb.AppendLine($"Will Fight:\t{WillFight.ToString()} when {WhenToFight.ToString()}");
            Console.WriteLine(sb.ToString());
        }
        #endregion
    }
}
