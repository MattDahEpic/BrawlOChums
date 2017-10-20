# Brawl O' Chums
 Abandoned tech demo. The concept worked, didn't end up being a marketable product. The system is still pretty cool.
 
# Usage
1. Clone a local copy of all 3 branches.
2. Change [\[client\]/_resources/js/main.js#L41](https://github.com/MattDahEpic/BrawlOChums/blob/54a14a752b2122649bac1726eff463f97a531444/_resources/js/main.js#L41) to `ws://localhost:36245/ws/`.
3. Run a local web server serving the content in the `client` branch.
4. Change [[game]/Assets/Scripts/StaticBuildData.cs#L8](https://github.com/MattDahEpic/BrawlOChums/blob/game/Assets/Scripts/StaticBuildData.cs#L8) to `ws://localhost:36245/ws/`.
5. Run an instance of the `voxelatedavacadoserver-dev.js` (in `server` branch) under Node.
6. Load the project in Unity and run it.
7. Connect a browser to the web server and enter the room code.

# Contact
Reach out to us on [Twitter](https://twitter.com/MattDahEpic).  
Reach out to us by [Email (at bottom of page)](https://mattdahepic.com).

# License
If you use this code for inspiration or otherwise, **the minimum you must do is tell us**.  
If you're feeling generous, drop a mention to MattDahEpic in the credits for your product, if possible with a link.
