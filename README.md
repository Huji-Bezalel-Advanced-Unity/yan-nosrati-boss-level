# Blind Defense
This is a 2D computer game.
# controls
use mouse to aim
use Q, W, R for spell usage

# Plan
architecture - https://drive.google.com/file/d/19fQX5iCFoHXjwprKPKEn_8Rg8FS-TKKh/view?usp=sharing                               
Systems -      https://www.canva.com/design/DAGFhT_zlL8/TzasQ9Z8eVaQGl76XoQigg/edit?utm_content=DAGFhT_zlL8&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton   
Loading plan - https://github.com/Huji-Bezalel-Advanced-Unity/yan-nosrati-boss-level/edit/main/Assets/LoadingPlan.puml#L10C24
## Story
The Greek god Hades, furious with the sins of a village, descends upon it with his underworld army to exact punishment. As the commander in chief, you are tasked by the king to defend the village entrance with limited resources. Can you navigate through the onslaught and defeat Hades and his army?

## Boss Mechanics
- Hades is invisible, moving unpredictably across the right edge of the screen.
- Hades can summon:
  - SkeletonWarriors: Fast-moving, low-health units that deal minor damage.
  - BigSkeletonWarriors: Slow-Moving paced units with high health and damage.

-  Hades can throw large rocks at the player when he has low health.

## Boss Phases
- Phase 1: Hades moves slowly, summoning warriors at a moderate frequency.
- Phase 2: When Hades reaches 60% health, he speeds up and summons warriors more frequently.
- Phase 3: At 30% health, Hades changes movement strategy and earns rock-throwing abilities.

## Boss Design
Hades, portrayed as a formidable Greek god, embodies power and wrath.

## Player Mechanics
- The player automatically shoots arrows from the village post towards the mouse direction.
- The player can summon warriors to aid in defense, gaining weapon upgrades as enough warriors reach the end of the screen.
- The player can throw fairy dust arrow to reveal Hades briefly.
- The player can shoot a super arrow with a significant cooldown and damage, reveals and stuns Hades.

## Level Design
- The level is set at the rocky entrance of the village, featuring a medieval aesthetic.

## AI
Warriors looking and fighting eachother as a group or singularly.

## Sound
- Epic battle sounds

## VFX
- Electic lines, trails, hit effects, damaged affects.
