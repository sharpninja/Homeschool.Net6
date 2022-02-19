Push-Location
try {
    $config = (Get-Item Servers\nuget.config).FullName
    # $projects = Get-ChildItem *.csproj -Path Client -Recurse
    # $projects += Get-ChildItem *.csproj -Path Server -Recurse
    # $projects += Get-ChildItem *.csproj -Path Servers -Recurse
    # $projects.FullName | ForEach-Object -Process { Set-Location (Split-path $_); & dotnet restore "$_" --configfile "$config"}

    # Pop-Location
    Set-Location Servers
    dotnet restore Servers.sln --configfile "$config"
    Set-Location Homeschool.Server
    dotnet publish Homeschool.Server.csproj --configuration Debug --arch x64 --output "${env:outdir}"
}
finally {
    Pop-Location
}