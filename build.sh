#!/bin/bash

TARGET="default"
VERBOSITY="verbose"
DRYRUN=
SHOW_VERSION=false

SCRIPT_NAME="build.cake"
TOOL_PATH="dependencies/Nancy/tools"
NUGET_PATH="nuget/NuGet.exe"
CAKE_VERSION="0.13.0"
CAKE_PATH="$TOOL_PATH/Cake.$CAKE_VERSION/Cake.exe"

SCRIPT_ARGUMENTS=()

# Parse arguments.
for i in "$@"; do
    case $1 in
        --target ) TARGET="$2"; shift ;;
        -s|--script) SCRIPT_NAME="$2"; shift ;;
        -v|--verbosity) VERBOSITY="$2"; shift ;;
        -d|--dryrun) DRYRUN="-dryrun" ;;
        --version) SHOW_VERSION=true ;;
        --) shift; SCRIPT_ARGUMENTS+=("$@"); break ;;
        *) SCRIPT_ARGUMENTS+=("$1") ;;
    esac
    shift
done

function installdotnet() {
  if [ ! $(dotnetinstalled dotnet) ]
    then
      echo "Installing dotnet"
      apt-get install curl
      curl -sSL https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0/scripts/obtain/dotnet-install.sh | bash /dev/stdin --channel beta --version 1.0.0-preview1-002702 --install-dir .dotnet
      export PATH=.dotnet:$PATH
  fi
}

function dotnetinstalled() {
  # set to 1 initially
  local return_=1
  # set to 0 if not found
  type $1 >/dev/null 2>&1 || { local return_=0; }
  # return value
  echo "$return_"
}

function installcake() {
  echo "Checking for Cake at "$CAKE_PATH
  if [ ! -f $CAKE_PATH ]; then
    echo "Installing Cake"
    mono $TOOL_PATH/$NUGET_PATH install Cake -Version $CAKE_VERSION -OutputDirectory $TOOL_PATH
  fi
}

function runbuildscript() {
  if $SHOW_VERSION; then
      mono $CAKE_PATH -version
  else
      echo "Executing "mono $CAKE_PATH $SCRIPT_NAME -target=$TARGET -verbosity=$VERBOSITY $DRYRUN "${SCRIPT_ARGUMENTS[@]}"
      mono $CAKE_PATH $SCRIPT_NAME -target=$TARGET -verbosity=$VERBOSITY $DRYRUN "${SCRIPT_ARGUMENTS[@]}"
  fi
}


installdotnet
installcake
runbuildscript