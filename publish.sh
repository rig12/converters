#!/bin/sh

echo "Configure credentials"
dotnet nuget add source $FEED_URL -n space -u "%JB_SPACE_CLIENT_ID%" -p "%JB_SPACE_CLIENT_SECRET%" --store-password-in-clear-text
VERSION=1.0.$JB_SPACE_EXECUTION_NUMBER

echo RUN BUILD...
dotnet build  
                
echo "Publish nuget package"
cd dotnet
# Beware create old format debug info
dotnet pack -p:IncludeSymbols=true -p:SymbolPackageFormat=symbols.nupkg -p:PackageVersion=$VERSION -o ./
dotnet nuget push GPNA.Converters.$VERSION.nupkg -s space
dotnet nuget push GPNA.Converters.$VERSION.symbols.nupkg -s space
