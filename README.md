### CncCalculator

Calculator for CNC feed and speed, with conversion of units from or to inches and mm.

[Git repository](https://github.com/papaathome/CncCalculator)


![screenshot](https://github.com/papaathome/CncCalculator/blob/main/doc/CncCalculator_screenshot.jpg)


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
location and using default settings. You can drag the form to any location and
when you close the application it wil reopen on exact the location where you
closed it. This is handy when you arrange your (CNC related) tools on fixed
positions on the screen.

By default, on the top you find a menu bar with entries like ‘file’ and
‘about’, below that is an tools bar. There are two tabs available, one for
calculating 'Feeds and Speeds' and a second one for doing independent
conversions of lengts and speeds to a number of units..

On 'Feeds and Speeds': The tools bar contains two selection buttons, one for
a tool and another for a material. Selecting one will fill in the ‘Tool’ or
‘Material’ section with values from the selected item. You do not have to
select any tool or material, it is possible to use CncCalculator by filling in
all required information by typing data in the ‘Tool’ and ‘Material’ section.
(see below)

There are two sections for input, ‘tool’ and ‘material’, and
one for result. Working with wood it is common to use a recomended spinle
speed, working with other materials a more accurate specification of the tool
cutting speed can be used. Selecting between the two methods is done by
selecting the round selection button after ‘Cutting speed’ or ‘Spindle speed’
in the material section. The one not selected will be calculated from the other
and a given tool diameter.

Input fields are coloured white and calculated (readonly) fields are gray.
Next to the field with the value is a button with a list of units.
For edit fields you can select the appropriate units for your value. No ‘on the
fly’ conversion of the value is done, assuming that the value is the right value
for the units you select. For calculated fields an ‘on the fly’ conversion is
done and the value is changed to the correct value for the units you selected.
You can select any value, calculated or input, in the edit field and
use ‘copy/paste’ shortcuts to get the values to/from the clip board. Or you
could just read and type it from/into your final application.


Source and NUnit testing
------------------------

CncCalculator is realised with the following layers of code.
 1  The user interface (CncCalculator.exe, Caliburn Micro)
 2  Calculations of values with unit information (libScaledType.dll)
 3a Information about units (libScaledType.dll, conversion factors etc.)
 3b String Parser for units (libScaledType.dll, manual coded recusive descend parser)

Calculations done in libScaledType.dll are validated with a comprehensive set of NUnit tests.
The API to libScaledType classes for this is considered to be a `contract` and as long as the API
delivers what is promissed that is ok. For this reason NUnit testing is realised as `black box` tests.
This is an option because there are only a few dependencies and hardly any mocking is required.
Tailoring tests to the underlying code, `white box` tests, feels like chaising white rabbits.
Time consuming to realise, too specific and not always necesary. If uncovered problems show up
NUnit tests will be refined, maybe with white box tests, to cope with it.

Note: The recursive descend parser and unit information, all at level 3, is not covered (yet)
with a comprehensive set of NUnit tests.


Release history
---------------

 * soon: v0.3. .NET 8 windows version. (breaking change, .NET framework no longer supported.)
 * 03/IV/2021 v0.1-beta.0, first release. .NET framework 4.6 (also available: v0.1-beta.0D, debug version).


Project details
---------------

 * Platform: .NET 8 windows (tested) and other platforms supported by .NET 8 (untested).
 * Language: C#
 * Package dependencies:
    * Caliburn micro
    * Log4Net (not essential but handy for debugging.)
    * Newtonsoft.Json
