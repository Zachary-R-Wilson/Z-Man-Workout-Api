#get database password from .env
$envFilePath = ".\.env"
$envVars = Get-Content -Path $envFilePath | Where-Object { $_ -match "^\s*[^#].*=\s*.*" }
$saPassword = ($envVars | Where-Object { $_ -match "^SA_PASSWORD=" }) -replace "^SA_PASSWORD=", ""

#initialize database and create tables
$path = ".\WorkoutApi\WorkoutApi\Repositories\Sql\"
$serverInstance = "localhost,1433"
$database = "master"
$username = "sa"
$sqlFile = $path + "InitDatabase.sql"

$connectionString = "Server=$serverInstance;Database=$database;User Id=$username;Password=$saPassword;TrustServerCertificate=True;"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop

#switch databases and create stored procedures
$database = "workoutdb"
$connectionString = "Server=$serverInstance;Database=$database;User Id=$username;Password=$saPassword;TrustServerCertificate=True;"

$sqlFile = $path + "AuthUser.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "CreateDay.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "CreateExercise.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "CreateWorkout.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "DeleteWorkout.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "GetAllWorkouts.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "GetMaxes.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "getProgress.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "InsertTracking.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "RegisterUser.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "UpdateMaxes.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop
$sqlFile = $path + "verifyUserExercise.sql"
$sqlQuery = Get-Content -Path $sqlFile -Raw
Invoke-Sqlcmd -Query $sqlQuery -ConnectionString $connectionString -ErrorAction Stop

# Start-Sleep 10