param(
    [Parameter(Position = 0)]
    [string]$Command,

    [Parameter(Position = 1)]
    [string]$Suite,

    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$RemainingArgs
)

function Show-Help {
    Write-Host ""
    Write-Host "Usage:"
    Write-Host "  .\prep.ps1                 Show this help"
    Write-Host "  .\prep.ps1 help            Show this help"
    Write-Host "  .\prep.ps1 app             Run the .NET app locally"
    Write-Host "  .\prep.ps1 docker         Start the docker compose stack"
    Write-Host "  .\prep.ps1 test           Run all test suites"
    Write-Host "  .\prep.ps1 test app       Run only the app test suite"
    Write-Host "  .\prep.ps1 test data      Run only the data test suite"
    Write-Host "  .\prep.ps1 test --filter \"SomeTestName\""
    Write-Host "  .\prep.ps1 test app --filter \"SomeTestName\""
    Write-Host "  .\prep.ps1 test data --filter \"SomeTestName\""
    Write-Host ""
    Write-Host "Notes:"
    Write-Host "  - app runs the main web app with dotnet run"
    Write-Host "  - docker runs the compose development stack"
    Write-Host "  - test routes to the appropriate csproj"
    Write-Host "  - --filter is passed through to dotnet test"
    Write-Host ""
}

function Get-TestFilter {
    param([string[]]$Args)

    if ($null -eq $Args -or $Args.Count -eq 0) {
        return $null
    }

    $filterIndex = [Array]::IndexOf($Args, "--filter")
    if ($filterIndex -lt 0) {
        return $null
    }

    if ($filterIndex + 1 -ge $Args.Count) {
        throw "Missing value after --filter"
    }

    return $Args[$filterIndex + 1]
}

switch ($Command) {
    "" {
        Show-Help
        exit 0
    }

    "help" {
        Show-Help
        exit 0
    }

    "app" {
        Write-Host "Starting app..."
        dotnet run --project ".\Dfe.PrepareConversions\Dfe.PrepareConversions\Dfe.PrepareConversions.csproj"
        exit $LASTEXITCODE
    }

    "docker" {
        Write-Host "Starting docker compose..."
        docker compose -f ".\docker-compose.development.yml" up
        exit $LASTEXITCODE
    }

    "test" {
        $filter = Get-TestFilter -Args $RemainingArgs

        $projects = @{
            "app" = ".\Dfe.PrepareConversions\Dfe.PrepareConversions.Tests\Dfe.PrepareConversions.Tests.csproj"
            "data" = ".\Dfe.PrepareConversions\Dfe.PrepareConversions.Data.Tests\Dfe.PrepareConversions.Data.Tests.csproj"
        }

        if ([string]::IsNullOrWhiteSpace($Suite)) {
            foreach ($project in $projects.Values) {
                if ($filter) {
                    dotnet test $project --filter $filter
                }
                else {
                    dotnet test $project
                }
            }
            exit $LASTEXITCODE
        }

        $suiteKey = $Suite.ToLowerInvariant()

        if (-not $projects.ContainsKey($suiteKey)) {
            Write-Error "Unknown test suite: '$Suite'. Valid suites: app, data"
            Show-Help
            exit 1
        }

        $projectPath = $projects[$suiteKey]

        if ($filter) {
            dotnet test $projectPath --filter $filter
        }
        else {
            dotnet test $projectPath
        }

        exit $LASTEXITCODE
    }

    default {
        Write-Error "Unknown command: '$Command'"
        Show-Help
        exit 1
    }
}
