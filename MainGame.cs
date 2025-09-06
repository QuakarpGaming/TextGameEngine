using TextGameEngine.Env;
using TextGameEngine.Game;
using TextGameEngine.Player;
var game = new TheGame();
game.Rooms.Add(new Room(code: "Room1",
                        name: "Bat Combat Room",
                        desc: "Bat Country!",
                        exits: new List<Exit> { new Exit(toRoomCode: "Start", toRoomDiscription: "Outside") },
                        NPCs: new List<NonPlayerCharacter> {
                            new NonPlayerCharacter(name: "The Bat",
                                                   description: "A Bat",
                                                   willFight: true,
                                                   whenToFight: WhenToFight.enter,
                                                   inventory: new List<Item> { new Item(code: "Bat Testicals"
                                                                                        , description: "They are bat balls, what else do you want pervert!")},
                                                   droppedOnDeathEquipment: new List<Equipment> { new Equipment(name: "Bat Balls",
                                                                                                                type: EquipmentType.Weapon,
                                                                                                                minDamageBoost:0,
                                                                                                                maxDamageBoost: 10,
                                                                                                                dodgeBoost:4)},
                                                   droppedGold: 9999
                                                   )},
                        shop: new Shop(name: "The Bat Shoppe",
                                        items: new List<Item> { new Item(code: "BAT BATS", description: "Bats for bats", gold: 10), new Item(code: "Can't buy this", description: "Cost way too much", gold: 10000000) },
                                        equipment: new List<Equipment> { new Equipment(name: "BatMan"), new Equipment(name:"Manbat", gold:1000000) }
                                        )

                        ));

game.Rooms.Add(new Room(code: "Start",
                        name: "Outside Of Cave",
                        desc: "You are outside of a cave. looks like its time to fight some a bat",
                        exits: new List<Exit>() { new Exit(toRoomCode: "Room1",toRoomDiscription:"Cave" ) }
                        ,floorItems: new List<Item> { new Item(code:"Red herring",description:"Definatly the correct clue.")},
                        floorEquipment: new List<Equipment> { new Equipment(name:"Hammer of Whacking",type:EquipmentType.Weapon,minDamageBoost:1,maxDamageBoost:1,hitBoost:12,dodgeBoost:1,damageReductionBoost:1,healthBoost:1) }
                        
                        
                        
                        )
    );

game.Player.Equipment.Add(new Equipment(name: "Leather Helm", isEquiped: true, healthBoost: 2));
game.Player.Equipment.Add(new Equipment(name: "Leather Armor",type:EquipmentType.Body, isEquiped: true, healthBoost: 2,damageReductionBoost:1));
game.Player.Equipment.Add(new Equipment(name: "Leather Gloves", type: EquipmentType.Arms, isEquiped: true, healthBoost: 2, damageReductionBoost: 1, hitBoost:5));
game.Player.Equipment.Add(new Equipment(name: "Leather Boots", type: EquipmentType.Legs, isEquiped: true, dodgeBoost: 5));
game.Player.Equipment.Add(new Equipment(name: "Bronze Sword", type: EquipmentType.Weapon, isEquiped: true, minDamageBoost:4500,maxDamageBoost:5000));
game.Player.Equipment.Add(new Equipment(name: "Iron Sword", type: EquipmentType.Weapon, isEquiped: false, minDamageBoost: 1, maxDamageBoost: 3,healthBoost:155));


game.Player.HealFull();
game.Player.CurrentHealth -= 4;
//game.Player.printAll();
//Console.ReadLine();
//game.CurrentRoom = game.Rooms[0];
game.GamePlayLoop();







Console.ReadLine();