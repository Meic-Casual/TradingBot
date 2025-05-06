param (
    [string]$designDb = "dca_bot_design",   # Source schema (design table)
    [string]$devDb = "dca_bot_dev",         # Migration target (development table)
    [string]$msg = ""                       # Optional migration message
)

# === CONFIG ===
$contextName = "DcaBotContext"
$modelsFolder = "../Models/Scaffolded"
$migrationsFolder = "Migrations"
$user = "user"
$pass = "password"
$provider = "Pomelo.EntityFrameworkCore.MySql"

# === SAFETY GUARD ===
$protectedSources = @("dca_bot_prod", "production", "main")
if ($protectedSources -contains $designDb.ToLower()) {
    Write-Host "❌ ERROR: Scaffolding from production DB '$designDb' is blocked!" -ForegroundColor Red
    return
}

# === MIGRATION NAME FORMAT ===
if ([string]::IsNullOrWhiteSpace($msg)) {
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $msg = "Migration_$timestamp"
} else {
    # Replace spaces and illegal chars for migration name
    $msg = $msg -replace '[^a-zA-Z0-9_]', '_'
}

# === STEP 1: Scaffold from DESIGN DB ===
$sourceCs = "server=localhost;database=$designDb;user=$user;password=$pass;"
Write-Host "🔄 Scaffolding from DESIGN DB: '$designDb'..."
Scaffold-DbContext $sourceCs $provider `
  -OutputDir $modelsFolder `
  -Context $contextName `
  -Force `
  -NoOnConfiguring

# === STEP 2: Set TARGET (DEV DB) for migration ===
[System.Environment]::SetEnvironmentVariable("TARGET_DB", $devDb, "Process")

# === STEP 3: Add migration ===
Write-Host "🧱 Creating migration: '$msg' targeting DEV DB: '$devDb'..."
Add-Migration $msg -Context $contextName -OutputDir $migrationsFolder

# === STEP 4: Apply migration to DEV DB ===
Write-Host "🚀 Applying migration to DEV DB: '$devDb'..."
Update-Database -Context $contextName

Write-Host "`✅ Migration '$msg' has been applied to '$devDb'."