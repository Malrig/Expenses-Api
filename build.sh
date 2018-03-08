#!/bin/bash

# Exit if any command fails
set -ev

# Output the version being built against
echo dotnet --version

# Display minimal restore text
dotnet restore --verbosity m

dotnet test ./Expenses-Api-Tests/Expenses-Api-Tests.csproj -c Release
dotnet build ./Expenses-Api/Expenses-Api.csproj -c Release