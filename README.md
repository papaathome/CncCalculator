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


Quick start
-----------

Assuming that you know what ‘Feeds’ and ‘Speeds’ are this is a quick run
trough for CncCalculator.

When you start CncCalculator for the first time it comes up on a default
locationand using default settings. You can drag the form to any location and
when you close the application it wil reopen on exact the location where you
closed it. This is handy when youarrange your (CNC related) tools on fixed
positions on the screen.

By default, on the top you find a menu bar with entries like ‘file’ and
‘about’, below that is an tools bar. You can drag  this tools bar to any side
of the application, top, left, right or bottom.

The tools bar contains two selection buttons, one for
a tool and another for a material. Selecting one will fill in the ‘Tool’ or
‘Material’ section with values from the selected item. You do not have to
select any tool or material, it is possible to use CncCalculator by filling in
all required information by typing data in the ‘Tool’ and ‘Material’ section.
(seebelow)

There are two sections for input, ‘tool’ and ‘material’, and
one for result. Working with wood it is common to use a recomended spinle
speed, working with other materials a more accurate specification of the tool
cutting speed is required. Selecting between the two methods is done by
selecting the round selection button after ‘Cutting speed’ or ‘Spindle speed’
in the material section. The one not selected will be calculated from the other
and a given tool diameter.

Input fields are coloured white and calculated fields are gray.
Next to the fieldwith the value is a button with a list of units.
For edit fields you can select the appropriate units for your value. No ‘on the
fly’ conversion is done, assuming that the value is the right value for the
units you select. For calculated fields an ‘on the fly’ conversion is done and
the value is changed to the correct value for the units you selected. You can
select any value calculated or any value in an edit field and
use ‘copy/paste’ shortcuts to get the values to/from the clip board. Or you
could just read and type it from/into your final application.


Release history
---------------

 * v1.0-beta.0, first release (also available: v1.0-beta.0D, debug version).


Project details
---------------

 * Platform: Windows.
 * Language: C# (but avoiding as much as possible any C# specific constructions or .NET specific objects.)
 * Package dependencies:
    * Log4Net (not essential but handy for debugging.)
    * Newtonsoft.Json
