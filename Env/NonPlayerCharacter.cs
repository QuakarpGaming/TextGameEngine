using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGameEngine.Env
{
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
        }
        public NonPlayerCharacter(string name,string description)
        {
            Name = name;
            Description = description;
            Inventory = new List<Item>();
            Responses = new Dictionary<string, string>();
            NamePrivate = this.Name.ToUpper();
        }
        public NonPlayerCharacter(string name, string description, List<Item> inventory) : this(name, description)
        {
            Inventory = inventory;
            Responses = new Dictionary<string, string>();
        }

        public NonPlayerCharacter(string name, string description, List<Item> inventory, Dictionary<string, string> responses) : this(name, description, inventory)
        {
            Responses = responses;
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
            sb.Append(".");
            return sb.ToString();
        }
        #endregion
    }
}
