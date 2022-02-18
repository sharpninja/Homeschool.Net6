Push-Location
try {
    dotnet restore Homeschool.Net6.sln --configfile Servers\nuget.config
    Set-Location Servers
    dotnet restore Servers.sln --configfile Servers\nuget.config
}
finally {
    Pop-Location
}