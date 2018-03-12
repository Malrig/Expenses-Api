#!/bin/bash

# Exit if any command fails
set -ev

# Output the version being built against
dotnet --version

runtests=true
publish=false
while (( "$#" )); do
  if [[ "$1" == "--notests" ]]
  then 
    echo "Not running tests..."
    runtests=false
  elif [[ "$1" == "--publish" ]]
    echo "Publish the build"
	publish = true
  fi
  shift
done

# Display minimal restore text
dotnet restore --verbosity m

dotnet build -c Release ./Expenses-Api/Expenses-Api.csproj

if [[ "$runtests" = true ]]
then
  dotnet test -c Release ./Expenses-Api-Tests/Expenses-Api-Tests.csproj
fi

if [[ "$publish" = true ]]
then
  dotnet publish -c Release ./Expenses-Api/Expenses-Api.csproj -r ubuntu.16.04-x64 -o ../artifacts/
fi
