using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextGameEngine.Player;

namespace TextGameEngine.Env
{
    public class Shop
    {
        #region constructors
        public Shop(string? name,int gold = 255,List<Item>? items = null, List<Equipment>? equipment = null,string? thankYouMsg = null)
        {
            this.Name = name?.ToUpper() ?? string.Empty;
            this.Gold = gold;
            this.Items = items ?? [];
            this.Equipment = equipment ?? [];
            this.ThankYouString = thankYouMsg ?? "Hehe, Thnak you!";
        }
        #endregion

        #region Public Data
        public string Name { get; set; }
        public int Gold { get; set; }
        public List<Item> Items { get; set; }
        public List<Equipment> Equipment { get; set; }
        public string ThankYouString { get; set; }
        #endregion

        #region Prints
        public void PrintNameGold()
        {
            Console.WriteLine($"Welcome to {this.Name}\n\nShop Gold: {this.Gold}\n\n");
        }
        public void PrintItems()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ITEMS\n\n");
            foreach (var item in Items)
            {
                sb.AppendLine($"{item.Code} - {item.Gold} Gold Pieces");
            }

            Console.WriteLine(sb.ToString());
        }
        public void PrintEquipment()
        {
            var sb = new StringBuilder();
            sb.AppendLine("EQUIPMENT\n\n");
            foreach (var eq in Equipment)
            {
                sb.AppendLine($"{eq.Name} - {eq.Gold} Gold Pieces");
            }
            Console.WriteLine(sb.ToString());

        }

        public void PrintStore()
        {
            PrintNameGold();
            if (Items.Count > 0)
                PrintItems();

            if (Equipment.Count > 0)
                PrintEquipment();
        }
        #endregion
    }
}
