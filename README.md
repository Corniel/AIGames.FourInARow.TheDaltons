The Daltons
===========

The Daltons is an AI trying to play Four-in-a-Row.

Version 32
----------
Hugh improvement on speed (benchmark on 10 ply: 3.4 seconds to 25 ply instead
of 7.5). Also added detection on direct winning multiple threats and threats on
two different columns.

Version 31
----------
Different values for different threats.

Version 30
----------
Fixed strong threat bug, and skipped 2 out 4 now.

Version 29
----------
Tweaked evaluation to also take 2 out 4 into account.

Version 28
----------
Speed improvement with different sorting.

Version 27
----------
Further tweaking of time consumption.

Version 26
----------
Redone Branching.

Version 25
----------
Tweak in evaluation and time consumption.

Version 24
----------
Made lowest winner less winning, and made branching bigger.

Version 23
----------
Fixed evaluator bugs and added odd/even winning position detection.

Version 22
----------
Added double threat-detection and huge speed improvements.

Version 21
----------
Made book moves less forcing.

Version 20
----------
Just play 3 at the start.

Version 19
----------
Don't look beyond ply 9 until we are there.

Version 18
----------
Also keep losing nodes.

Version 17
----------
Start calculation on move 1.

Version 16
----------
Totally refactored the search tree, and simplified it.

Version 15
----------
Tweaked book.

Version 14
----------
Added an opening book.

Version 13
----------
Time fix, and fix for forced moves because of single open column.

Version 12
----------
Tweaked and improved evaluation. Branching restriction. More time-consuming to
start with.

Version 11
----------
Static evaluator for the board position.

Version 10
----------
Struggling with cleaning up.

Version 9
---------
Clear all cache of previous rounds.

Version 8
---------
Added sorting tweaks and a premature book.

Version 7
---------
Fix for not be able to send winning moves.

Version 6
---------
Fixed tons of issues with the search tree.

Version 5
---------
Fixed tons of bugs, in parsing, ply-determination, and evaluation.

Version 4
---------
Removed debug output.

Version 3
---------
Because of missed communication changes.

Version 1
---------
First attempt. Not to smart yet.