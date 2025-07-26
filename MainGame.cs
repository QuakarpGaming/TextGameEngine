using TextGameEngine.Env;
using TextGameEngine.Game;

var game = new TheGame();

game.Rooms.Add(new Room()
{
    Name = "Outside the Cave",
    RoomCode = "OTC",
    Description = "You have finally arrived at the cave where there is a Hag that has your sister. It is just an unassuming hole in the wall, that is too dark to see into.",
    Exits = new List<Exit> { new Exit()
    {
        IsLocked = true,
        KeyItemCode = "TORCH",
        ToRoomCode = "ICR1",
        ToRoomDiscription = "CAVE",
        LockMsg = "I need to pick up the torch to procced."
    }},
    FloorItems = new List<Item> { new Item()
    {
        Code = "TORCH",
        Description = "A long stick with a rag soak in oil. Great for cave exploration!"
    }}
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
            ToRoomCode = "OTC",
            ToRoomDiscription = "OUTSIDE",
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
            Description = "This looks like it could do some damage."
        }
    }
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

game.CurrentRoomCode = "OTC";

game.GamePlayLoop();

Console.ReadLine();