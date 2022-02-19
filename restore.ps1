Push-Location
try {
    $config = (Get-Item Servers\nuget.config).FullName
    # $projects = Get-ChildItem *.csproj -Path Client -Recurse
    # $projects += Get-ChildItem *.csproj -Path Server -Recurse
    # $projects += Get-ChildItem *.csproj -Path Servers -Recurse
    # $projects.FullName | ForEach-Object -Process { Set-Location (Split-path $_); & dotnet restore "$_" --configfile "$config"}

    # Pop-Location
    Set-Location Servers
    dotnet restore Servers.sln -c Debug --configfile "$config"
    dotnet build Servers.sln --no-restore -c Debug
    dotnet publish Servers.sln --no-build -c Debug -a x64 -o "${env:outdir}"
}
finally {
    Pop-Location
}