using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextGameEngine.Env;
using TextGameEngine.Player;

namespace TextGameEngine.Game
{
    public class TheGame
    {
        #region Contructors
        public TheGame()
        {
            this.Rooms = new List<Room>();
            this.CurrentRoomCode = string.Empty;
            this.CurrentRoomCodePrivate = string.Empty;
            this.Errors = new List<string>();
            this.PlayInv = new List<Item>();
            this.InputPrefix = "What Will You Do: ";
            this.MoveRegex = new Regex(@"\b(GO TO|GO|ENTER|MOVE|LEAVE|GOTO)\b");
            this.LookAtItemRegex = new Regex(@"\b(LOOK AT|CHECK OUT|CHECK|LOOK|BEHOLD|EXAMINE|SCAN|VIEW|SPOT)\b");
            this.PickUpItemRegex = new Regex(@"\b(GET|TAKE|PICKUP|LOOT|STEAL|PICK UP)\b");
            this.DroppedItemRegex = new Regex(@"\b(DROP|PLACE|REMOVE|ABANDON)\b");
            this.QuitRegex = new Regex(@"\b(QUIT|DESKTOP|ALT-F4)\b");
            this.InvRegex = new Regex(@"\b(INV|INVENTORY|POCKETS|BACKPACK|CHECK INV|CHECK INVENTORY)\b");
            this.FromNPCRegex = new Regex(@"\b( FROM )\b");
            this.LookRoomRegex = new Regex(@"\b(ROOM|AROUND)");
            this.AskNPCRegex = new Regex(@"\b(ASK ABOUT|ASK|TALK ABOUT)\b");
            this.AboutRegex = new Regex(@"\b( ABOUT )");
            this.PickedUpItemMsg = "You picked up the {item}.";
            this.DroppedItemMsg = "You drop the {item}.";
            this.MissPickUpMsg = "There Is no {item} in this room.";
            this.MissDropMsg = "You do not have {item} to drop.";
            this.missLookAtItem = "You look for {item}, but it is not in your INVENTORY or the room.";
            this.MissNPCResponse = "Sorry I do not know anything about that.";
            this.GetItemFromNPCMsg = "You got the {item} from {npc}";
            this.PrintRoomDuringLoop = true;
            this.WinningRoomCodePrivate = string.Empty;
            this.WinningItems = new List<Item>();
            this.GameState = "CONT";
            this.WhatKilledCode = string.Empty;
            this.WhereKilledCode = string.Empty;
            this.WinningMsg = string.Empty;
            this.Player = new PlayerClass();
        }
        #endregion

        #region Data
        public List<Room> Rooms { get; set; }
        public string CurrentRoomCode { get { return this.CurrentRoomCodePrivate; } set { this.CurrentRoomCodePrivate = value.ToUpper(); } }
        private string CurrentRoomCodePrivate { get; set; }
        public List<string> Errors { get; set; }
        public List<Item> PlayInv {  get; set; }
        public string InputPrefix {  get; set; }
        public string PickedUpItemMsg {  get; set; }
        public string GetItemFromNPCMsg {  get; set; }
        public string DroppedItemMsg {  get; set; }
        public string MissPickUpMsg {  get; set; }
        public string MissDropMsg { get; set; }
        public string MissNPCResponse {  get; set; }

        public string missLookAtItem {  get; set; }
        public Regex MoveRegex { get; set; }
        public Regex LookAtItemRegex { get; set; }

        public Regex PickUpItemRegex { get; set; }
        public Regex DroppedItemRegex { get; set; }
        public Regex QuitRegex { get; set; }
        public Regex InvRegex { get; set; }
        public Regex LookRoomRegex { get; set; }
        public Regex FromNPCRegex { get; set; }
        public Regex AskNPCRegex { get; set; }
        public Regex AboutRegex { get; set; }
        private bool PrintRoomDuringLoop { get; set; }

        private string WinningRoomCodePrivate { get; set; }
        public string WinningRoomCode { get { return this.WinningRoomCodePrivate; } set { this.WinningRoomCodePrivate = value.ToUpper(); } }

        public List<Item> WinningItems { get; set; }
        public string GameState { get; set; }
        private string WhereKilledCode { get; set; }
        private string WhatKilledCode { get; set; }
        public string WinningMsg { get; set; }
        public PlayerClass Player { get; set; }
        #endregion

        #region Game Play Functions
        #region Rooms
        public bool MoveRoom(string exitDesc)
        {
            
            var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);
            var codeToMoveTo = string.Empty;
            this.Errors.Clear();
            if (currentRoom != null)
            {
                //does the room code exist
                var currentExit = currentRoom.Exits.FirstOrDefault(x => x.ToRoomDiscription.ToUpper() == exitDesc.ToUpper());
                if (currentExit != null)
                {
                    codeToMoveTo = currentExit.ToRoomCode;
                    if (currentExit.IsLocked)
                    {
                        if (PlayInv.Any(x => x.Code == currentExit.KeyItemCode))
                        {
                            currentExit.IsLocked = false;
                        }
                        else
                        {
                            this.Errors.Add("{locked}");
                            Console.WriteLine(currentExit.LockMsg ?? "Locked");
                        }
                    }
                }
                else
                {
                    this.Errors.Add("{not_found}");
                }
            }
            else
            {
                this.Errors.Add("{not_found}");
            }

            if(this.Errors.Count == 0)
            {
                this.CurrentRoomCode = codeToMoveTo;
                var newRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == codeToMoveTo);
                if (newRoom != null && newRoom.canKill)
                {
                    if (newRoom.preventKill.Count == 0)
                    {
                        this.GameState = "DEAD";
                    }
                    else
                    {
                        var newGameState = "DEAD";
                        foreach (var item in newRoom.preventKill)
                        {
                            if (PlayInv.Any(x => x.Code == item))
                            {
                                newGameState = "CONT";
                                break;
                            }
                        }

                        this.WhereKilledCode = newGameState == "DEAD" ? codeToMoveTo : string.Empty;
                        this.GameState = newGameState;
                    }
                }
            }

            return this.Errors.Count == 0;
        }
        public void PrintFloorItemDesc(string code)
        {
            code = code?.ToUpper() ?? string.Empty;
            var item = this.Rooms.FirstOrDefault(x=> x.RoomCode == this.CurrentRoomCode)?.FloorItems.FirstOrDefault(x => x.Code == code);
            if (item != null)
            {
                Console.WriteLine(item.Description);
            }
            else
            {
                Console.WriteLine($"There is no {code} here.");
            }
        }
        #endregion

        #region Player Inv
        private void PrintPlayerInv()
        {
            Console.WriteLine("You currently Have: \n");
            foreach (var item in this.PlayInv)
                Console.WriteLine(item.Code);
        }

        private void PrintItemDesc(string ItemCode)
        {
            var desc = string.Empty;
            var item = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode)?.FloorItems.FirstOrDefault(x => x.Code == ItemCode);
            if (item != null)
            {
                desc = item.Description;
            }
            else
            {
                item = this.PlayInv.FirstOrDefault(x => x.Code == ItemCode);
                if (item != null)
                {
                    desc = item.Description;
                }
            }

            if (string.IsNullOrEmpty(desc))
            {
                Console.WriteLine(this.missLookAtItem.Replace("{item}", ItemCode));
            }
            else
            {
                Console.WriteLine(desc);
            }
        }
        private void PickUpItem(string input)
        {
            input = input.ToUpper() ?? string.Empty;
            var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);
            if (currentRoom != null)
            {
                var floorItem = currentRoom.FloorItems.FirstOrDefault(x => x.Code == input.ToUpper());
                if (floorItem != null)
                {
                    ItemKillYou(currentRoom, floorItem);
                }
                else
                {
                    Item? NPCItem = null;
                    var foundItem = false;
                    if (FromNPCRegex.IsMatch(input))
                    {
                        var inputParts = FromNPCRegex.Split(input);
                        if (inputParts.Length >= 3)
                        {
                            var npc = currentRoom.NonPlayerCharacters.FirstOrDefault(x => x.Name == inputParts[2]);
                            if (npc != null)
                            {
                                var item = npc.Inventory.FirstOrDefault(x => x.Code == inputParts[0]);
                                if (item != null)
                                {
                                    ItemKillYou(npc, item);
                                }
                                else
                                {
                                    Console.WriteLine($"{npc.Name} Does Not Have {inputParts[0]}.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"{inputParts[2]} is not here to give you {inputParts[0]}.");
                            }
                        }
                    }
                    else
                    {
                        foreach (NonPlayerCharacter npc in currentRoom.NonPlayerCharacters)
                        {
                            NPCItem = npc.Inventory.FirstOrDefault(x => x.Code == input);
                            if (NPCItem != null)
                            {
                                ItemKillYou(npc, NPCItem);
                                foundItem = true;
                                break;
                            }
                        }
                    }
                    if(!foundItem)
                        Console.WriteLine(MissPickUpMsg.Replace("{item}", input));
                }
            }
        }

        private void AskNPC(string input)
        {
            var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);
            if (currentRoom != null)
            {
                if (AboutRegex.IsMatch(input))
                {
                    var inputParts = AboutRegex.Split(input);
                    if (inputParts.Length >= 3)
                    {
                        var npc = currentRoom.NonPlayerCharacters.FirstOrDefault(x => x.Name == inputParts[0]);
                        if (npc != null)
                        {
                            if (npc.Responses.TryGetValue(inputParts[2], out var response))
                            {
                                Console.WriteLine(response.ToString());
                            }
                            else
                            {
                                Console.WriteLine(MissNPCResponse);
                            }
                        }
                    }
                }
                else
                {
                    var response = string.Empty;
                    foreach (var npc in currentRoom.NonPlayerCharacters)
                    {
                        if (npc.Responses.TryGetValue(input, out response))
                        {
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(response))
                    {
                        Console.WriteLine(MissNPCResponse);
                    }
                    else
                    {
                        Console.WriteLine(response);
                    }
                }
            }
        }

        private void ItemKillYou(Room currentRoom,Item item)
        {
            PlayInv.Add(item);
            currentRoom.FloorItems.Remove(item);
            Console.WriteLine(PickedUpItemMsg.Replace("{item}", item.Code));
            if (item.CanKill)
            {
                var newGameState = "DEAD";
                if (item.PreventKill.Count == 0)
                {
                    WhatKilledCode = item.Code;
                }
                else
                {
                    newGameState = "DEAD";
                    foreach (var itemCode in item.PreventKill)
                    {
                        if (PlayInv.Any(x => x.Code == itemCode))
                        {
                            newGameState = "CONT";
                            break;
                        }
                    }

                    this.WhatKilledCode = newGameState == "DEAD" ? item.Code : string.Empty;
                }
                this.GameState = newGameState;
            }
        }
        private void ItemKillYou(NonPlayerCharacter npc, Item item)
        {
            PlayInv.Add(item);
            npc.Inventory.Remove(item);
            Console.WriteLine(GetItemFromNPCMsg.Replace("{item}", item.Code).Replace("{npc}",npc.Name));
            if (item.CanKill)
            {
                var newGameState = "DEAD";
                if (item.PreventKill.Count == 0)
                {
                    WhatKilledCode = item.Code;
                }
                else
                {
                    newGameState = "DEAD";
                    foreach (var itemCode in item.PreventKill)
                    {
                        if (PlayInv.Any(x => x.Code == itemCode))
                        {
                            newGameState = "CONT";
                            break;
                        }
                    }

                    this.WhatKilledCode = newGameState == "DEAD" ? item.Code : string.Empty;
                }
                this.GameState = newGameState;
            }
        }
        private void DropItem(string code)
        {
            code = code.ToUpper() ?? string.Empty;
            var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);
            if (currentRoom != null)
            {
                var item = this.PlayInv.FirstOrDefault(x => x.Code == code.ToUpper());
                if (item != null)
                {
                    currentRoom.FloorItems.Add(item);
                    this.PlayInv.Remove(item);
                    Console.WriteLine(DroppedItemMsg.Replace("{item}", item.Code));
                }
                else 
                    Console.WriteLine(MissDropMsg.Replace("{item}", code));
                
            }
        
        }

        
        #endregion

        #region Input
        private string GetInput()
        {
            string input = string.Empty;
            do
            {
                Console.Write(this.InputPrefix);
                input = (Console.ReadLine() ?? string.Empty).ToUpper();
            } while (!(string.IsNullOrWhiteSpace(input) == false));
            return input;
        }

        private void ActInput(string input)
        {
            var strippedInput = string.Empty;
            if (MoveRegex.IsMatch(input))
            {
                strippedInput = Regex.Replace(input, MoveRegex.ToString(), "").Trim();
                PrintRoomDuringLoop = MoveRoom(strippedInput);
            }
            else if (LookAtItemRegex.IsMatch(input))
            {
                strippedInput = Regex.Replace(input, LookAtItemRegex.ToString(), "").Trim();
                if (string.IsNullOrWhiteSpace(strippedInput) || LookRoomRegex.IsMatch(strippedInput))
                {
                    PrintCurrentRoom();
                }
                else
                {
                    var npc = this.Rooms.FirstOrDefault(r => r.RoomCode == CurrentRoomCode)?.NonPlayerCharacters.FirstOrDefault(x => x.Name == strippedInput);
                    if (npc != null)
                    {
                        Console.WriteLine(npc.PrintLookedAt());
                    }
                    else
                    {
                        PrintItemDesc(strippedInput);
                    }
                }
            }
            else if (PickUpItemRegex.IsMatch(input))
            {
                strippedInput = Regex.Replace(input, PickUpItemRegex.ToString(), "").Trim();
                PickUpItem(strippedInput);
            }
            else if (DroppedItemRegex.IsMatch(input))
            {
                strippedInput = Regex.Replace(input, DroppedItemRegex.ToString(), "").Trim();
                DropItem(strippedInput);
            }

            else if (InvRegex.IsMatch(input))
            {
                PrintPlayerInv();
            }
            else if (AskNPCRegex.IsMatch(input))
            {
                strippedInput = Regex.Replace(input, AskNPCRegex.ToString(), "").Trim();
                AskNPC(strippedInput);
            }
        }
        #endregion

        #region Death and Taxes
        private string CheckGameState()
        {
            if(GameState == "DEAD")
                return GameState; 
            
            if (CurrentRoomCode == WinningRoomCodePrivate)
            {
                foreach(var item in this.WinningItems)
                {
                    if (PlayInv.Any(x => x.Code == item.Code) == false)
                        return "CONT";
                }
                return "WIN";
            }

            return "CONT";
        }

        private bool BreakGamePlayLoop(string input)
        {
            var breakLoop = false;

            if(this.GameState == "DEAD" ||  this.GameState =="WIN")
                breakLoop = true;

            else if (this.QuitRegex.IsMatch(input))
                breakLoop = true;


            return !breakLoop;
        }

        private void DisplayGameEndMsg()
        {
            Console.Clear();

            switch (this.GameState)
            {
                case "DEAD":
                    //tell player
                    Console.WriteLine("YOU ARE DEAD!\n");
                    //checking if a room killed you
                    if (string.IsNullOrWhiteSpace(this.WhereKilledCode) == false)
                    {
                        //find room
                        var killedRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == WhereKilledCode.ToUpper());
                        if (killedRoom != null)
                        {
                            //tell player why they are dead 
                            Console.WriteLine(killedRoom.KillMsg);
                        }
                    }
                    else if (string.IsNullOrEmpty(this.WhatKilledCode) == false)
                    {
                        var killedItem = this.PlayInv.FirstOrDefault(x => x.Code == WhatKilledCode.ToUpper());
                        if (killedItem != null)
                        {
                            Console.WriteLine(killedItem.KillMsg);
                        }
                    }
                    break;

                case "WIN":
                    Console.WriteLine("YOU WIN!\n");
                    if (string.IsNullOrWhiteSpace(this.WinningMsg) == false)
                    {
                        Console.WriteLine(this.WinningMsg);
                    }
                    break;
            }

            Console.WriteLine("\n\nThank you so much for playing, press the enter key to exit.");
        }
        #endregion
        #endregion

        #region Game Play Loop
        public void GamePlayLoop()
        {
            var input = string.Empty;
            
            do
            {
                if (this.PrintRoomDuringLoop)
                {
                    PrintCurrentRoom();
                    this.PrintRoomDuringLoop = false;
                }
                input = GetInput();
                ActInput(input);
                this.GameState = CheckGameState();
                
            }while(BreakGamePlayLoop(input));

            DisplayGameEndMsg();
        }
        #endregion

        #region Prints for Debugging
        public void printListOfRooms()
        {
            foreach (var room in this.Rooms)
            {
                Console.WriteLine(room.PrintRoom());

            }
        }

        public void PrintCurrentRoom()
        {
            var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);

            if (currentRoom != null)
            {
                Console.Clear();
                Console.WriteLine(currentRoom.PrintRoom());
            }
            else
            {
                Console.WriteLine("you are outside the programmers time and space.");
            }
        }
        #endregion
    }
}
