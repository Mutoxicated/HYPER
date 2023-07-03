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
So basically, when you kill a lot of enemies in quick succession, you get a bonus depending on how many you killed. This bonus gives a boost to your stats and you gain back some health.
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

## Day 16

Pretty happy with what all I've done today. I refactored the enemy code, practically finished the first enemy, and set up some stuff for the futures enemies, which will definitely speed up the development process quite a bit. I added the a lil pedastol thing that will have a beam that expands to infinity at the center. This beam will act as a means of teleporting to the shop after the round ends. The same beam will exist in the shop to teleport you to the arena. 

I also made the missile homing, so yeah that's neat.

All in all I just did some really important-for-the-future enemy stuff. 

I'm planning on adding a melee attack, and another movement mechanic tomorrow. I'm also planning to start making the shop in the following days. I'm probably gonna do UI wayyy later. Prolly like in the middle to the end of the next month. Anyway yeah I'm pretty satisfied with what I've done today so yeah cya tomorrow :>

### OH I FORGOT TO MENTION...

I've thought of something, so at the start of each run, you will be asked to choose an "echelon". Echelons determine a lot of stuff, including: your bullets per shot, your passive item pool, and what active/class items will be excluded from the whole item pool. These echelons serve the purpose of reinforcing the idea of already having a build in mind. You cannot change your echelon during the run.

**The echelons are as follows:**

1. SINGULARITY: You can only shoot 1 bullet.
2. DOUBLE-STANDARD: You can only shoot 2 bullets. All their bullet stats split in half.
3. CERBERUS: You can only shoot 3 bullets. All bullet stats equally shared.
4. TETRAHEAD: You can only shoot 4 bullets. All bullet stats equally shared.
5. CINCOS: You can only shoot 5 bullets. All bullet stats equally shared.

Aight now buh bye.

## Day 17

I'm tired. Not because of game development I just had a rough but fun day lol. So here is what I did today:

I fixed some shader stuff, not completely, but mostly. I added the new movement mechanic I was talking about and I also added an glass amphitheatre-type roof over the the arena to make sure that the player doesn't escape. I completely finished the first enemy, which is called "Ether". It's a floating icosphere that shoots a missile at your direction. Shooting the missiles cause them to break into pieces (cool). Also I don't know if I said this already, but I made the beam in the middle that That's it lol.

I have a very modular system for the enemies, which is probably what I will also do for the upgrades. I'm going to do the shop tomorrow though and I will also try to completely fix all the shader problems (there's only one, actually, at least I think so..).

Anyway yeah it was pretty fun today, haven't put much thought into anything lately but I feel better when I don't overthink it.

Anyway I will go to sleep soon so cya.

## Day 18

Added the melee attack I was talking about, I was thinking of making it a punch, but it turned out to be more if a karate attack lmao. I like it, was pretty fun making it and animating it. Learned about rigging models as well today. Other than that I also tried to fix an issue with the shaders but I realized it was an issue with unity... So basically, Unity doesn't write to the depth buffer for materials that are on the "Transparent" queue, and my shader is using the "Transparent" queue to create the quad wireframe look. This means that anything the shader draws on the screen has no depth concept to it, meaning wireframes will be on top of wireframes no matter what. I think you can understand why this is bad. Apparently calculating the depth of transparent objects is very expensive... so Unity just doesn't, if I were to try and fix the issue, I would have to use completely custom shaders, and possibly custom render components... Imo I'm not gonna bother doing that, not because I'm bored (well maybe a bit) but because I don't think I'll have enough time to make those changes. I can't forget that it's a 3 month challenge. 

Anyway yeah that was a big paragraph lol, I'm gonna go now.

## Day 19

Didn't progress on the project today, I took a day off.

## Day 20

Painful day, but I'm so happy I got some really important things done.

The punch melee attack is done, implemented the sceen shake, and implemented player health bar. And also one thing that I'll mention in the end.

I pretty much put screen shake and some particle effects and debris upon punching an object/enemy. I like how the screen shake turned out, you just put in the strength of the shake, number of times you want it to happen, and the interval of each shake, simple. The player health system was the turning point, it was pretty painful to implement, because of all the things I was sub-implementing at once... BUT I did it :D.

Now let's talk about something...

I build the game, right? Playtesting it and all that, when I realized the game was acting differently in the build. I was befuddled, cunfuzzled, and quite possibly flabbergasted. I was trying to find the reason why it was happening, until it occured to me... ***All my scripts were framerate dependent.*** It was all framerate dependent because I wasn't using Time.deltaTime, or Time.fixedDeltaTime in their respective updates, which exist to keep things consistent despite framerate changes. I died inside for a brief moment, then went on to fix every single script I have made up until this point. 

Was it a really painful day? Yes. Do I feel whole-heartedly fulfilled after all that pain? Yes.

Anyway yeah I'm NOT touching this project any more for today lol.

Also 69 changed files committed LET'S GO.

## Day 21 

Didn't do a lot but I made the beam in the center of the arena teleport you to the shop, and i also set up the gameplay systems. So the systems are as follows: Round System > Wave System > Sequence System. And all of these systems depend on the Difficulty System for not only acquiring essential info but also incrementing the info so as to create difficulty progression.

1. Round System handles the round and initializing the wave system.
2. Wave System handles the waves in the round and initializing the sequence system.
3. Sequence System handles the sequences of enemies spawning in the wave.
4. Difficulty System and all of the above handle each other.

It's also worth mentioning that I organized some of the files and folders. Damn the game is getting big...

That's all yeah.

## Day 22 

Completed the gameplay loop, fully implemented, fixed a weird bug that is honestly unity's fault tbh. OnCollisionExit does not get called if you are colliding with an enemy constantly but then it suddenly dies, so the Movement class thinks that you are still colliding with the enemy, and this leads to some errors when trying to jump or slide. Fixed it though by checking if the other collider of the collision is null or not. If it is null, then you are most definitely airborne, and therefore can't jump or slide.

Anyway yeah things are going fine, but I still like haven't even done half of the game, which is kinda scary.

But uhh yep that's all, I'm not concentrating on the game so much because I want to finish some other stuff I've been working on for some time, but haven't had the time to finish them because of the game, lol.

Tomorrow I want to think of a few potential enemies, upgrades, etc. And also make the shop actually be a shop lol. Maybe I'll also think of some lore for the enemies as well lol idk.

## Day 23 

I didn't open Unity or matter of fact anything today. I did the unthinkable... I WENT OUTSIDE!! I touched GRASS! I made a staring contest with the SUN!! 

Jokes aside it was a fun day.

## Day 24 

I almost made the second enemy, I don't have time to finish it today. I changed the mesh for the particles, they look better now :] I also thought of some new items and stuff so yeah. Will explain tomorrow.

Anyway yeah, going to sleep now.

## Day 25

So I did some stuff, not a lot because the development today went frmo ok to catastrophic to can it get worse? to it got worse to ok again. I do not want to talk about it. Lol. I thought of some new items again, kind of reorganized the item list because it's getting concerningly big...

TOMORROW I am 102% sure that I will show you some of the new items and some more gameplay ideas that I've kept to myself for the past few days. But yeah that's all I gotta say. Bye !!