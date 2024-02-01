
using System;
using System.Net.Security;
using System.Runtime.Serialization;

Console.WriteLine("Time for the RPG Battler");
Player player = createPlayer();
Console.Write("Press Any Key...");
Console.ReadKey();
Console.Clear();

int levelMax = 100;
int level = 0;

Arena(level, levelMax, player);


































static void Arena(int level, int levelMax, Player player)
{
    player.dead = false;
    for (int i = 0; level <= levelMax; level++)
    {
        Enemy enemy = createEnemy(level);

        Console.WriteLine($"It's {player.name} VERSUS {enemy.name}");
        Console.WriteLine();

        player.dead = Battle(enemy, player);
        Console.Write("Press Any Key...");
        Console.ReadKey();
        Console.Clear();

        if (player.dead)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over!");
            Console.ResetColor();
            break;
        }
    }
}










static bool Battle(Enemy enemy, Player player)
{
    while (player.health > 0 && enemy.health > 0)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        playerTurn(player, enemy);
        Console.ResetColor();

        Console.WriteLine();
        if (enemy.dead)
        {
            return false;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Attack(enemy, player);
        Console.ResetColor();

        Console.ReadKey();
        Console.WriteLine();
        if (player.dead)
        {
            return true;
        }
    }
    return false;
}










static void playerTurn(Player player, Enemy enemy)
{
    Console.WriteLine("It's your turn!");
    Console.WriteLine("______\t\t\t_____\t\t\t____");
    Console.WriteLine("Attack\t\t\tBlock\t\t\tItem");
    Console.WriteLine("------\t\t\t-----\t\t\t----");

    int playerMove = playerChoice();

    if (playerMove == 1)
    {
        Console.WriteLine();
        Attack(player, enemy);
        player.block = false;
    }
    else if (playerMove == 2)
    {
        player.block = true;
    }
    else
    {
        playerItemMenu(player);
        player.block = false;
    }
}










static int playerChoice()
{
    string choice = Console.ReadLine().ToLower().Trim();

    while (choice != "attack" && choice != "block" && choice != "item")
    {
        choice = Console.ReadLine().ToLower().Trim();
    }

    if(choice == "attack")
    {
        return 1;
    }
    else if (choice == "block")
    {
        return 2;
    }
    else
    {
        return 3;
    }
}










static void playerItemMenu(Player player)
{
    Console.WriteLine("Choose an item");
    Console.WriteLine("==============");
    Console.WriteLine();
    for (int i = 0; i < player.items.Count; i++)
    {
        Console.WriteLine($"{i + 1}- {player.items[i].name}");
    }
    int choice = -2;
    while(!int.TryParse( Console.ReadLine(), out choice) &&  (choice < 0 || choice > player.items.Count))
    {
        Console.WriteLine("Choose an item from the list...");
    }
    choice--;

    player.items[choice].Equip(player);
    Console.WriteLine($"{player.name} just used a {player.items[choice].name}!");
    if (player.items[choice].maxHealth > 0)
    {
        Console.WriteLine($"{player.name}'s Max Health increased by {player.items[choice].maxHealth}");
        Console.WriteLine($"{player.name} is now at {player.health} health!");
    }
    if (player.items[choice].health > 0)
    {
        Console.WriteLine($"{player.name} healed by {player.items[choice].health}");
        Console.WriteLine($"{player.name} is now at {player.health} health!");
    }
    if (player.items[choice].damage > 0)
    {
        Console.WriteLine($"{player.name}'s damage increased by {player.items[choice].damage}");
        Console.WriteLine($"{player.name} now deals {player.damage} damage!");
    }
    if (player.items[choice].critChance > 0)
    {
        Console.WriteLine($"{player.name}'s Crit Chance increased by {player.items[choice].critChance * 100}%");
        Console.WriteLine($"{player.name} now has a {player.critChance * 100}% chance to crit!");
    }

    player.items.Remove(player.items[choice]);
}










static void Attack(Character attacker, Character defender)
{
    Random ran = new Random();
    int damage = attacker.damage;
    if (defender.block)
    {
        damage = damage / 2;
        blockText(defender);
    } 
    double critVal = ran.NextDouble();
    if (attacker.critChance > critVal)
    {
        Console.WriteLine($"{attacker.name} attacks and it's a CRIT! He did {damage * 2} damage!");
        defender.health -= damage * 2;
    }
    else
    {
        Console.WriteLine($"{attacker.name} attacks! He did {damage} damage!");
        defender.health -= damage;
    }


    if (defender.health > 0)
    {
        Console.WriteLine($"{defender.name} has {defender.health} health left!");
    }

    else if (defender.health <= 0)
    {
        Console.WriteLine($"{defender.name} HAS DIED!");
        defender.dead = true;
    }
}










static void blockText(Character player)
{
    Console.WriteLine($"{player.name} blocked the attack!");
}










static Player createPlayer()
{
    Player player = new Player();
    player.maxHealth = 15;
    player.health = 15;
    player.damage = 3;
    player.critChance = 0.2;
    player.gold = 0;
    player.items = new List<Item>();

    player.items.Add(sHealthPotion());
    player.items.Add(sHealthPotion());
    player.items.Add(sHealthPotion());
    player.items.Add(megaCritPotion());
    player.items.Add(megaDamagePotion());
    player.items.Add(megaVitalityPotion());


    Console.Write("What is your name young warrior?: ");
    player.name = Console.ReadLine();

    return player;
}










static Enemy createEnemy(int level)
{
    int baseHealth = 10;
    int baseDamage = 2;
    double baseCrit = 0;
    int baseDrop = 5;


    int health = (int)Math.Round((baseHealth + (-Math.Pow(((level - 100) / 5.34522483824848), 2) + 350)));
    int damage = (int)Math.Round((baseDamage + (-Math.Pow(((level - 100) / 14.14213562373095), 2) + 50)));
    double crit = baseCrit + (level * 1.5 / 100);
    if (crit > 0.35)
    {
        crit = 0.35;
    }
    int dropAmm = baseDrop + (level * 2);

        

    Enemy enemy = new Enemy();
    enemy.health = health;
    enemy.damage = damage;
    enemy.critChance = crit;
    enemy.name = createEnemyName(level);
    enemy.dropAmmount = dropAmm;


    return enemy;
}










static string createEnemyName(int level)
{
    Random ran = new Random();
    string[] enemyT1 = { "Goblin", "Giant Rat", "Bee Hive", "Giant Centipede", "Boa Constrictor", "Street Urchin", "Rabid Dog" };
    string[] enemyT2 = { "Bandit", "Gnarled Bear", "Aged Mage", "Ruffian", "Giant Alligator", "Thirsty Vampire Spider", "Porgon" };
    string[] enemyT3 = { "Strange Penguin", "Queen Bee", "Troll", "Great Knight", "Terrestrial Whale" };
    string[] bosses = { "Gilded Knight", "Lone Mercenary", "Frenzied Beast", "Roland the First" };

    string enemyName = "";

    if (level < 10)
    {
        enemyName = enemyT1[ran.Next(enemyT1.Length)];
        return enemyName;
    }
    else if (level < 30)
    {
        if (ran.Next(0, 2) == 0)
        {
            enemyName = "Three " + enemyT1[ran.Next(enemyT1.Length)] + "s";
            return enemyName;
        }
        else
        {
            enemyName = enemyT2[ran.Next(enemyT2.Length)];
            return enemyName;
        }
    }
    else if (level < 60)
    {
        int enemyTier = ran.Next(0, 3);
        if (enemyTier == 0)
        {
            enemyName = "Five " + enemyT1[ran.Next(enemyT1.Length)] + "s";
            return enemyName;
        }
        else if ((enemyTier == 1))
        {
            enemyName = "Two " + enemyT2[ran.Next(enemyT2.Length)] + "s";
            return enemyName;
        }
        else
        {
            enemyName = enemyT3[ran.Next(enemyT3.Length)];
            return enemyName;
        }
    }
    else
    {
        int enemyTier = ran.Next(0, 3);
        if (enemyTier == 0)
        {
            enemyName = "Seven " + enemyT1[ran.Next(enemyT1.Length)] + "s";
            return enemyName;
        }
        else if ((enemyTier == 1))
        {
            enemyName = "Four " + enemyT2[ran.Next(enemyT2.Length)] + "s";
            return enemyName;
        }
        else
        {
            enemyName = enemyT3[ran.Next(enemyT3.Length)] + " and Two " + enemyT2[ran.Next(enemyT2.Length)] + "s";
            return enemyName;
        }
    }
}









static Item sHealthPotion()
{
    Item shealthPotion = new Item();
    shealthPotion.name = "Small Health Potion";
    shealthPotion.health = 5;

    return shealthPotion;
}

static Item mHealthPotion()
{
    Item mhealthPotion = new Item();
    mhealthPotion.name = "Medium Health Potion";
    mhealthPotion.health = 15;

    return mhealthPotion;
}

static Item lHealthPotion()
{
    Item lhealthPotion = new Item();
    lhealthPotion.name = "Large Health Potion";
    lhealthPotion.health = 30;

    return lhealthPotion;
}

static Item megaCritPotion()
{
    Item mgCritPotion = new Item();
    mgCritPotion.name = "Mega Crit Potion";
    mgCritPotion.critChance = 0.5;

    return mgCritPotion;
}

static Item megaDamagePotion()
{
    Item mgDamPotion = new Item();
    mgDamPotion.name = "Mega Damage Potion Potion";
    mgDamPotion.damage = 50;

    return mgDamPotion;
}

static Item megaVitalityPotion()
{
    Item mgVitPotion = new Item();
    mgVitPotion.name = "Mega Vitality Potion";
    mgVitPotion.maxHealth = 100;

    return mgVitPotion;
}






public class Character
{
    public int maxHealth;
    public int health;
    public int damage;
    public double critChance;
    public string name;
    public bool dead;
    public bool block;
}





public class Player : Character
{
    public int gold;
    public Item? equiped;
    public List<Item>? items;
}





public class Enemy : Character
{
    public int dropAmmount;
}





public class Item
{
    public int maxHealth;
    public int health;
    public int damage;
    public double critChance;
    public string name;

    public void Equip(Character character)
    {
        character.maxHealth += maxHealth;
        character.health += maxHealth;
        character.health += health;
        if (character.health > maxHealth) 
        {
            character.health = maxHealth;        
        }
        character.damage += damage;
        character.critChance += critChance;
    }

    public void Remove(Character character)
    {
        character.maxHealth -= maxHealth;
        character.health -= maxHealth;
        character.health -= health;
        if(character.health <= 0)
        {
            character.health = 1;
        }
        character.damage -= damage;
        character.critChance -= critChance;
    }
}