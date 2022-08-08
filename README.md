# AutoCloseDoors
### Server Only Mod
Server only mod for Auto Close Doors.

## Installation
Copy & paste the `AutoCloseDoors.dll` to `\Server\BepInEx\plugins\` folder.

## Removal
If your server is on windows, or can be shutdown properly.\
All you need to do to remove the mod is shutdown your server and delete the dll.

If your server is on linux, or can't be shutdown properly\
Make sure to change the config of `Enable Uninstall` to `true`,\
Restart your server, and then execute the uninstallation command [default `~autoclosedooruninstall`].\
After the uninstallation is complete, shutdown your server and remove the dll.

## Wetstone Version
1. Uncomment `<!--<DefineConstants>WETSTONE</DefineConstants>-->` in `AutoCloseDoors.csproj`
2. Rebuild the dll.

## Config
<details>
<summary>Config</summary>

- `Enable Auto Close Doors` [default `true`]\
Switch on/off auto close for doors.
- `Auto Close Timer` [default `2.0`]\
How many second(s) to wait before door is automatically closed.
- `Enable Uninstall` [default `false`]\
Do not enable for better performance on server.\
This uninstallation method is only required on servers that can't shutdown properly, like VRising on Linux Wine.\
On Windows, servers can be shutdown properly, and all doors is by default reverted back to normal on server shutdown.
- `Uninstall Command` [default `~autoclosedooruninstall`]\
Chat command to uninstall mod. Only work if "Enable Uninstall" is set to true & the user is an Admin (adminauth).

</details>

## Commands

<details>
<summary>autoclosedooruninstall</summary>

`autoclosedooruninstall`\
Revert all doors in the game world to not close automatically.

</details>

## More Information
<details>
<summary>Changelog</summary>

`1.0.2`
- Fixed issue with servants causing door to be stuck.

`1.0.1`
- Now properly initialize config when reloaded with wetstone.

`1.0.0`
- Initial Release

</details>

<details>
<summary>Known Issues</summary>

### General
- When doors are built for the first time, the user will need to open the door twice for the door to open.

</details>