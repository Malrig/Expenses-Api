#!/bin/bash

set -e

# Install OpenCover and ReportGenerator, and save the path to their executables.
nuget install -Verbosity quiet -OutputDirectory packages -Version 4.6.519 OpenCover
nuget install -Verbosity quiet -OutputDirectory packages -Version 2.4.5.0 ReportGenerator

OPENCOVER=$PWD/packages/OpenCover.4.6.519/tools/OpenCover.Console.exe
REPORTGENERATOR=$PWD/packages/ReportGenerator.2.4.5.0/tools/ReportGenerator.exe

CONFIG=Release
# Arguments to use for the build
DOTNET_BUILD_ARGS="-c $CONFIG"
# Arguments to use for the test
DOTNET_TEST_ARGS="$DOTNET_BUILD_ARGS"

echo CLI args: $DOTNET_BUILD_ARGS

echo Restoring

dotnet restore

echo Building

dotnet build $DOTNET_BUILD_ARGS

echo Testing

coverage=./coverage
rm -rf $coverage
mkdir $coverage

echo "Calculating coverage with OpenCover"
$OPENCOVER \
  -target:"C:\Program Files\dotnet\dotnet.exe" \
  -targetargs:"test $DOTNET_TEST_ARGS ./Expenses-Api-Tests/Expenses-Api-Tests.csproj" \
  -register:user \
  -output:$coverage/coverage.xml \
  -oldstyle \
  -searchdirs:./Expenses-Api-Tests/bin/$CONFIG/netcoreapp2.0 \
  -hideskipped:File \
  -filter:"+[Expenses*]* -[Expenses-Api-Tests*]* -[Expenses*]ExpensesApi.Startup -[Expenses*]ExpensesApi.Program"
  
echo "Generating HTML report"
$REPORTGENERATOR \
  -reports:$coverage/coverage.xml \
  -targetdir:$coverage \
  -verbosity:Error
  
if [ -n "$COVERALLS_REPO_TOKEN" ]
then
  echo "Sending report to Coveralls"
  nuget install -OutputDirectory packages -Version 0.7.0 coveralls.net
  packages/coveralls.net.0.7.0/tools/csmacnz.Coveralls.exe --opencover -i coverage/coverage.xml --useRelativePaths
fi
