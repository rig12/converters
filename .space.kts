job(".NET Core GPNA.Converters build,test and publish"){
    container(image = "mcr.microsoft.com/dotnet/sdk:5.0"){
        env["FEED_URL"] = "https://nuget.pkg.jetbrains.space/gpna/p/gpna-common/common/v3/index.json"
        shellScript {
            content = """
                echo DOTNET ADD GPNA.COMMON...
                dotnet add ./GPNA.Converters package GPNA.Common --source https://nuget.pkg.jetbrains.space/gpna/p/gpna-common/common/v3/index.json
                
                echo PUBLISH NUGET PACKAGE...
                chmod +x publish.sh
                
                ./publish.sh
            """
        }
    }
}
