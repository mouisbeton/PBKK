#!/usr/bin/env bash
set -euo pipefail
project_dir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
workspace_dir="$(cd -- "$project_dir/../../.." && pwd)"
export DOTNET_CLI_HOME="$workspace_dir/.tools/cli-home"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export NUGET_PACKAGES="$workspace_dir/.tools/nuget"
if command -v dotnet >/dev/null 2>&1; then dotnet_bin="$(command -v dotnet)";
else dotnet_bin="$workspace_dir/.tools/dotnet/dotnet"; fi
case "${1:-DataMahasiswa}" in
  sdk) exec "$dotnet_bin" --info ;;
  HelloDotNet|DataMahasiswa) exec "$dotnet_bin" run --project "$project_dir/${1:-DataMahasiswa}" ;;
  *) echo "Gunakan: bash run.sh HelloDotNet | DataMahasiswa | sdk"; exit 1 ;;
esac
