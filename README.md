# Stack
This project is an attempt at recreating Stack by Ketchapp.  
It was done in 3.5 hours as a class exercise.  

## Features
Floors are either coming from the left or the right and speed up every 10 platforms.  
For each block, hue shifts so it looks like a rainbow at the end.  
Blocks divide when you fail to place them perfectly but slowly grow back when you  
successfully place them at least 5 times in a row.  
The game is played in isometric view. An effect is played on perfect placement.

## Limitations
Perfect placement is taken into account when the difference between the previous and  
current platform is less than 5% of the traveled width, so it becomes more and more  
difficult the smaller your current block becomes.

## Known Issues
Whenever a platform grows, the tower becomes miscentered.

#
> Disclaimer:  
*This was a project made in class.  
It's an exercise to help us understand how to make good prototypes in a short time.  
This project is now on GitHub for accessibility and visibility.*
