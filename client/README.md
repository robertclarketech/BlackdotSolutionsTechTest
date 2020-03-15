# Project Structure  

All source files can be found under `/src`. 

All Elm files can be found under `/src/elm`

All distribution files can be found under `/dist`  
  
# Building from Source

* Run `npm i` or `yarn` in the root folder
* Run any of the following in the root folder (feel free to substitute `yarn` for `npm run`)
	* Running `yarn dev` will compile a new unoptimized app.js in dist with elm set to debug mode
	* Running `yarn serve` will start a devserver that watches for changes on localhost:8080 (if available) running an unoptimized debug build
	* Running `yarn prod` will create an optimized production build in dist

# About this project

Since the spec was very kindly open to any technologies, I decided to use the Elm language for the front-end of this project. You can find out more about Elm from [it's website](https://elm-lang.org/). The quick description is that it's a front-end language that compiles to JavaScript. It has a very comprehensive compiler that ensures safety and performance, and the Elm Framework has been the inspiration for many state libraries [such as Redux](https://redux.js.org/introduction/prior-art/#elm).