

The app reads user input from a textbox to the left and auto-saves it into 
the selected file every 5 seconds. Changes to the file are displayed immediately
in a read-only textbox to the right.

Only new input is appended to the file. If already saved input is edited in the 
textbox, these changes are ignored.

File can be changed on the fly.

Things to improve:

- User input and file contents area are implemented as two user controls which are placed 
on the main app window. If required, they may be placed on two separate (torn-out) 
windows, to be displayed on different monitors.

- Additions to user input control: "save input now" button and a control to change auto-save interval.

- Put a limit on user input length: trim aready saved input that exceeds the limit.

- There is a limit on displayed file contents, however, the control still reads the whole file. 
Should read only the required portion.
