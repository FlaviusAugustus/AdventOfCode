param(
    [ValidateRange(2015, 2100)]
    [string]$Year = (Get-Date).year.ToString(),
    [ValidateRange(1, 25)]
    [string]$Day = (Get-Date).day.ToString()
)

$dayString = "Day$($Day.PadLeft(2,'0'))"
$filePath = ".\$($Year)\$($dayString).cs"
$baseFilePath = ".\dayBase"

(Get-Content $baseFilePath).Replace('[_YEAR]', "_$($Year)") | Set-Content $filePath
(Get-Content $filePath).Replace('[DAY]', "$($dayString)") | Set-Content $filePath
