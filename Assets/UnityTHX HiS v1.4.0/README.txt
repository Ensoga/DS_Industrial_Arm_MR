----------------------------
UnityTHX HiS package v1.4.0
----------------------------

* Release notes (v1.4.0):
	- CloudReciever and CloudSender merged into CloudHandler.
	- CloudRecieverHelper and CloudSenderHelper merged into CloudHandlerHelper.
	- Multiple aux methods added.
	- RecieverExample scene deleted.

Created by: Victor Igelmo 2021 - 03 - 21
IMPORTANT: Please, if you get "Unauthorized" with your appKey, or you do not have one, contact Victor Igelmo (victor.igelmo.garcia@his.se) to get an up-to-date appKey.

----------------
To be developed
----------------

	- Correct debug info (probably CloudSender.cs or CloudSenderHelper.cs)
	- Catch lack of SetOneTimeListener() [no listener added]

--------------------
Older release notes
--------------------

* Release notes (v1.3.1):
	- Helpers now initialize their main classes on Awake(), rather than on Start(). This solves problems of keys not found, and allows to 
		perfom operation on Start in the GameObjects behavious (custom scripts).
	- Added Get functions to reciever helper.
	- Added cohesion between CloudSenderHelper.cs and CloudRecieverHelper.cs
	- Communication error console info as "Error" instead of "Info".
	- Check unity version for WebHelper.Result
	- Improve accuracy of "added listener" message.
	- Add thing to debug message "added listener"
	- Added overload to SetThingProperties() in sender.
	- General bug fix

* Release notes (v1.3):
	- Structure of CloudReciever.cs has been changed. Now it not needed to added as component.
	- CloudRecieverHelper.cs introduced. Interfaces CloudReciever.cs. This script is the one to add as component.
	- Examples updated, including behaviour of Unity elements.

* Release notes (v1.2):
	- Structure of CloudSender.cs has been changed. Now it not needed to added as component.
	- CloudSenderHelper.cs introduced. Interfaces CloudSender.cs. This script is the one to add as component.
	- Examples added, including behaviour of Unity elements.

