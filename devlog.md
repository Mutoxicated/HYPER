# Devlog

**This is a log of my entire development process of HYPER.**

## Day 1

Day 1 is as simple and dry as it can get. Creating the repository, project and just analyzing the concept.

## Day 2

Made the groundworks of the player today: movement, camera look, all that.
Movement is almost done.
The movement is very similar to ultrakill, but that's not intentional and is only because ultrakill just generally has good basic movement mechanics.
As I have stated in the description of the game, you will be able to upgrade your weapon with many different items.
Not only that, but certain items are categorized in different **classes**.
Classes can be synergized to create **synergized classes**.
Haven't thought a lot about the classes, so I'll think about them more on the following days.

## Day 3

Completed the movement. 
You can slide, jump, wall jump, ground slam, dash and, of course, walk.
Haven't implemented a limit to the wall jumps or a cooldown for the dash but I will do it eventually.
I took some time to think about the gameplay and here's what i'm thinking:
Of course, there will be the Arena, where you fight enemies in waves, but once the round ends, you go to the **Shop**.
The Shop is where you buy your upgrades. The upgrades are categorized in certain classes.
There are starter classes, synergized classes and HYPER-classes. Starter classes are what you start with, synergized classes can only be crafted with two starter classes.

### **STARTER CLASSES:**

1. INSANER (dps)
2. WARRIOR (defense)
3. MAGICIAN (magic)
4. SUMMONER (summoning stuff)

### **SYNERGIZED CLASSES:**

1. JUGGERNAUT = INSANER + WARRIOR 
2. WIZARD = INSANER + MAGICIAN 
3. ELEMENTOR = WARRIOR + MAGICIAN 
4. ALPHA TRAINER = INSANER + SUMMONER
5. SIGMA TRAINER = WARRIOR + SUMMONER
6. BETA TRAINER = MAGICIAN + SUMMONER

### **HYPER-CLASSES:**

idk :p

That's it for today.

## Day 4

So today I did a little bit of both analyzing gameplay and working on the game mechanics.
I made a quick weapon model in blender and put it together in Unity.
I also implemented some sort of inertia to the gun, so that whenever you move, it would sort of react to your movements by being slightly dragged.
And also shooting is semi-done, I only implemented the recoil and stuff, but yeah.
Very cool :)

Besides that, I also implemented some more important mechanics, those being the upgrades and the classes.
Though it's a VERY early prototype, so of course it's likely to change many many times.
That being said, here's an update on the classes:

So as you may know, upgrades are categorized in different classes (thought some upgrades can be categorized into two classes as well). Once you’ve had max items in a certain class, you can choose to synergize the class with another one (that you've also had max items), creating a synergized class. 

Each class has its own upgrade pool, but many upgrades can overlap with each other. Once you’ve upgraded to the synergized class, the game will randomly choose X/2 upgrades from the class’ upgrade pool to give you, where X is the class’ max amount of upgrades. Getting upgrades that overlap with another class that you don’t have requires you to sacrifice a starter class (if you have one, because they start getting pretty rare later, because of the item pool getting bigger).

With Starter classes, the max items there are on all of them is 4.
With Synergized classes, the max is 8.
And with Hyper classes, the max is 16.

I'm not gonna tell you the HYPER classes right now, but once they are done I will show.
Forgot to mention but HYPER classes can be crafted with synergized classes and any starter class.

But yeah that's it for today I'm kinda tired. 

## Day 5

Couldn't do anything to the game today for a few private reasons, but I did think about an important part of the whole structure of the game: the **Upgrade System**.
Here's how the Weapon System and the Upgrade System co-operate together:

The Weapon System contains Weapon stats and Weapon info. Weapon stats include stuff like bullet damage, fire rate, etc.
Meanwhile, Weapon Info includes members and methods like OnShoot(), current bullet pool, etc.
The Upgrade System has two types of Upgrades: Modifiers and Addifiers.
Modifiers modify the Weapon Stats, while Addifiers use stuff from Weapon Info to add stuff.
There's also the Upgrades class, which creates the Upgrades upon start-up and collects them in an array.
I'm still not too sure about this though, so expect things to change a lot.
I'm already thinking of making them scriptable objects... idk, anyway yeah that's it for today.

## Day 6

Added a new ability to the movement: lock.
You can lock yourself midair for a brief moment. Very cool imo.
I made a lot of changes in the upgrade system, realized scriptable objects don't really work well with what i'm trying to do, so instead i will just make each upgrade a prefab.
But yeah all around I just did some small refactoring and semi-implemented the upgrade system. It's really tough though because I have to make it versatile enough that it would satisfy my creativity.


A good example to what I am trying to do here is the Unity's Particle System. It has modules, that do different stuff and you can enable and disable them.
Unfortunately I don't think I can do the same thing the Particle System did, so I'm going for a different solutions.
I came up with different behaviours that upgrades will have in their prefabs.
There are Player, Bullet and Weapon behaviours. 

Anyway's I'm tired as hell so i'm just gonna buh bye.

## Day 7

Not a lot, just improving movement, getting rid of the static WeaponInfo class, because i just didnt have a great feeling about it, also created actual prefabs for the gun parts.
Also, i made the gun screw rotate 90 degrees when you shoot, looks very cool imo.
Again, didn't do anything crazy yet because I'm planning on thinking out the upgrades and shit, so yeah. There will be like 28 upgrades absolute minimum, so there's a lot to come up with.

## Day 8

Slow but steady progress.
That's all I gotta say.
Been focusing on some other projects lately.
Still devoted to making changes every day even if it's gonna be small.

Anyway i came up with the last few HYPER classes and some upgrades so yeah.
**Here's the complete list of classes:**

### STARTER CLASSES:

1. INSANER (dps)
2. WARRIOR (defense)
3. MAGICIAN (magic)
4. SUMMONER (summoning stuff)

### SYNERGIZED CLASSES:

1. PENETRATOR = INSANER + WARRIOR 
2. WIZARD = INSANER + MAGICIAN 
3. ELEMENTOR = WARRIOR + MAGICIAN 
4. ALPHA TRAINER = INSANER + SUMMONER
5. SIGMA TRAINER = WARRIOR + SUMMONER
6. BETA TRAINER = MAGICIAN + SUMMONER

### HYPER-CLASSES:

(16 items max on all)
SC means Starter Class

1. ZEUS = SC + PENETRATOR + WIZARD
2. DIONE = SC + PENETRATOR + ELEMENTOR
3. ARTEMIS = SC + PENETRATOR + ALPHA TRAINER
4. AVATAR = SC + PENETRATOR + SIGMA TRAINER
5. HEKATE = SC + PENETRATOR + BETA TRAINER
6. LIZZARD = SC + WIZARD + ELEMENTOR
7. XENIA = SC + WIZARD + ALPHA TRAINER
8. GRINDER = SC + WIZARD + SIGMA TRAINER
9. MOONER = SC + WIZARD + BETA TRAINER
10. MENTOR = SC + ELEMENTOR + ALPHA TRAINER
11. BALLER = SC + ELEMENTOR + SIGMA TRAINER
12. HEPHAESTUS = SC + ELEMENTOR + BETA TRAINER
13. KAPPA TRAINER = SC + ALPHA TRAINER + SIGMA 14. TRAINER
15. INTER TRAINER = SC + ALPHA TRAINER + BETA TRAINER
16. LAMBDA TRAINER = SC + BETA TRAINER + SIGMA TRAINER

### A LITTLE BIT ABOUT ITEMS...

There are probably gonna be MINIMUM 28 upgrades and MAXIMUM 56.
The upgrades' class type(s) will overlap with each other to add more complexity and replayability.
Upgrades can modify or add to the player, the gun, or the bullets the gun shoots.
That's it lol.

## Day 9 

I did a bunch of stuff today! I made the base for the arena, i messed a bit with the graphisc (which resulted in some unfortunate circumstances) and i also fixed some more stuff with the movement.
Pretty happy with everything right now :)

Tomorrow I will try to make the arena better and also try to make the first few enemies. I have no idea how I will do it, these enemies will probably require AI and stuff...

I have an idea about how the player will transition from the arena to the shop. Initially, I had the idea of just making a vault door that would open and close if the round ended or you went inside the arena.
But now I think I'll just make like a tower thing in the middle of the arena that will have a minecraft beam shoot up at the sky, and once you complete the round, the beam will become white ig and will teleport you to the shop once you touch it. In the shop there will be a similar beam that you can also touch to got to the arena for the next round.

Also i made a custom skybox, which i will have to make more high quality cuz it sucks rn. Anyway yeah things are going smoothly right now :)

Here's a preview of the arena btw:
![image](https://github.com/Mutoxicated/HYPER/assets/96009711/e7b7db7d-87fc-4d5e-8a04-163d987a8e67)

## Day 10

Not a lot today, was focusing on other projects. I thought this was gonna be the day that I can relax but NOPE. 

Felt a little bit bad that I didn't do anything today so I actually tried to do a few stuff. So I wanted to kind of finish the movement once and for all, so i implemented the dash cooldown and the jump limit.
I also kinda played a bit with the particle system to do some effects for when the bullets collide, so yeah. That's pretty much everything I did today. 

Tomorrow I'll actually get to making the first enemy, and also start decorating the Arena with stuff, should be fun.

## Day 11

Didn't even open Unity today :/ Lately my sleep schedule hasn't been the best, but I will try to improve it in the following days.

Sorry.

## Day 13

Didn't have time yesterday to commit, but hey im back.
I made a quite some stuff today that were kind of a pain to do...
I made the first enemy, and made him shoot missiles that will break into pieces if you shoot them.
Also made the gun have a shoot sound effect so yeah lol.
That's it i gotta do some other stuff right now so yeah, cya.

## Day 14

Sorry but I've been busy with irl stuff. I'm still extremely commited to this so I still tried doing some progress even though I can't code atm.

So I've analyzed the gameplay more, so now basically there is gonna be this mechanic that will basically let you gain back lost health. 
So basically, when you kill a lot of enemies in quick succession, you get a bionus depending on how many you killed. This bonus gives a boost to your stats and you gain back some health.
Kills that are not caused from your weapon do not count.

I think this will lead to some interesting dynamics! Can't wait to implement it as well as all the other enemies and upgrades and the shop and... a billion more stuff. 
It's overwhelming but I always like to step back, regain my footing, breathe, focus and think that it's all gonna go fine :)

I've been writing all the upgrades btw, I finished the starter class items, but I'll give the full list once I'm done with them.
In the meanwhile here's a little bit about the synergized classes:

Penetrator has a lot of high dps and high defense stuff, but he doesn't have any health or backup to help him out when he needs it the most.

Wizard has high dps and has a lot of variety of attacks, but he can be very vulnerable without proper handling of his techniques.

Elementor has good defense and attacks, but he relies a lot on being precise and focusing.

Alpha trainer has good backup and dps, but can be left vulnerable very quickly if you aren’t careful enough.

Sigma trainer has good defense and backup, but he’s not very equipped.

Beta trainer has good backup with good attacks that mostly cover him, but his lack of defense leaves him very vulnerable at the beginning.

Aight buh bye :p

## Day 15

Changed the skybox, much better now.