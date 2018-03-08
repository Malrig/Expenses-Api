#!/bin/bash

# Exit if any command fails
set -ev

dotnet restore

dotnet test ./Expenses-Api-Tests/Expenses-Api-Tests.csproj -c Release
dotnet build ./Expenses-Api/Expenses-Api.csproj -c Release