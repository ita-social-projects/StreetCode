#!/bin/bash
set -e

echo "===== Starting dbupdate ====="
echo " Date & Time: $(date)"
echo " Working Directory: $(pwd)"
echo " Directory Listing:"
ls -la

echo ""
echo " Network Info:"
ip a
echo ""
echo "  Routing Table:"
ip route

echo ""
echo " Checking DNS resolution (host.docker.internal):"
getent hosts host.docker.internal || echo " Could not resolve host.docker.internal"

echo ""
echo " Environment Variables (filtered):"
printenv | grep -i connection || echo " No connection string in environment"
printenv | grep -i sql || true

echo ""
echo " Checking connection string..."
if [ -z "$STREETCODE_ConnectionStrings__DefaultConnection" ]; then
  echo " No connection string found, using default connection string from environment"
else
  echo " Using provided connection string"
fi

echo ""
echo " Checking if DbUpdate.dll exists..."
if [ ! -f /app/DbUpdate.dll ]; then
  echo " ERROR: DbUpdate.dll not found!"
  exit 1
else
  echo " DbUpdate.dll found"
fi

echo ""
echo " Connection string (masked password):"
echo "$STREETCODE_ConnectionStrings__DefaultConnection" | sed -E 's/(Password=)[^;]*/\1***HIDDEN***/'

echo ""
echo " Running dotnet with diagnostics enabled..."
# Enable verbose dotnet diagnostics
export DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_PRINT_TELEMETRY_MESSAGE=false
export DOTNET_EnableDiagnostics=1

#echo "Mock failure in dbupdate..."
#exit 1

echo "========== [RUNNING DBUPDATE] =========="

export DOTNET_LOG_LEVEL=Trace
export DOTNET_TRACE=debug

dotnet /app/DbUpdate.dll | tee /app/dbupdate.log

echo ""
echo " Checking contents of dbupdate.log..."
cat /app/dbupdate.log || echo " dbupdate.log not found or empty."

echo ""
echo " Done."