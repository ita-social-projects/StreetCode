<#PSScriptInfo
.VERSION 1.0.0
.GUID 8115a829-e7dc-4cee-bc7e-f495471c29ae
.AUTHOR Henrik Lau Eriksson
.COMPANYNAME
.COPYRIGHT
.TAGS .NET Test Rerun Retry
.LICENSEURI
.PROJECTURI https://github.com/hlaueriksson/ConductOfCode
.ICONURI
.EXTERNALMODULEDEPENDENCIES
.REQUIREDSCRIPTS
.EXTERNALSCRIPTDEPENDENCIES
.RELEASENOTES
#>

<#
    .Synopsis
    Runs tests in a given path, and reruns the tests that fails.

    .Description
    Runs "dotnet test" and use the trx logger result file to collect failed tests.
    Then reruns the failed tests and reports the final result.

    .Parameter Path
    Path to the: project | solution | directory | dll | exe
    https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test#arguments

    .Parameter Configuration
    Build configuration for environment specific appsettings.json file.

    .Parameter Filter
    Filter to run selected tests based on: TestCategory | Priority | Name | FullyQualifiedName
    https://learn.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests

    .Parameter Settings
    Path to the .runsettings file.
    https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file

    .Parameter Retries
    Number of retries for each failed test.

    .Parameter Percentage
    Required percentage of passed tests.

    .Example
    .\test.ps1 -filter "TestCategory=RegressionTest"
    # Runs regression tests

    .Example
    .\test.ps1 .\FooBar.Tests\FooBar.Tests.csproj -filter "TestCategory=SmokeTest" -configuration "Development"
    # Runs FooBar smoke tests in Development

    .Example
    .\test.ps1 -retries 1 -percentage 95
    # Retries failed tests once and reports the run as green if 95% of the tests passed

    .Example
    .\test.ps1 -settings .\test.runsettings
    # Runs tests configured with a .runsettings file

    .Link
    https://github.com/hlaueriksson/ConductOfCode
#>
param (
    [string]$path = ".",

    [ValidateSet("Debug", "Release", "Development", "Production")]
    [string]$configuration = "Debug",

    [string]$filter,

    [string]$settings,

    [ValidateRange(1, 9)]
    [int]$retries = 2,

    [ValidateRange(0, 100)]
    [int]$percentage = 100
)

function Get-Option {
    param (
        [Parameter(Mandatory)]
        [string]$option,

        [string]$value
    )

    if ($value.Length -gt 0) {
        "$option $value"
    }
    else {
        [string]::Empty
    }
}

function Green {
    process { Write-Host $_ -ForegroundColor Green }
}

function Red {
    process { Write-Host $_ -ForegroundColor Red }
}

function Get-Elapsed {
    param (
        [Parameter(Mandatory)]
        [System.Diagnostics.Stopwatch]$timer
    )

    if ($timer.Elapsed.TotalHours -gt 1) {
        "$($timer.Elapsed.TotalHours.ToString("0.0000")) Hours"
    }
    elseif ($timer.Elapsed.TotalMinutes -gt 1) {
        "$($timer.Elapsed.TotalMinutes.ToString("0.0000")) Minutes"
    }
    else {
        "$($timer.Elapsed.TotalSeconds.ToString("0.0000")) Seconds"
    }
}

$timer = New-Object System.Diagnostics.Stopwatch
$timer.Start()

dotnet build $path --configuration $configuration

if (!$?) {
    # Build FAILED.
    Exit $LastExitCode
}

$currentPath = (Get-Item .).FullName
$options = $("--no-build --configuration $configuration $(Get-Option "--filter" $filter) $(Get-Option "--settings" $settings)").Split(' ')

dotnet test $path $options

if ($?) {
    # Passed!
    Exit
}

[xml]$testResults = Get-Content -Path $testResultsPath

$failedUnitTestResults = $testResults.TestRun.Results.UnitTestResult | Where-Object outcome -eq 'Failed'
$unitTests = $testResults.TestRun.TestDefinitions.UnitTest
$counters = $testResults.TestRun.ResultSummary.Counters
$passedTests = New-Object Collections.Generic.List[string]
$failedTests = New-Object Collections.Generic.List[string]

Write-Output "`r`nRetry $($counters.failed) failed tests..."

foreach ($failed in $failedUnitTestResults) {
    $unitTest = $unitTests | Where-Object id -eq $failed.testId | Select-Object -First 1
    $fqn = "$($unitTest.TestMethod.className).$($unitTest.TestMethod.name)"

    Write-Output "`r`nRetry $fqn..."

    # Retry
    for ($i = 1; $i -le $retries; $i++) {
        $options = $("--no-build --logger `"console;verbosity=detailed`" --configuration $configuration --filter FullyQualifiedName=$fqn $(Get-Option "--settings" $settings)").Split(' ')
        $escapeparser = '--%'
        $parameters = "-- TestRunParameters.Parameter(name=\`"Retry\`", value=\`"$i\`")"

        dotnet test $path $options $escapeparser $parameters

        if ($?) {
            # Passed!
            $passedTests.Add($fqn)
            break
        }
    }

    if (!$?) {
        # Failed!
        $failedTests.Add($fqn)
    }
}

$timer.Stop()

$successPercentage = (($counters.executed - $failedTests.Count) * 100) / [int]$counters.executed

if ($failedTests.Count -eq 0 -or $successPercentage -ge $percentage) {
    # Passed!
    Write-Output "`r`nTest Rerun Successful." | Green
}
else {
    # Failed!
    Write-Output "`r`nTest Rerun Failed." | Red
}

Write-Output "Total tests: $($counters.executed)"
Write-Output "    Reruned: $($counters.failed)"
Write-Output "     Passed: $($counters.executed - $failedTests.Count) " | Green
Write-Output "     Failed: $($failedTests.Count)" | Red
if ($percentage -lt 100 -and $successPercentage -ge $percentage) {
    Write-Output "  Success %: $successPercentage (>= $percentage)" | Green
}
if ($percentage -lt 100 -and $successPercentage -lt $percentage) {
    Write-Output "  Success %: $successPercentage (< $percentage)" | Red
}
Write-Output " Total time: $(Get-Elapsed $timer)"

Write-Output "`r`nReruned:"

foreach ($passed in $passedTests) {
    Write-Output "Passed $passed" | Green
}

foreach ($fail in $failedTests) {
    Write-Output "Failed $fail" | Red
}

if ($failedTests.Count -eq 0 -or $successPercentage -ge $percentage) {
    # Passed!
    Exit
}

# Failed!
Exit $failedTests.Count