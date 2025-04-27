Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    # Lade den Inhalt der .csproj-Datei
    $content = Get-Content $_.FullName

    # Entferne vorhandene <TargetFrameworkVersion> und <TargetFramework>-Zeilen
    $content = $content -replace '<TargetFrameworkVersion>.*?</TargetFrameworkVersion>', ''
    $content = $content -replace '<TargetFramework>.*?</TargetFramework>', ''
    $content = $content -replace '<TargetFrameworkProfile>.*?</TargetFrameworkProfile>', ''

    # Füge die neuen Zeilen ein
    $newLines = @"
    <TargetFrameworkVersion>v4.8.1</TargetFrameworkVersion>
    <TargetFramework>net481</TargetFramework>
"@

    # Füge die neuen Zeilen in die PropertyGroup ein
    $content = $content -replace '<PropertyGroup>', "<PropertyGroup>`n$newLines"

    # Schreibe den aktualisierten Inhalt zurück in die Datei
    Set-Content $_.FullName $content
}