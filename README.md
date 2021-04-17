### CncCalculator
Calculator for CNC feed and speed, with conversion of units from or to inches and mm.

[Git repository](https://github.com/papaathome/CncCalculator)


![screenshot](https://github.com/papaathome/CncCalculator/blob/main/CncCalculator_screenshot1.jpg)


Overview
--------

**Why another tool for calculating CNC feeds and speeds?**

There are already several tools available on the internet which will do the job and are managed by companies with more knowledge and resources available than just another lone coder.
All have their strong and also some weak points. And that is where the differences are. Also, what a strong point is for one can be a a weak point for another, just depending on your point of view.
You should choose the tool with as many strong points that help and as few weak points that bothers you. That is how CncCalculator can make a difference.

What are the points for CncCalculator that make it interesting? You can classify it as a strong or weak point for yourself.
 * All source code is available and a permissive licence form.
 * Selection of units, e.g.: [in] or [mm].
 * Conversion of units, e.g.: from [in/min] to [mm/sec]
 * Includes a list of materials with preset values.
 * Includes a list of tools with preset values.
 * Can import any FreeCAD tools library (both v0.18 or before and v0.19 or better)
 * Where you put it on the screen there it will be the next time you start it.

Release history:
 * v1.0-beta.0, first release (also available: v1.0-beta.0D, debug version).

Project details:
 * Platform: Windows.
 * Language: C# (but avoiding as much as possible any C# specific constructions or .NET specific objects.)
 * Package dependencies:
    * Log4Net (not essential but handy for debugging.)
    * Newtonsoft.Json
