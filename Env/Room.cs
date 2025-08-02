using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TextGameEngine.Env
{
    public class Room
    {
        #region Constructors
        public Room() 
        {
            this.Exits = new List<Exit> ();
            this.FloorItems = new List<Item>();
            this.Description = string.Empty;
            this.RoomCode = string.Empty;
            this.Name = string.Empty;
            this.RoomCodePrivate = string.Empty;
            this.canKill = false;
            this.preventKill = new List<string> ();
            this.KillMsg = string.Empty;
            this.NonPlayerCharacters = new List<NonPlayerCharacter> ();
            this.LocationTag = "Location: ";
            this.NPCsTag = "People in Room: ";
            this.ItemsTag = "Items: ";
            this.DescTag = string.Empty;
            this.ExitTag = "Exits: ";
        }
        public Room(List<NonPlayerCharacter> NPCs) : this()
        {
            this.NonPlayerCharacters = NPCs;
        }
        public Room(List<Exit>  exits) : this()
        {
            this.Exits = exits;
        }

        public Room(List<Exit> exits, List<NonPlayerCharacter> NPCs) :this(exits)
        {
            this.NonPlayerCharacters = NPCs;
        }


        public Room(List<Item> items) : this()
        {
            this.FloorItems = items;
        }
        public Room(List<Item> items, List<NonPlayerCharacter> NPCs) : this(items)
        {
            this.NonPlayerCharacters = NPCs;
        }
        public Room(List<Exit>  Exits, List<Item> Items) :this()
        {
            this.Exits = Exits;
            this.FloorItems = Items;
        }

        public Room(List<Exit> Exits, List<Item> Items, List<NonPlayerCharacter> NPCs) : this(Exits,Items)
        {
            this.NonPlayerCharacters = NPCs;
        }
        public Room(string code) : this()
        {
            this.RoomCode = code;
        }

        public Room(string code, List<NonPlayerCharacter> NPCs) : this(code)
        {
            this.NonPlayerCharacters = NPCs;
        }

        public Room(string code, string name)  :this()
        {
            this.RoomCode = code;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.Name = name;
        }
        public Room(string code, string name, List<NonPlayerCharacter> NPCs) : this(code,name)
        {
            this.NonPlayerCharacters = NPCs;
        }

        public Room(string code,string name,string desc) :this(code, name)
        {
            this.Description = desc;
        }
        public Room(string code, string name, string desc, List<NonPlayerCharacter> NPCs) : this(code, name,desc)
        {
            this.NonPlayerCharacters = NPCs;
        }

        public Room(string code, string name, string desc,List<Exit> exits,List<Item> floorItems) :this(code, name, desc)
        {
            this.Exits = exits;
            this.FloorItems = floorItems;
        }
        public Room(string code, string name, string desc, List<Exit> exits, List<Item> floorItems, List<NonPlayerCharacter> NPCs) : this(code, name, desc,floorItems)
        {
            this.NonPlayerCharacters = NPCs;
        }

        public Room(string code, string name, string desc,List<Exit> exits) :this(code, name, desc) 
        {
            this.Exits = exits;
        }
        public Room(string code, string name, string desc, List<Exit> exits, List<NonPlayerCharacter> NPCs) : this(code, name, desc, exits)
        {
            this.NonPlayerCharacters = NPCs;
        }
        public Room(string code, string name, string desc,List<Item> floorItems) : this(code, name, desc) 
        {
            this.FloorItems = floorItems;
        }
        public Room(string code, string name, string desc, List<Item> floorItems, List<NonPlayerCharacter> NPCs) : this(code, name, desc,floorItems)
        {
            this.NonPlayerCharacters = NPCs;
        }

        public Room(string code, string name, string desc, List<Exit> exits, List<Item> floorItems,bool canKill, List<string> preventKill, string killMsg): this(code, name, desc, exits, floorItems)
        {
            this.canKill = canKill;
            this.preventKill = preventKill;
            this.KillMsg = killMsg;
        }
        public Room(string code, string name, string desc, List<Exit> exits, List<Item> floorItems, bool canKill, List<string> preventKill, string killMsg, List<NonPlayerCharacter> NPCs) : this(code, name, desc, exits, floorItems,canKill, preventKill,killMsg)
        {
            this.NonPlayerCharacters = NPCs;
        }


        #endregion

            #region Data - Public
        public string RoomCode { get { return this.RoomCodePrivate; } set { this.RoomCodePrivate = value.ToUpper(); } }
        private string RoomCodePrivate { get; set; }
        public string Name { get; set; }
        public string Description {  get; set; }
        public List<Exit> Exits { get; set; }

        public List<Item> FloorItems { get; set; }
        public bool canKill {  get; set; }
        public List<string> preventKill { get; set; }
        public string KillMsg {  get; set; }
        public List<NonPlayerCharacter> NonPlayerCharacters { get; set; }
        public string LocationTag { get; set; }
        public string DescTag { get; set; }
        public string ItemsTag { get; set; }
        public string NPCsTag { get; set; }
        public string ExitTag { get; set; }
        #endregion

        #region Setters and Getters
        //setters and getters
        public void AddOrUpdateRoomExit(string exitCode, string exitText, string keyItem = "" ,bool isLocked = false)
        {
            var exit = this.Exits.FirstOrDefault(x => x.ToRoomCode == exitCode);
            if (exit != null)
            {
                exit.ToRoomDiscription = exitText;
                exit.IsLocked = isLocked;
                exit.KeyItemCode = keyItem;
            }
            else
            {
                this.Exits.Add(new Exit(exitCode.ToUpper(), exitText.ToUpper(),keyItem.ToUpper(),isLocked));
            }
        }
        public void RemoveExit(string exitCode)
        {
            this.Exits.RemoveAll(x => x.ToRoomCode == exitCode.ToUpper());
        }

        public void AddorUpdateFloorItem(string code,string desc)
        {
            var item = this.FloorItems.FirstOrDefault(x => x.Code == code);

            if (item != null)
            {
                item.Description = desc;
            }
            else 
            {
                this.FloorItems.Add(new Item(code, desc));
            }
        }

        public void RemoveFloorItem(string code)
        {
            this.FloorItems.RemoveAll(x => x.Code == code.ToUpper());
        }
        #endregion

        #region Death and Taxes

        #endregion
        #region Printers
        //prints
        private string PrintExits()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var exit in this.Exits)
            {
                if (sb.Length == 0)
                {
                    sb.Append(exit.ToRoomDiscription);
                }
                else
                {
                    sb.Append(", " + exit.ToRoomDiscription);
                }
            }
            
            sb.Append(".");
            return sb.ToString();
        }

        private string PrintFloorItems()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.FloorItems)
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

        private string PrintNPCs()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var npc in this.NonPlayerCharacters)
            {
                if (sb.Length == 0)
                {
                    sb.Append(npc.Name);
                }
                else
                {
                    sb.Append(", " + npc.Name);
                }
            }
            sb.Append(".");
            return sb.ToString();
        }
        public string PrintRoom()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.Name) == false)
            {
                sb.AppendLine(this.LocationTag + this.Name);
                sb.AppendLine();
            }
            if (string.IsNullOrWhiteSpace(this.Description) == false)
            {
                sb.AppendLine(this.DescTag + this.Description);
                sb.AppendLine();
            }
            if (this.NonPlayerCharacters.Count > 0)
            {
                sb.AppendLine(this.NPCsTag + PrintNPCs());
                sb.AppendLine();
            }
            if (this.FloorItems.Count > 0)
            {
                sb.AppendLine(this.ItemsTag + this.PrintFloorItems());
                sb.AppendLine();
            }
            if (this.Exits.Count > 0)
            {
                sb.AppendLine(this.ExitTag + this.PrintExits());
            }
            return sb.ToString();
        }
        #endregion
    }
}
