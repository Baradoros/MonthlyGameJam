# MonthlyGameJam

# FMOD Integration

## Working with ONLY the GAME, not FMOD
* You may get errors on first load, hit play or ignore them until Unity is pointing to the FMOD project file.
* You will need to adjust/create the path to the `FMODBanks` folder, located in the root (e.g. a sibling to `Assets` folder. 
* FMOD > EDIT SETTINGS
* In settings, you will use the "Single Project Build" button
* Browse... and find your local path to `FMODBanks`
* This will likely require the banks to be refreshed FMOD > REFRESH BANKS

## Working with ONLY FMOD, not the GAME
Audio Repo : https://github.com/webthingee/MGJAudio

## Working with FMOD, and the GAME
Audio Repo : https://github.com/webthingee/MGJAudio

* Structure w/ both repos should look like this example:

```
drwxr-xr-x  8 user  staff  256 Apr  1 15:55 FMODAudio
drwxr-xr-x  8 user  staff  256 Apr  1 15:55 FMODGame
```

* The important part is that the FMOD project files and the UNITY project are not in the same hierarchy. The specific are really up to you.
* You may get errors on first load, hit play or ignore them until Unity is pointing to the FMOD project file.
* FMOD > EDIT SETTINGS
* In settings, you will use the "Project" button
* Browse... and find your local path to FMOD project you got from git.
* This will likely require the banks to be refreshed FMOD > REFRESH BANKS

NOTE:
* The sound dev will be able to use the Project, to create the .bank files in the /StreamingAssets folder. - however -
* Before commiting the GAME to the repo. The sound dev will need to copy the .bank files to the FMODBanks folder referenced above.

## Test Scene
* There is a scene in the FMOD_UI for testing.
* You may need to assing Music and a Test sound effect. Both of which need to be 2D at the moment.

## Notes
* Using a holdDirectory.MD file to ensure some directories are created and the .meta is tracked in and effort to make conflicts less likely.
