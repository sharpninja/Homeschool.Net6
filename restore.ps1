Push-Location
try {
    $config = (Get-Item Servers\nuget.config).FullName
    $projects = Get-ChildItem *.csproj -Path Client -Recurse
    $projects += Get-ChildItem *.csproj -Path Server -Recurse
    $projects += Get-ChildItem *.csproj -Path Servers -Recurse
    $projects.FullName | ForEach-Object -Process { Set-Location (Split-path $_); & dotnet restore "$_" --configfile "$config"}
}
finally {
    Pop-Location
}