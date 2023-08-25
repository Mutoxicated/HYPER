# Devlog

**This is a log of my entire development process of HYPER.**

## Day 1

Day 1 is as simple and dry as it can get. Creating the repository, project and just analyzing the concept.

## Day 2

Made the groundworks of the player today: movement, camera look, all that.
Movement is almost done.
The movement is very similar to ULTRAKILL, but that's not intentional and is only because ultrakill just generally has good basic movement mechanics.
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

## Day 26

Okay jesus so I finally finished the new enemy, it's called Disco and it's a disco ball that bounces around the place, acting crazy and all that, while shooting a laser at you. However there are still problems with the graphics... I tried fixing them today, and felt like I was very close... But nothing worked. Contrary to my past opinion, I do think this transparency sorting problem can be fixed, some people have found solutions. I scoured the whole internet trying to find possible solutions, I EVEN WENT ON PAGE 2 OF GOOGLE. 

I found a promising solution, that practically wrote to the depth buffer first before all the rendering of the transparent objects, which actually makes sense kind of, however that didn't work... It thought it doesn't because maybe the render sorting is set to front to back, but I realized I was wrong, because the transparent queue renders back to front, soooo idk at this point what could be wrong. 

Anyway I feel like everything is a mess right now, primarily because of how I've set up the whole enemy system (it being a bunch of modules that you can assign to gameobjects). But honestly I don't know If I could do it any other better way (i probably could but whatever). I just like it this way because the code is very conveniently recyclable for other games I'm planning to make so yeah. I do have almost everything set up right now, which relieves me a lot, but there are still the upgrades to make. 

What I've thought about mostly though is of an interval event system thing. Which I commited myself to just because of how versatile it could be. Basically I imagined a module that has a timer that resets when it has reached a certain amount of time. Not only that, but it also invokes an event when it resets. This module has a t variable that is the current time divided by the interval, which gives a value from 0 to 1. This t variable is public and accesible. The main idea is that everything that requires doing something at a given interval will all depend on this singular module to tell them when to do something. This also lets us properly sync actions, which is very nice not to worry about. So things like Shoot(), but also, for example, ResetVelocity(), can be done at the same time by adding these as listeners in the interval event. Then, when the interval event is invoked, both will be called. I really like it this way and I think it's a good way to do things imo.

Anyway I just realized that it's July which kind of worries me because 1/3 of my time has passed for this challenge... and while I know a month since I started still hasn't technically passed, I still think I need to speed things up. I'm gonna go now though so yeah bye !!

### BY THE WAY

Downloading the project in this version will result in many missing prefabs probably, I've unpacked a lot of prefabs because of missing scripts warnings (that show up for no reason...) so yeah next version I will fix everything.

## Day 27

Pretty slow progress, mostsly due to me trying to make things flexible enough for the future. I feel like I learned a lot during these 27 days, like, A LOT. Not only about coding architecture, but also about graphics, shaders, game design, etc. It's just been a nice 27 days y'know. So I got stuck on this problem that I believe I have found the solution that wouldn't cause any other problems. Surprisingly, the solution not only involved making a script, but changing an existing script to make it better and more flexible, while being acceptable in my mind. 

The problem was, I wanted for particles to be colored depending on how the enemy was colored at that moment. This isn't as easy as it seems, however there are plenty solutions. One solution I thought of was to make an interface, with the function ColorParticle(Color color), and call it when instantiating the particle like this maybe: particleInstance.GetComponent<IColorParticle>()?.ColorParticle(color). But I quickly rejected that idea because particle instances are going to be pretty frequent and I would rather not to be calling GetComponent a lot of times, as it would probably reduce performance. Not only that but the idea wasn't all that entirely promising to me and didn't feel very intuitive. 

So I thought of some other stupid problems, until one solution stuck with me, which was to have the enemy be instantiated with deactivated particles that would be activated detached from the parent upon death of the enemy. This sounds good, because the particles are already instantiated and I could very easily imagine a script using OnEnable() function to set the particle system color to that of the color of the enemy. But of course there was another problem. I realized that my missile code was detaching all of the children upon death... All of them, including the particles. But wait, the missile isn't supposed to have any particles activating when they are dead, instead there are supposed to be fragments of them exploding, and only when it collides something it should activate the particles. 

So I thought about it until I realized the solution was extremely simple: just like I'm destroying gameobjects upon death, let's detach certain objects upon death! This fixes everything because now, it will not detach the particles upon death unless we put the particles in the array. 

Anyway I didn't have enough time to implement everything, tomorrow I will though, bye!!!!!

## Day 28 

Did the last finishing touche sot the discos enemy. These 28 days were fun. Here's a list of what I did:

1. Movement Mechanics
2. Graphics
3. 2 enemies
4. Attacking mechanics (melee, weaponry, etc)
5. Gameplay Systems (except difficulty progression)
6. Gameplay UI (except some small stuff like showing the current wave, current round, etc)

Yup, and a few sound effects I guess. So what now? I'll tell you what. This month, I need to do:

1. Sound effects
2. Rets of Gameplay UI
3. Difficulty progression
4. Shop layout/environment
5. More enemies
6. Upgrades

Seems like a fair amount of stuff I'd say. The next month I'll definitely start on like Main menus and all the UI.

I have an idea about the shop. Whilst playing the game, everytime you teleport to the shop, a new random little piece of decoration appears. I just think that would be kinda neat. Like just have an array of preplaced decorations that are all deactivated and one random out of all gets activated after each round.

### Also...

If I remember correctly on Day 25 I told you guys I'd tell you about some of the upgrades and gameplay ideas I had thought of. I completely forgot about it lol. Let me tell you about the gameplay idea I thought about:

So there will be these moments in the round called **Red Intermission** (might change the name later), that happen spontaneously and get more common as difficulty progresses. The arena becomes completely red and every enemy receives a random buff. Red intermissions only last about 10 seconds but you have to be very wary and cautious not to die. Feels like it would create some traumatic experiences on the person who is gonna be playing the game, and that is just what I want :)

Anyway I can't talk about the upgrades because they arent really done, it's pretty slow progress on that side but yeah.

Also I kinda reworked the melee script because of some weird bug. If I pressed F to punch it would lag for a second ONLY the first time, every other time it was fine, which is really weird tbh.

Anyway yeah bye.

## Day 29

So I made the gameplay UI, I made it show the current wave you're at and also whenever you teleport to the arena it pops up a floating message saying the current round. Pretty cool if you would ask me.

I also drew the shop layout and then just made the exterior in blender, here's a goofy ahh pic:
![image](https://github.com/Mutoxicated/HYPER/assets/96009711/ff8c983e-c849-4da1-a8fa-a18c7ffe4ac3)

Anyway that's all for now. I'm planning on finally making the enemy health bars that were planned since i made the enemy health script (long time ago) and uh yeah. I also have been thinking about refactoring the cameraShake's code design... Like how it's applied and how I could make it better and more flexible. 

I know that I shouldn't constantly be thinking about flexibility too much, because that slows my progress, but I think when you hit the sweetspot and think just enough for it, the oppsoite actually happens. In my opinion, thinking about the flexibility for the future to its almost bare minimum (and still thinking about possible exceptions and how to cover them) is very important, because you prepare yourself and make it easier for your future self to handle things later down the road. I think this is just about thinking not only analytically, but also how your intuition feels about the things you implement, with regards to everything else. Sometimes though you have to be analytical instead of thinking intuitively.

Today I made the FadeMatColor more flexible to make it also applicable in TMP texts. Not only that, I also made this really cool (in my opinion) implementation, where you can choose if you want certain FadeMatColor color values to apply to your material, or whether you want it to apply the material's color value instead. This also let me slowly fade out the text showing the current Round number in the center, while still making it change color. I also had to make a change in the OnInterval script so that now it has the choice to self destruct once the interval has reached. I only did this because unity doesn't have a Destroy() function in the GameObject when picking for functions in the inspector in the UnityEvent, sooo yeah.

By the way, no progress on the upgrades :p Sorry... I just haven't really been very active with the game or with any of my hobbies. Maybe because I've like worked on the game these past 28 days for more than 6 hours a day. I mean, that's what happens when you set up a deadline that lasts 3 months haha.

Anyway yeah that's the end of day 29! With the amount of writing I'm doing, I'm prolly gonna write like 1200+ lines lol.

## Day 30

I advanced the CameraShake script, I ended up realizing that camera shakes are applicable in explosion, which means that it's also applicable in most particle explosions, so I don't have to change the code a lot because you can just add the camerashake on a particle and the shake just happens to the player's main camera. 

Anyway I wanted to implement the health bar but I ended up fixing another problem with the graphics that's kind of stupid to be honest. So for some reason that I am not fully aware of, the post-processing effects dont go by specific camera and happen in every camera even if it's not set to do that effect in the other cams. Currently I have 3 cams set, one for UI, that renders last, one for the weapon, which renders after the 3rd camera that hold renders all of the environment. 

So to fix this problem I'm basically creating 3 different render textures for each camera, and then I'm trying to combine all those renderTexture into one and then copy that into the main render texture that the player can actually view. The way I'm doing things though don't work, and I'm sure it's just a very small detail that I haven't picked up on.

## Day 31

I implemented the enemy health bar, it shows up below the enemy once you hit it and after some time it disappears. I fixed the problem with the render textures, almost. So now I CAN combine render textures with each other, but the problem is that it doesn't have a sense of depth. So it just looks like the texture has just been plasted onto the main texture, overlapping with objects in front of it.

I'm sure I'll find a fix or something.

I was importing the shop exterior blender mesh into unity, but after seeing how scuffed the mesh was with the wireframe material, i realized i could just make the whole shop in unity using scaled cubes, so that's what I'm doing now lol. 

I'm gonna show you guys a video of the game's current state soon, so stay tuned for that hehe.

## Day 32 

I was doing a lot of fiddling with the render textures and shaders, trying to find solutions but I decided fuck it and switched to the Universal Render Pipeline. I switched to URP because it lets me have different post-processing volumes for different layers (if i even remember correctly). I've had some past experience with URPs so it should aallll be fine. So I completely got rid of the custom post-processing files. Did I every mention I got the post-processing stuff from a great youtuber? Check him out: https://github.com/GarrettGunnell.

I made barebones prototype of the stats for the enemies and players. Yeah... I forgot that stats are actually gonna be really important and only today I remembered. 

It's been 32 days since this project started and I still haven't told you guys some of the very basics of the game. So let me present you:

### The Premise

**I will explain the following in sections:**
* The Gameplay
* The Movement Mechanics
* The Offensive Mechanics
* The Arena and the Interlude
* The Player Systems

### THE GAMEPLAY:

So, the gameplay consists of a single Round, that contains challenges that you must go through to survive. There are different types of Rounds. There are currently only **Endurance Rounds** and **Agility Rounds**, there are more planned though, as part of your S.P.E.C.I.A.L-TY ;)

**Endurance Rounds** last for a long time, consisting of multiple waves and sequences of enemies to survive. It's a test of Endurance, and it rewards you appropriately for your success in survival. You get points at the end of the Round plus bonus points depending on how much damage you've taken. 

**Agility Rounds** are quite the challenge, as they don't depend on systematic spawning of enemies. Instead, enemies spawn every single second. There are 3 pillars around the Arena that you have to destroy as fast as you can. Your agility pays off at the end of the round, as you get rewarded with points and bonus points depending on how much time it took you to finish the round.

Other than Rounds, there are also the Waves. Waves are a bunch of sequences of enemies. Each sequence has to wait for the other sequence to finish before starting. Finishing a sequence means killing all the enemies.

Other than Waves and Sequences, in every round, eventually, you will get what's called a **Red Intermission**. Red Intermissions happen almost at random and last for 10-15 seconds. The whole Arena becomes red and all the enemies temporarily get buffs to all their stats during the moment. It's as very nerve-racking and frightening moment, as it happens out of nowhere. 

### THE MOVEMENT MECHANICS:

You've got quite the arsenal of abilities on your hands in terms of movement mechanics. You can:
1. Jump (wall jump up to 3 times)
2. Slide
3. Ground slam while midair
4. Lock yourself in place while midair
5. Launch yourself outwards and inwards from the direction of your current position and the position from which you jumped from. Launching yourself will lead to bouncing up to 3 times (or 2? i dont remember lol)
6. Dash

### THE OFFENSIVE MECHANICS:

There are 4 offenseive mechanics in this game currently. Here's the list:

1. Gun, can range from short to long range depending on your choosing of echelons.
2. Punch, short range, very deadly.
3. Dynamite throw, medium range, pretty good, though you have a limited supply of them that regenerate every start of the round.
4. Minions, i guess?

### THE ARENA AND THE INTERLUDE:

**The Arena:**

The Arena is the place where you fight all the enemies, where the Round begins and ends, where you find yourself clenching both your butt cheeks as tensely as possible.
Every time you teleport to the Arena, it will get larger and larger, and new decorations will be places around to make it more interesting. 
The Arena will get larger so as to not cause a huge clutter in the later stages of runs. 
It just gives more room to breathe while also making the Arena feel fresh and open.

**The Interlude:**

The Interlude is a chill place that you teleport to from the Arena. It's a pretty chill place. In this place, you can go to the **Shop**. The Shop is where you buy your upgrades, increase your dynamite capacity, restore your health, all that. Moreover, there is also this small arcade machine-like building called the **Info-Room**. In this Info-Room, you can find info about all the entities of this planet. 
There is also this small underground room, that when you go in, feels like you're floating in space while you're eating some shrooms that you're not supposed to eat. Very psychedelic stuff. This room is important though, because this is where you get to choose your passive items. You won't always be able to choose a passive items, you can only choose after 3 rounds have finished, or something like that...

Also every time you teleport to the Interlude, a new piece of decoration will appear, no reason really, just why not lol.

### THE PLAYER SYSTEMS:

Here are the player systems:

**CLASSES:**

Classes are a VITAL part of the whole player system. Classes fall into 3 categories:
1. Starter Classes (4 max item capacity)
    - INSANER 
    - WARRIOR 
    - MAGICIAN
    - SUMMONER
2. Synergized Classes (8 max item capacity)
    - PENETRATOR = INSANER + WARRIOR 
    - WIZARD = INSANER + MAGICIAN 
    - ELEMENTOR = WARRIOR + MAGICIAN 
    - ALPHA TRAINER = INSANER + SUMMONER
    - SIGMA TRAINER = WARRIOR + SUMMONER
    - BETA TRAINER = MAGICIAN + SUMMONER
3. HYPER Classes (12 max item capacity)
    - ZEUS = SC + PENETRATOR + WIZARD
    - DIONE = SC + PENETRATOR + ELEMENTOR
    - ARTEMIS = SC + PENETRATOR + ALPHA TRAINER
    - AVATAR = SC + PENETRATOR + SIGMA TRAINER
    - HEKATE = SC + PENETRATOR + BETA TRAINER
    - LIZZARD = SC + WIZARD + ELEMENTOR
    - XENIA = SC + WIZARD + ALPHA TRAINER
    - GRINDER = SC + WIZARD + SIGMA TRAINER
    - MOONER = SC + WIZARD + BETA TRAINER
    - MENTOR = SC + ELEMENTOR + ALPHA TRAINER
    - BALLER = SC + ELEMENTOR + SIGMA TRAINER
    - HEPHAESTUS = SC + ELEMENTOR + BETA TRAINER
    - KAPPA TRAINER = SC + ALPHA TRAINER + SIGMA TRAINER
    - INTER TRAINER = SC + ALPHA TRAINER + BETA TRAINER
    - LAMBDA TRAINER = SC + BETA TRAINER + SIGMA TRAINER

By the way, SC means Starter Class.
Classes are important because upgrades are categorized into classes, and classes predefine certain builds. Upgrades may categorize in multiple classes and therefore will overlap with other upgrades. 

Starter classes are what you start with, synergized classes can be obtained by synergizing two starter classes. This unlocks the item pool of that synergized class, which consequently opens up more synergized classes that can be obtained by either synergizing two starter classes again or buying an item that contains the current class you have plus another one or two. HYPER classes can be obtained the same way, but you synergize synergized classes instead of Starter Classes. When synergizing classes, it will synergize the items as well, however it will only give you X/2 items randomly picked by the computer, where X is the max amount of items your class can have.

**ECHELONS:**

Echelons define not only the amount of bullets per shot you shoot, but also you passive item pool, and what class items are excluded from the whole item pool. Here are the list of echelons:

1. SINGULARITY: You can only shoot 1 bullet.
2. DOUBLE-STANDARD: You can only shoot 2 bullets. All their bullet stats split in half.
3. CERBERUS: You can only shoot 3 bullets. All bullet stats equally shared.
4. TETRAHEAD: You can only shoot 4 bullets. All bullet stats equally shared.
5. CINCOS: You can only shoot 5 bullets. All bullet stats equally shared.

Alright that's all I've spent like 2 hours writing this lol.

## Day 33

Switched back to the built in render pipeline because I realized that URP isn't compatible with the Post-Processing v2 package. Anyway so I basically fixed all of the materials because everything was broken. Yeah today was a big waste of time but oh well.

## Day 34 and 35

Couldn't commit yesterday because a huge wasp invaded my space and it was getting late anyway. BUT I am very satisfied with what I've done today. 

I made the gun have a muzzle flash when you shoot. Small but nice detail. I am planning on adding more details to the movement. Speaking of movement, I'm probably going to change the launch&bounce mechanic, to something a little different but with the same spirit nonetheless. I'm probably gonna make it so that you shoot out a zipline out of your arm and when it sticks to something you can use your scrolling wheel to either launch yourself inwards or outwards to that point the ziplinen stuck to. If you do nothing for 1 second the zipline will come back. So basically the controls are different plus a few other small things. But I'm basically just trying to turn this mechanic into something that would feel satisfying to execute.

I also made the missile enemy become red if eneough time has reached. That's all they're doing right now but soon they will also do some more exciting stuff. 

I think I changed a few other things but I can't remember them. I realized that the enemy modules are really not advanced enough for the enemies that I'm thinking, so I'm advancing them, trying to make them as flexible as creativity can be (well not that much but you get the point).

Oh I also implemented the stats for the enemies and for the player. I will implement the buffs soon. I also made some nice progress for the shop building. 
Here is a pic:

![image](https://github.com/Mutoxicated/HYPER/assets/96009711/1556965c-95aa-4f9c-8de1-ff22b03496ad)

Anyway that's it for day 34 and 35, going pretty well, but I don't think I'll finish it in 3 months, I definitely overestimated my abilities. 3.5 - 4.5 months? Perhaps. But for my 2nd game it's going really well.

## Day 36

I re-implemented the launch movement mechanic. Now you throw a magnetic thing and when it sticks to a wall or ground you have 1 second to decide if you want to launch in or out, bouncing 3 times as you do so.

## Day 37

Reverted the re-implementation, kept some things. Made the health bar cooler, added ui elements.

## Day 38

I'm probably not gonna be doing a lot these following days.

## Day 39

-

## Day 40 and 41

I fixed some really, REALLY stupid things lol. Bugs that should not have been there. Anyway I made a trello board page to have all my to-dos in there to clear up my mind and stop keeping track of everything in samsung notes lol. I theorized some of the enemies, which was really fun. Lemme tell you some!

First of all, here's a list of all debuffs I've currently thought about:

1. Burning
2. Freezing
3. Poison
4. Rain
5. Blindness

Talking about the enemies, there will be organic and non-organic enemies. Organic enemies are immunne to rain, while non-organic enemies are immune to poison. The idea is that poison can't infiltrate a non-organic creature, but it can with an organic one. And with rain, it's the idea that non-organic cretures become rusty over time with rain, while organic creatures are fine.

Here are a few O and N.O enemies:

**Mog:** 
A non-organic creature that can only be described as having a glowy core with multiple rings rotating on it. These creatures stay on the ground because of how heavy they are. But because of this, they have an extreme sensitivity to vibrations in the ground. So when you touch it, they immediately start rolling towards you. If there are many of them and collide, they will create a huge explosion.

**Memoriam:** 
This non-organic creature is a one for all. It is the memories and hopes and feelings of all monsters. It can turn into anything. Because this creature is like a memory suspended in time, it cannot hurt you and you cannot hurt it. However, it can indirectly hurt you through your environment. It can manifest itself into whatever enemy it wants, and you have to endure it, until it goes away.

**Zorretox:** 
An organic wall of extremely compact intestines from the gluttony layer. It moves slowly, constantly spewing toxic liquid everywhere. 

**Siphan:** 
An organic creature that is half-plant half-animal. Because of its two mouths, it is able to blow the air constantly. When blowing, it also releases particles that communicate with each other. If a particle is disturbed, it will communicate with other particles close to it, reaching back to the siphan, which then starts fiercely suck the air until you reach its mouth, where you get damaged.

Here ya go :p

Also I just realized this is day 41 and not day 40 haha.

## Day 42

I've realized there is no way I'm finishing it in 3 months. It's probably gonna take me twice the amount that i thought it would. I don't mind though, but that probably means that I will not be doing daily logs. Instead I'll be logging my progress on a weekly basis. This is exactly the 6th week that has passed since I started development. 

Anyway now I'm making the movement particle effects and all that. I also realized that I can set the color of an instanced material in a 10x easier way, which is to just access the color variable of the material instead of using the function that requires the name that it has in the shader. Turns out if you put the [MainColor] attribute in a shader property or name the shader property _Color, this property will be the color variable of the instanced material that uses that shader. So that's cool and made my life easier.

Anyway there are a few bugs that I'm stuck on and don't know how to fix, so that's fun :)

I reflected upon my game a bit and thought about what would really make it fun. I came to the conclusion that your sheer evolution of becoming overpowered and the visual satisfaction of your power - with a big touch of strategic thinking and decisions that, if done right, will make you feel good about yourself - will make the game very satisfying and fun to play. I hope whoever is reading this can relate to this :]

I think it is obvious though that this game will be very fast paced and adrenaline-inducing.

On top of all that, the graphics make it really unique, as the 3D vector graphics genre has not been explored a lot. I think that's what's gonna grab the attention of many people, since the graphics are the most important first impressions a player gets from your game.

Anyway that's it for today. This week hasn't been the msot productive, and I know that I should be more disciplined with my development and more consistent, so I will do better.\

## Week 7 & Day 50

Here's what I did:

> I added a shield system. You can have a certain namount of shields and once you get hit, they break. The more you have, the bigger the damage reduction.
> I semi-finished the immune and bacteria (buff and debuff) system.
> Making the immune system also allowed me to categorize enemies as organic or non-organic.
> I made some more scripts use stats. Also change the way the stats work. Now each stat has its own lasting duration.
> Fixed right mouse button throwing error.
> Made the in and out launch direction be shown as two arrows in the bottom of the ui screen.


All of that was pretty trivial to do. However, there was one thing, that took HALF of my week. For that half of the week I was in pain, trying to figure out how to implement this. Of course, this thing is graphics-related. 

I wanted to do object pixelization, that being pixelating specific objects in runtime. This is harder than you think. First of all, it is very hard, almost impossible to do this. It's not just hard in a specific case, IT'S HARD IN EVERY SINGLE CASE. Why? BECAUSE UNITY. 

It's still not completely done, I have to fix some things but basically I found an age old post in unity forums that contained half the solution. I'm implementing the other half currently. Basically it takes grabs the main render texture and it downscales and upscales it--the downscale and upscale part is probably the easiest out of all of this--and then simply outputs it. Simple. The downside is, it pixelates everything that the camera looks through the volume. So it not only pixelates the cube, but the background part as well. I'm currently just fixing this so I'm sure tomorrow it'll be good and ready to be used.

## Week 8, 9 & Day 65

Can't be working on the game because laptop cmos battery has got some problems. Will try to fix. In the meantime I will use a different pc and focus on some other stuff.

## Day 66 & 67

So I fixed the problem with my laptop. Back to development!

Let me tell you guys what I've done so far these weeks:
> Implemented object pixelization (still needs some fixing).
> Changed the way stats are modified to now being additive instead of directly modifying the variable.
> Renovated the UI.
> Made space skybox with some particles.
> Fixed bloom not being intense on 0 intensity HDR colors & non-HDR colors.
> Made a super cool enemy spawn effect.
> Tweaked lock ability.
> Made it so that ground slamming and jumping after you come into contact with the ground will give you extra jump force (depending on how long your ground slamming took).
> Made the player throw TNT!
> Made a screen effect that plays when you get hit.

Yeah that was a lot. And I still am not even halfway done, to be honest. I made a profiler check and it shows that it's at a stable 70 fps. It's mostly due to the editor and the post processing effects, but my scripts don't seem to be affecting the fps a lot.

## Day 68 & 69

Soooooo i did some really cool stuff today. It was one cool thing actually. I implenmented a pixelated particle shader that uses vertex streams from the particle system. It's actually so cool.

There was a lot of pain and suffering involved but when I finished it I legit teared up from the joy. It looks really cool imo.

## Day 70 thru 77

Alright, so I've implemented almost all the buffs and debuffs. I've also made player escape lock state through dashing, implemented piercing on stuff like lasers and bullets (not sure about missiles) and created a custom object pooling system that is gonna save a ton of performance. I've almost completed the space island! I've done the campfire site, which I'm pretty happy with.

Now all there is to do before I start actually creating the enemies is to implement absolutely all debuffs and buffs, implement all code designs that will be needed for gameplay flexibility, set up a gun transforming system and make a fisheye lens effect shader.

That is actually a lot of work...

Despite organization, everything is kind of foggy in my head right now. Maybe because I got distracted from the game with another game that I've started to put hours on. Anyway yeah that's it for now.