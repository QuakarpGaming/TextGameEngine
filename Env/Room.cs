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
        }

        public Room(List<Exit>  exits)
        {
            this.Exits = exits;
            this.FloorItems = new List<Item>();
            this.Description = string.Empty;
            this.RoomCode = string.Empty;
            this.Name = string.Empty;
            this.RoomCodePrivate = string.Empty;
            this.canKill = false;
            this.preventKill = new List<string>();
            this.KillMsg = string.Empty;
        }


        public Room(List<Item> items)
        {
            this.Exits = new List<Exit> ();
            this.FloorItems = items;
            this.Description = string.Empty;
            this.RoomCode = string.Empty;
            this.Name = string.Empty;
            this.RoomCodePrivate = string.Empty;
            this.canKill = false;
            this.KillMsg = string.Empty;
            this.preventKill = new List<string>();
        }

        public Room(List<Exit>  Exits, List<Item> Items)
        {
            this.Exits = Exits;
            this.FloorItems = Items;
            this.Description = string.Empty;
            this.RoomCode = string.Empty;
            this.Name = string.Empty;
            this.RoomCodePrivate = string.Empty;
            this.canKill = false;
            this.KillMsg = string.Empty;
            this.preventKill = new List<string>();
        }

        public Room(string code)
        {
            this.Exits = new List<Exit> ();
            this.FloorItems = new List<Item>();
            this.Description = string.Empty;
            this.RoomCode = code;
            this.Name = string.Empty;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.KillMsg = string.Empty;
            this.canKill = false;
            this.preventKill = new List<string>();
        }

        public Room(string code, string name) 
        {
            this.Exits = new List<Exit> ();
            this.FloorItems = new List<Item>();
            this.Description = string.Empty;
            this.RoomCode = code;
            this.Name = name;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.canKill = false;
            this.KillMsg = string.Empty;
            this.preventKill = new List<string>();
        }

        public Room(string code,string name,string desc)
        {
            this.Exits = new List<Exit> ();
            this.FloorItems = new List<Item>();
            this.Description = desc;
            this.RoomCode = code;
            this.Name = name;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.canKill = false;
            this.KillMsg = string.Empty;
            this.preventKill = new List<string>();

        }
        public Room(string code, string name, string desc,List<Exit> exits,List<Item> floorItems)
        {
            this.Exits = exits;
            this.FloorItems = floorItems;
            this.Description = desc;
            this.RoomCode = code;
            this.Name = name;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.canKill = false;
            this.preventKill = new List<string>();
            this.KillMsg = string.Empty;
        }

        public Room(string code, string name, string desc,List<Exit> exits)
        {
            this.Exits = exits;
            this.FloorItems = new List<Item>();
            this.Description = desc;
            this.RoomCode = code;
            this.Name = name;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.canKill = false;
            this.preventKill = new List<string>();
            this.KillMsg = string.Empty;
        }
        public Room(string code, string name, string desc,List<Item> floorItems)
        {
            this.Exits = new List<Exit>();
            this.FloorItems = floorItems;
            this.Description = desc;
            this.RoomCode = code;
            this.Name = name;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.canKill = false;
            this.preventKill = new List<string>();
            this.KillMsg = string.Empty;

        }
        public Room(string code, string name, string desc, List<Exit> exits, List<Item> floorItems,bool canKill, List<string> preventKill, string killMsg)
        {
            this.Exits = exits;
            this.FloorItems = floorItems;
            this.Description = desc;
            this.RoomCode = code;
            this.Name = name;
            this.RoomCodePrivate = this.RoomCode.ToUpper();
            this.canKill = canKill;
            this.preventKill = preventKill;
            this.KillMsg = killMsg;
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
        public string PrintExits()
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

        public string PrintFloorItems()
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

        public string PrintRoom()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.Name) == false)
            {
                sb.AppendLine("Location: " + this.Name);
                sb.AppendLine();
            }
            if (string.IsNullOrWhiteSpace(this.Description) == false)
            {
                sb.AppendLine(this.Description);
                sb.AppendLine();
            }
            if (this.FloorItems.Count > 0)
            {
                sb.AppendLine("Items: " + this.PrintFloorItems());
                sb.AppendLine();
            }
            if (this.Exits.Count > 0)
            {
                sb.AppendLine("Exits: " + this.PrintExits());
            }
            return sb.ToString().Trim();
        }
        public string PrintRoom(string locationTag = "",string descTag = "", string itemsTag = "", string exitTag = "")
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.Name) == false)
            {
                sb.AppendLine(locationTag + this.Name);
                sb.AppendLine();
            }
            if (string.IsNullOrWhiteSpace(this.Description) == false)
            {
                sb.AppendLine(descTag + this.Description);
                sb.AppendLine();
            }
            if (this.FloorItems.Count > 0)
            {
                sb.AppendLine(itemsTag + this.PrintFloorItems());
                sb.AppendLine();
            }
            if (this.Exits.Count > 0)
            {
                sb.AppendLine(exitTag + this.PrintExits());
            }
            sb.AppendLine();
            return sb.ToString();
        }
        #endregion
    }
}
