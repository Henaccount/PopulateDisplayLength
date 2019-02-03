# PopulateDisplayLength
<pre>
Problem to solve
1. length of pipe cannot be annotated in the feet/inch string format on the ortho
2. elevation cannot be annotated as decimal feet value rounded with precision of 2
 
Prerequisites
The dll has to be loaded with “netload”
or you place the following string in the
C:\Program Files\Autodesk\AutoCAD 2017\Support\de-de\acad2017doc.lsp (installation language dependend)
for automatic loading (use slashes not backslashes):
(command "_netload" "C:/Program Files/Autodesk/AutoCAD 2017/PLNT3D/pssCommands.dll")
It requires the custom properties "DisplayLength" and "DisplayElevation" for the pipe object

How to use
The command is: "populateDisplayFields"
It will loop through all pipes in the open drawing and 
1.copies the value that is underlaying the "Fixed Length" value formatted as feet/inch string 
(rounded with precision: 4) into the string field "DisplayLength". 
2.copies the value of "Position Z"/12 formatted as feet decimal
(rounded with precision: 2) into the string field "DisplayElevation". 
This field can be annotated as it is and will not be updated automatically.
 
See this article about how do create the dll and how to install it: http://autode.sk/2jYKHJy 

</pre>
