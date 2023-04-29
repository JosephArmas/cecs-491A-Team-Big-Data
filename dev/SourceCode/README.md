# cecs-491A-Team-Big-Data
Team Lead: Joseph Armas\
<br />
Team Members:\
Joshua Gherman <br /> 
Rhoy Oviedo <br />
Frank Curry <br />
Ghabrille Ampo <br />
David De Girolamo <br />
# Run Instructions
Data Base: 	Make sure to follow sql instructions to set up database.

Back-end: 	Open TeamBigData.Utification.sln in Visual Studio.
		In the Solutions Explorer right click on "Utification.EntryPoint" and select "Set as Startup Project"

Front-end:	Open front-end folder in VS Code and open two terminals.
		First terminal run "npm install" then "npm run dev" to turn on front-end.
		Second terminal run "npm run test:cy" to turn on e2e tests.
		When connecting to Cypress accept prompted messaged and configure to e2e testing.
		I used chrome as the browser to test on.
		The tests should be listed, but if not press scaffold specs.
		This will fill your e2e specs and should have map.cy.js and pin.cy.js to be run 