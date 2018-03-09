#!/bin/bash

# Exit if any command fails
set -ev

# Output the version being built against
dotnet --version

runtests=true
while (( "$#" )); do
  if [[ "$1" == "--notests" ]]
  then 
    echo "Not running tests..."
    runtests=false
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
