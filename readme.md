# Flights
Simple game- and tournament calculate for Darts matches

## Features
Currently four game types:
- X01
- Cricket
- Cut-Throat Cricket
- Around the Clock

All types may be played as single game or in a tournament.  
Customizable winner jingles and in-game sound effects.  
Viewer mode for secondary spectator display
Light and dark app-themes

## Deployment
Published as multi-arch docker image (amd64/arm64)  
Checkout repository, then from repo-root:  

for linux/amd64
> docker compose up -d  

for linux/arm64
> docker compose -f docker-compose.rpi.yaml up -d

for other platforms you can do a build from scratch
> docker compose -f docker-compose.local.yaml up -d

The started container can be reached on

>localhost:8080

Note 1: per default, port 8080 is used to map to the container.  
Change the compose file if necessary  
Note 2: All compose files apply a custom timezone!  

## Hardware
Designed to work on a RaspberryPi 4 (4gig version) with a  
10"/1280x800 touch-display. Since I suck at UI, the frontend  
is somewhat hard-wired for that resolution as of now.

<img src="pics/main.png" alt="main" width="600"/>
<img src="pics/game.png" alt="game" width="600"/>
<img src="pics/trnmnt.png" alt="tournament" width="600"/>