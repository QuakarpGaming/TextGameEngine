using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextGameEngine.Env;

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
            this.InputPrefix = "\nWhat Will You Do: ";
            this.MoveRegex = new Regex("^GO$|^MOVE$|^ENTER$");
            this.LookAtItemRegex = new Regex("^LOOK$|^BEHOLD$|^EXAMINE$|^SCAN$|^VIEW$|^SPOT$");
            this.PickUpItemRegex = new Regex("^GET$|^TAKE$|^PICKUP$|^LOOT$|^STEAL$");
            this.DroppedItemRegex = new Regex("^DROP$|^PLACE$|^REMOVE$|^THROW$|^ABANDON$");
            this.QuitRegex = new Regex("^QUIT$|^DESKTOP$|^ALT-F4$");
            this.PickedUpItemMsg = "You picked up the {item}.";
            this.DroppedItemMsg = "You drop the {item}.";
            this.MissPickUpMsg = "There Is no {item} in this room.";
            this.missDropMsg = "You do not have {item} to drop.";
            this.PrintRoomDuringLoop = true;
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
        public string DroppedItemMsg {  get; set; }
        public string MissPickUpMsg {  get; set; }
        public string missDropMsg { get; set; }

        public Regex MoveRegex { get; set; }
        public Regex LookAtItemRegex { get; set; }

        public Regex PickUpItemRegex { get; set; }
        public Regex DroppedItemRegex { get; set; }
        public Regex QuitRegex { get; set; }
        private bool PrintRoomDuringLoop { get; set; }
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
        public void PrintPlayerInv()
        {
            Console.WriteLine("You currently Have: \n");
            foreach (var item in this.PlayInv)
                Console.WriteLine(item.Code);
        }

        public void PrintItemDesc(string ItemCode)
        {
            var item = this.PlayInv.FirstOrDefault(x => x.Code == ItemCode.ToUpper());
            if(item != null)
            {
                Console.WriteLine(item.Description);
            }
            else
            {
                Console.WriteLine($"You do not have {ItemCode.ToUpper()}.");
            }
        }
        public void PickUpItem(string code)
        {
            code = code.ToUpper() ?? string.Empty;
            var currentRoom = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode);
            if (currentRoom != null)
            {
                var item = currentRoom.FloorItems.FirstOrDefault(x => x.Code == code.ToUpper());
                if (item != null)
                {
                    PlayInv.Add(item);
                    currentRoom.FloorItems.Remove(item);
                    Console.WriteLine(PickedUpItemMsg.Replace("{item}",item.Code));
                }
                else
                {
                    Console.WriteLine(MissPickUpMsg.Replace("{item}", code));
                }
            }
        }
        public void DropItem(string code)
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
                {
                    Console.WriteLine(missDropMsg.Replace("{item}", code));
                }
            }
        
        }
        #endregion

        #region Input
        public string[] GetInput()
        {
            var isValid = false;
            string[] input = new string[2];
            do
            {
                Console.Write(this.InputPrefix);
                input = (Console.ReadLine() ?? string.Empty).ToUpper().Split(' ');
                if (input.Length >= 1)
                    isValid = true;
                    
            } while (!isValid);
            return input;
        }

        public void ActInput(string[] input)
        {
            if (input.Length >= 2)
            {
                if (MoveRegex.IsMatch(input[0]))
                {
                    
                    PrintRoomDuringLoop = MoveRoom(input[1]);
                }
                else if (LookAtItemRegex.IsMatch(input[0]))
                {
                    var item = this.Rooms.FirstOrDefault(x => x.RoomCode == this.CurrentRoomCode)?.FloorItems.FirstOrDefault(x => x.Code == input[1]);
                    if (item != null)
                    {
                        Console.WriteLine(item.Description);
                    }
                    else
                    {
                        item = this.PlayInv.FirstOrDefault(x => x.Code == input[1]);
                        if (item != null)
                        {
                            Console.WriteLine(item.Description);
                        }
                    }
                }
                else if (PickUpItemRegex.IsMatch(input[0]))
                {
                    PickUpItem(input[1]);
                }
                else if (DroppedItemRegex.IsMatch(input[0]))
                {
                    DropItem(input[1]);
                }
            }
            else if(input.Length == 1)
            {
                if (LookAtItemRegex.IsMatch(input[0]))
                {
                    PrintCurrentRoom();
                }
            }
        }
        #endregion
        #endregion

        #region Game Play Loop
        public void GamePlayLoop()
        {
            string[] input = new string[1];
            
            do
            {
                if (this.PrintRoomDuringLoop)
                {
                    PrintCurrentRoom();
                    this.PrintRoomDuringLoop = false;
                }
                input = GetInput();
                ActInput(input);
            }while((input.Length == 0) || (input.Length != 0 && this.QuitRegex.IsMatch(input[0]) == false));
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
