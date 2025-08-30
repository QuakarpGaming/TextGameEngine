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
            this.CurrentRoomCode = "START";
            this.CurrentRoomCodePrivate = "START";
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
            this.WearEquimentRegex = new Regex(@"\b(WEAR|PUT ON|EQUIP|DRAW|DAWN|HOLD)\b");
            this.RemoveEquimentRegex = new Regex(@"\b(REMOVE|TAKE OFF|STORE|UNEQUIP)\b");
            this.SelfRegex = new Regex(@"\b(SELF|ME|STATUS|STAT|STATS)\b");
            this.PickedUpItemMsg = "You picked up the {item}.";
            this.DroppedItemMsg = "You drop the {item}.";
            this.MissPickUpMsg = "There is no {item} in this room.";
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
            this.WhoKilledCode = string.Empty;
            this.WinningMsg = string.Empty;
            this.Player = new PlayerClass();
            this.StartCombat = false;
            this.ListOfFoesPrefix = "Remaining foes: ";
            this.StartOfCombatString = "You are under attack!";
            this.AttackNPCRegex = new Regex(@"\b(ATTACK|HIT|STRIKE|BONK|PIMP SLAP|BITCH SLAP|SLAP)\b");
            this.MissAttackString = "Missed!";
            this.KIAstring = "You start to fall to the ground, dead before you reach it.";
            this.LootItems = [];
            this.addRemoveHealthOnEquipChange = true;
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
        public Regex WearEquimentRegex { get; set; }
        public Regex RemoveEquimentRegex { get; set; }
        public Regex SelfRegex { get; set; }
        public bool addRemoveHealthOnEquipChange { get; set; }

        private bool PrintRoomDuringLoop { get; set; }

        private string WinningRoomCodePrivate { get; set; }
        public string WinningRoomCode { get { return this.WinningRoomCodePrivate; } set { this.WinningRoomCodePrivate = value.ToUpper(); } }

        public List<Item> WinningItems { get; set; }
        public string GameState { get; set; }
        private string WhereKilledCode { get; set; }
        private string WhatKilledCode { get; set; }
        public string WinningMsg { get; set; }
        public PlayerClass Player { get; set; }
        private bool StartCombat { get; set; }
        public string ListOfFoesPrefix {  get; set; }
        public Regex AttackNPCRegex { get; set; }
        public string MissAttackString { get; set; }
        public string WhoKilledCode { get; set; }
        public string StartOfCombatString { get; set; }
        public string KIAstring {  get; set; }
        private List<Item> LootItems { get; set; }
        
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
                if(CheckForCombat(this.CurrentRoomCode, WhenToFight.leave))
                {
                    Combat(this.CurrentRoomCode, WhenToFight.leave);
                }

                if (this.GameState != "DEAD")
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
                    if (this.GameState != "DEAD")
                    { 
                        if (CheckForCombat(this.CurrentRoomCode, WhenToFight.enter))
                        {
                            Combat(this.CurrentRoomCode, WhenToFight.enter);
                        }
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
            Console.WriteLine();
            this.Player.printEquipment();
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
                else
                {
                    var equipment = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode)?.FloorEquipment.FirstOrDefault(x => x.Name == ItemCode);
                    if (equipment == null)
                    {
                        equipment = this.Player.Equipment.FirstOrDefault(x => x.Name == ItemCode);
                    }
                    if (equipment != null)
                    {
                        equipment.PrintEquipmentStats();
                        desc = "\t";
                    }
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
                    if (!foundItem)
                    {
                        var equipment = currentRoom.FloorEquipment.FirstOrDefault(x => x.Name == input);
                        if (equipment != null)
                        {
                            this.Player.Equipment.Add(equipment);
                            currentRoom.FloorEquipment.Remove(equipment);
                            foundItem = true;
                            Console.WriteLine($"{this.PickedUpItemMsg.Replace("{item}",equipment.Name)}");
                        }
                        
                    }

                    if (!foundItem)
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
                                if (CheckForCombat(npc))
                                {
                                    Combat(this.CurrentRoomCode, WhenToFight.ask);
                                }
                                if (this.GameState != "DEAD")
                                {
                                    Console.WriteLine(response.ToString());
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(MissNPCResponse);
                        }

                    }
                }
                else
                {
                    var response = string.Empty;
                    var attackingNPC = new NonPlayerCharacter();
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
                        if (CheckForCombat(attackingNPC))
                        {
                            Combat(this.CurrentRoomCode, WhenToFight.ask);
                        }
                        if (this.GameState != "DEAD")
                        {
                            Console.WriteLine(response);
                        }
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
        private void Equip(string input)
        {
            var targetEquipment = this.Player.Equipment.FirstOrDefault(x => x.IsEquiped == false && x.Name == input);
            if (targetEquipment != null)
            {
                var sb = new StringBuilder();
                var currentEquipment = this.Player.Equipment.FirstOrDefault(x => x.IsEquiped && x.Type == targetEquipment.Type);
                if (currentEquipment != null)
                {
                    if(currentEquipment.Type == EquipmentType.Weapon || currentEquipment.Type == EquipmentType.Shield)
                    {
                        sb.Append($"You put away your {currentEquipment.Name}. ");
                    }
                    else
                    {
                        sb.Append($"You take off your {currentEquipment.Name}.");
                    }
                        currentEquipment.IsEquiped = false;
                }
                targetEquipment.IsEquiped = true;
                if (targetEquipment.Type == EquipmentType.Weapon || targetEquipment.Type == EquipmentType.Shield)
                {
                    sb.AppendLine($"You are now Wielding your {targetEquipment.Name}");
                }
                else
                {
                    sb.AppendLine($"You are now wearing {targetEquipment.Name}");
                }
                Console.WriteLine(sb.ToString());
                if(this.addRemoveHealthOnEquipChange)
                {
                    if(currentEquipment != null && currentEquipment.HealthBoost != 0)
                    {
                        this.Player.CurrentHealth -= currentEquipment.HealthBoost;
                    }
                    if(targetEquipment.HealthBoost != 0)
                    {
                        this.Player.CurrentHealth += targetEquipment.HealthBoost;
                    }
                }
            }
            else
            {
                Console.WriteLine($"There is no {input} in your unequipped equipment.");
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
            else if (WearEquimentRegex.IsMatch(input))
            {
                strippedInput = Regex.Replace(input, WearEquimentRegex.ToString(), "").Trim();
                Equip(strippedInput);
            }
            else if (SelfRegex.IsMatch(input))
            {
                this.Player.printAll();
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
                    else if(string.IsNullOrEmpty(this.WhoKilledCode) == false)
                    {
                        var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);
                        var sb = new StringBuilder();
                        if(currentRoom != null)
                        {
                            sb.Append($"You lie in the {currentRoom.Name}. "); 
                        }
                        sb.Append($"You were fell by {this.WhoKilledCode}. Better luck next time.");
                        Console.WriteLine(sb.ToString());
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

        #region Combat
        private bool CheckForCombat(string roomCode, WhenToFight when)
        {
            bool veryYes = false;
            var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);
            if (currentRoom != null)
            {
                veryYes = currentRoom.NonPlayerCharacters.Any(npc => npc.WillFight && npc.WhenToFight == when);
            }
            return veryYes;
        }
        private bool CheckForCombat(NonPlayerCharacter npc,WhenToFight when = WhenToFight.ask)
        {
            return npc.WhenToFight == when;
        }

        private void Combat(string roomCode, WhenToFight when)
        {
            var currentRoom = this.Rooms.Where(r => r.RoomCode == roomCode.ToUpper()).FirstOrDefault();
            if (currentRoom != null)
            {
                var foeList = currentRoom.NonPlayerCharacters.Where(npc => npc.WillFight && npc.WhenToFight == when).ToList();
                //NEED TO REMOVE DEAD FOES FROM ROOM, THEY ARE ALL NPCS OBJECTS
                currentRoom.NonPlayerCharacters.RemoveAll(npc => npc.WillFight && npc.WhenToFight == when);
                Console.Clear();
                Console.WriteLine($"{this.StartOfCombatString}\n\n");
                while (foeList.Count > 0 && !(this.GameState == "DEAD"))
                {
                    PlayerPhase(foeList);
                    FoePhase(foeList);
                }

                if (this.GameState != "DEAD")
                {
                    PrintVictoryStats();
                    AddLootToPlayer();
                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
            }
        }
        private void PrintVictoryStats()
        {
            //will add more later see TODO.txt
            Console.WriteLine("YOU ARE VICTORIOUS!\n");
            PrintLoot();
        }

        private void AddLootToPlayer()
        {
            this.PlayInv.AddRange(LootItems);
            this.LootItems.Clear();
        }
        private void PrintLoot()
        {
            var sb = new StringBuilder();
            foreach (var item in LootItems)
            {
                if (sb.Length == 0)
                {
                    sb.Append($"{item.Code}");
                }
                else
                {
                    sb.Append($", {item.Code}");
                }
            }

            if (sb.Length > 0)
            {
                Console.WriteLine($"You have Received {sb.ToString()}\n");
            }
        }
        private void PlayerPhase(List<NonPlayerCharacter> foes)
        {
            var input = string.Empty;
            do
            {
                PrintFoeList(foes);
                input = GetInput();
                
            } while (ActCombatInput(input,foes));
        }
        private void FoePhase(List<NonPlayerCharacter> foes)
        {
            foreach( var foe in foes)
            {
                if(RollToHitPlayer(this.Player,foe))
                {
                    RollAndApplyDmg(this.Player, foe.MinDamageOutput, foe.MaxDamageOutput, foe.Name);
                    if(this.GameState == "DEAD")
                    {
                        this.WhoKilledCode = foe.Name;
                    }
                }
                else
                {
                    Console.WriteLine($"{foe.Name} MISSED their attack!\n\n");
                }
            }
        }
        private void PrintFoeList(List<NonPlayerCharacter> foes)
        {
            var sb = new StringBuilder();
            sb.Append($"{this.ListOfFoesPrefix}");
            foreach (var foe in foes)
            {
                if (sb.Length == this.ListOfFoesPrefix.Length)
                {
                    sb.Append(foe.Name);
                }
                else
                {
                    sb.Append(", " + foe.Name);
                }
            }
            sb.Append(".\n");
            Console.WriteLine(sb.ToString());
        }

        private bool ActCombatInput(string input,List<NonPlayerCharacter> foes)
        {
            var breakCombatInputLoop = false;
            var strippedInput = string.Empty;
            Console.WriteLine();
            if(AttackNPCRegex.IsMatch(input))
            {
                strippedInput = Regex.Replace(input, AttackNPCRegex.ToString(), "").Trim();
                breakCombatInputLoop = AttackNPC(strippedInput,foes);
            }

            return !breakCombatInputLoop;
        }

        private bool AttackNPC(string input, List<NonPlayerCharacter> foes)
        {
            var breakCombatInputLoop = false;
            var targetFoe = foes.FirstOrDefault(x => x.Name == input);
            if (targetFoe != null)
            {
                breakCombatInputLoop = true;
                if(RollToHitNPC(this.Player, targetFoe))
                {
                    var removeNPC = RollAndApplyDmg(targetFoe, this.Player.TotalMinDamagePublic, this.Player.TotalMaxDamagePublic);
                    if(removeNPC)
                    {
                        Console.WriteLine($"You have Defeated {targetFoe.Name}");
                        foes.Remove(targetFoe);
                        LootItems.AddRange(targetFoe.Inventory);
                    }
                }
                else
                {
                    Console.WriteLine($"{this.MissAttackString}");
                }

            }
            else
            {
                Console.WriteLine($"There is no target: {input}");
            }
            return breakCombatInputLoop;
        }

        private bool RollToHitNPC(PlayerClass player,NonPlayerCharacter targetFoe)
        {
            var dice = new Random();
            //player hit chance - foe dodge chance
            //90 - 10 = 80
            //roll d100 get less than or equal 80
            var toHit = player.TotalHitChancePublic - targetFoe.DodgeChance;
            var roll = dice.Next(100) + 1;

            return roll <= toHit;
        }
        private bool RollToHitPlayer(PlayerClass player, NonPlayerCharacter targetFoe)
        {
            var dice = new Random();
            //player hit chance - foe dodge chance
            //90 - 10 = 80
            //roll d100 get less than or equal 80
            var toHit = targetFoe.HitChance - player.TotalDodgeChancePublic;
            var roll = dice.Next(100) + 1;

            return roll <= toHit;
        }
        private bool RollAndApplyDmg(NonPlayerCharacter npc,int min,int max)
        {
            var dice = new Random();
            //roll between min max
            var dmg = dice.Next(min, max+1);//it does not include the upper limit
            //apply DR
            dmg -= npc.DamageReduction;
            //find floor on dmg
            if(dmg < 0)
            {
                if(npc.TakeAtLeastOneDamage)
                {
                    dmg = 1;
                }
                else
                {
                    dmg = 0;
                }
            }
            //apply dmg
            npc.CurrentHealth -= dmg;

            //output dmg
            Console.WriteLine($"You hit {npc.Name} for {dmg} damage!");

            //return if foe is dead
            return npc.CurrentHealth <= 0;
        }
        private void RollAndApplyDmg(PlayerClass player,int min,int max, string foeName)
        {
            var dice = new Random();
            //roll between min max
            var dmg = dice.Next(min, max + 1);//it does not include the upper limit
            //apply DR
            dmg -= player.TotalDamageReductionPublic;
            //find floor on dmg
            if (dmg < 0)
            {
                if (player.TakeAtLeastOneDamage)
                {
                    dmg = 1;
                }
                else
                {
                    dmg = 0;
                }
            }

            //apply dmg
            player.CurrentHealth -= dmg;
            //output dmg
            var HitDmgString = new StringBuilder();
            HitDmgString.Append($"{foeName} hit you for {dmg} damage! ");
            if(player.CurrentHealth <= 0)
            {
                HitDmgString.Append($"{this.KIAstring}\n\n");
                HitDmgString.AppendLine("Press any key to continue");
            }
            else
            {
                HitDmgString.Append($"You have {player.CurrentHealth} Hitpoint(s) remaining!\n");
            }
            Console.WriteLine(HitDmgString.ToString());
            //check if we need to change the gamestate to dead
            if (player.CurrentHealth <= 0)
            {
                this.GameState = "DEAD";
            }
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

            } while (BreakGamePlayLoop(input));

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
