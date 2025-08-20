using TextGameEngine.Env;
using TextGameEngine.Game;
using TextGameEngine.Player;
var game = new TheGame();

//game.WinningRoomCode = "ICR1";
//game.WinningMsg = "You Entered the cave with a torch, praise be unto you, for Completing this very hard task!";

game.Rooms.Add(new Room()
{
    Name = "Outside the Cave",
    RoomCode = "OUTSIDE THE CAVE",
    Description = "You have finally arrived at the cave where there is a Hag that has your sister. It is just an unassuming hole in the wall, that is too dark to see into.",
    Exits = new List<Exit> { new Exit()
    {
        IsLocked = false,
        KeyItemCode = "TORCH",
        ToRoomCode = "ICR1",
        ToRoomDiscription = "CAVE",
        LockMsg = "I need to pick up the torch to procced."
    }},
    FloorItems = new List<Item> { new Item()
    {
        Code = "TORCH",
        Description = "A long stick with a rag soak in oil. Great for cave exploration!"
    }},
    NonPlayerCharacters = new List<NonPlayerCharacter>
                                    {
                                        new NonPlayerCharacter()
                                        { Name = "John Smith",
                                          Description = "Smells like a hag.",
                                          Inventory = new List<Item>()
                                                        {
                                                        new Item() {
                                                            Code = "GUN",
                                                            Description = "L shaped pew pew." ,
                                                            CanKill = true,
                                                            KillMsg="Why did you try and get a gun from someones waist? ARE YOU A FOOL. MR T pities you."
                                                            }
                                                        },
                                          Responses = new Dictionary<string, string>()
                                            {
                                              {"GUN","That is for me to know and you to die over." }
                                          }
                                    },
    }
    
});

game.Rooms.Add(new Room()
{
    Name = "Cave Enterence",
    RoomCode = "ICR1",
    Description = "You Enter the cave enterence. the stone almost looks natural, but you can tell it has been carved by magic.\n\nThere is an uneasy feeling in your gut as you look around. You can't help the feeling you are being watched somehow.",
    Exits = new List<Exit> {
        new Exit()
        {
            IsLocked = false,
            ToRoomCode = "OUTSIDE THE CAVE",
            ToRoomDiscription = "OUTSIDE THE CAVE",
        },
        new Exit()
        {
            IsLocked = false,
            ToRoomCode = "SCR",
            ToRoomDiscription = "NORTH",
        }
    },
    FloorItems = new List<Item> {
        new Item()
        {
            Code = "ROCK",
            Description = "This looks like it could do some damage.",
            CanKill = true,
            PreventKill = new List<string> {"NOTHING THIS IS FOR TESTING"},
            KillMsg = "As you pick up the rock you hear a loud CLICK! After that everything went dark."
            
        }
    },
    canKill = true,
    preventKill = new List<string>() { "TORCH" },
    KillMsg = "Well, you went into a cave with no light source. after about 5 seconds in darkness you stub your toe, then face plant in a wall. You are bleeding out from your nose and dead. Maybe next time grab the torch first."
});

game.Rooms.Add(new Room()
{
    Name = "The Fork in the Road",
    RoomCode = "SCR",
    Description = "You Enter a room. there is a fork in the room. There are spike coming out of both the floor and ceiling. Upon further examinated you can tell these were made by crual magic. Best be fast in saving your sister.",
    Exits = new List<Exit> {
        new Exit()
        {
            IsLocked = false,
            ToRoomCode = "ICR1",
            ToRoomDiscription = "SOUTH",
        },
        new Exit()
        {
            IsLocked = false,
            ToRoomCode = "WC",
            ToRoomDiscription = "WEST",
        },
        new Exit()
        {
            IsLocked = true,
            KeyItemCode = "RING",
            ToRoomCode = "EC",
            ToRoomDiscription = "EAST",
        }
    },
});

game.CurrentRoomCode = "OUTSIDE THE CAVE";

//game.GamePlayLoop();



var player = new PlayerClass();
player.Name = "Ruby Hates Jack, teehee :)";

player.Equipment = new List<Equipment>
{
    new Equipment(name:"Sword Of Stabbing +1",type: EquipmentType.Weapon,minDamageBoost:1,maxDamageBoost:1,isEquiped:true ),
    new Equipment(name:"Sword Of Not Stabbing -2",type:EquipmentType.Weapon,minDamageBoost:-2,maxDamageBoost:-2,isEquiped:false),
    new Equipment(name:"Sword Of Not Stabbing -1",type:EquipmentType.Weapon,minDamageBoost:-1,maxDamageBoost:-1,isEquiped:false),
    new Equipment(name:"Helm Of Courage",type:EquipmentType.Head,isEquiped:true, healthBoost:10,hitBoost: -5,dodgeBoost:-5,damageReductionBoost:3)

};
player.printAll();

Console.ReadLine();