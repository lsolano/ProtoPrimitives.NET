#!/bin/bash

if [ -z $1 ]; then
    echo "The Nuget API Key was not set (first parameter)."
    exit 1
else
    echo "The Nuget API Key was set."
fi

SOURCE=$2
if [ -z "$SOURCE" ]
    echo "Working with provided Nuget Source '$SOURCE'."
then
   SOURCE="https://api.nuget.org/v3/index.json"
   echo "Working with default Nuget Source '$SOURCE'."
   echo "Use the second command line argument to set the source if you like to set override the default."
fi

# Cleaning dotnet build output
dotnet clean --configuration Release

# Removing previous packages
rm src/main/cs/ProtoPrimitives.NET/bin/Release/Triplex.ProtoDomainPrimitives*.nupkg

# Building and creating packages
dotnet build --configuration Release

# Move to packages root and issue publish command
cd src/Validations/bin/Release
dotnet nuget push *.nupkg -k $1 -s $SOURCE