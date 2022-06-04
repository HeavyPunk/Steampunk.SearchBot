#!/bin/bash
dotnet build Steampunk.SearchBot.sln
rm -Rf ./bin
mkdir ./bin
cp -r ./SearchBot.Worker/bin/Debug/net6.0/* ./bin/
cp commonconfig.json ./bin/
cp config.json ./bin/


